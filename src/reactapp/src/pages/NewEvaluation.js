// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

import React, { useState, useEffect } from "react";
import Select from 'react-select';
import {
  FormControl,
  FormLabel,
  Heading,
  Box,
  Button,
  ButtonGroup,
  Flex,
  Skeleton,
  Stack,
  useColorModeValue,
} from "@chakra-ui/react";
import { useNavigate } from 'react-router-dom';
import { AlertMessage } from "../components/AlertMessage";
import { get } from "../components/FetchHelpers"

export default function NewEvaluation() {
  const [selectedEvaluation, setSelectedEvaluation] = useState(null);
  const [selectedCandidate, setSelectedCandidate] = useState(null);
  const [candidateHasPerformedEvaluation, setCandidateHasPerformedEvaluation] = useState(true);
  const [showCandidateHasPerformedEvaluationMessage, setShowCandidateHasPerformedEvaluationMessage] = useState(false);
  const [evaluationData, setEvaluationsData] = useState([]);
  const [candidateData, setCandidateData] = useState([]);
  const [pageDataLoad, setPageDataLoad] = useState(false);

  useEffect(() => {
    fetchAvailableEvaluations();
    fetchCandidates();
    setPageDataLoad(true);
  }, []);

  const navigate = useNavigate();


  const fetchAvailableEvaluations = async () => {
    try {
      const response = await get("/api/PerformanceEvaluation");
      if (!response.ok) {
        throw new Error("Failed to fetch performance evaluations");
      }

      // Check if the response is valid JSON
      const contentType = response.headers.get("Content-Type");
      if (!contentType || !contentType.includes("application/json")) {
        throw new Error("Response is not valid JSON");
      }

      setEvaluationsData(await response.json());

    }
    catch (error) {
      console.error("Error fetching performance evaluations:", error);
    }
  }

  const fetchCandidates = async () => {
    try {
      const response = await get("/api/Candidate");
      if (!response.ok) {
        throw new Error("Failed to fetch candidates");
      }

      // Check if the response is valid JSON
      const contentType = response.headers.get("Content-Type");
      if (!contentType || !contentType.includes("application/json")) {
        throw new Error("Response is not valid JSON");
      }

      setCandidateData(await response.json());

    }
    catch (error) {
      console.error("Error fetching candidates:", error);
    }
  }

  const fetchHasCandidateThePerformedEvaluation = async (personId, evaluationId) => {
    try {
      const response = await get(`/api/candidateHasPerformedEvaluation?personId=${personId}&evaluationId=${evaluationId}`);

      if (!response.ok) {
        throw new Error("Failed to fetch performed evaluations per candidate");
      }

      return response.json();

      //var result = await response.json();
    }
    catch (error) {
      console.error("Error fetching performed evaluations per candidate:", error);
    }
  };

  /**
   * Event that updates Evaluation
   */
  const selectedEvaluationOnChange = async (evaluation) => {
    setSelectedEvaluation(evaluation);
    setCandidateHasPerformedEvaluation(true);

    if (selectedCandidate && evaluation) {
      var result = await fetchHasCandidateThePerformedEvaluation(selectedCandidate.personId, evaluation.id);
      setCandidateHasPerformedEvaluation(result.candidateHasPerformedEvaluation);
      setShowCandidateHasPerformedEvaluationMessage(result.candidateHasPerformedEvaluation);
    }
  };

  /**
   * Event that updates Candidate
   */
  const selectedCandidateOnChange = async (candidate) => {
    setSelectedCandidate(candidate);
    setCandidateHasPerformedEvaluation(true);

    if (candidate && selectedEvaluation) {
      var result = await fetchHasCandidateThePerformedEvaluation(candidate.personId, selectedEvaluation.id);
      setCandidateHasPerformedEvaluation(result.candidateHasPerformedEvaluation);
      setShowCandidateHasPerformedEvaluationMessage(result.candidateHasPerformedEvaluation);
    }
  };

  return (
    <Skeleton isLoaded={pageDataLoad} >
    <Flex minH={"100vh"} align={"center"} justify={"center"}>
      <Stack spacing={8} mx={"auto"} maxW={"lg"} py={12} px={6}>
        <Stack align={"center"}>
          <Heading fontSize={"6xl"}>New Evaluation</Heading>
        </Stack>
        <Box
          rounded={"lg"}
          bg={useColorModeValue("white", "gray.700")}
          boxShadow={"lg"}
          alignItems={"center"}
          w="100%"
          p={8}
        >
          <Stack spacing={4}>
            <FormControl>
              <FormLabel>Evaluation</FormLabel>
              <Select
                defaultValue={selectedEvaluation}
                onChange={(evaluation) => selectedEvaluationOnChange(evaluation)}
                options={evaluationData}
                getOptionValue={option => option.id}
                getOptionLabel={option => option.performanceEvaluationTitle}
              />

              <FormLabel>Candidate</FormLabel>
              <Select
                defaultValue={selectedCandidate}
                onChange={(candidate) => selectedCandidateOnChange(candidate)}
                options={candidateData}
                getOptionValue={option => option.personId}
                getOptionLabel={option => option.candidateName}
              />

              {/*<FormLabel>Date</FormLabel>*/}
              {/*<Input*/}
              {/*  placeholder="Select Date"*/}
              {/*  size="md"*/}
              {/*  type="date"*/}
              {/*/>*/}
            </FormControl>
              <Box mt="0" textAlign="center">
                { showCandidateHasPerformedEvaluationMessage
                  && (<AlertMessage message="There is already an evaluation for this candidate." />)
                }
              </Box>
            <Box mt="5" textAlign="center">
              <ButtonGroup variant="outline" spacing="6">
                  <Button isDisabled={!(selectedEvaluation && selectedCandidate) || candidateHasPerformedEvaluation}
                  onClick={() => {
                    navigate("/evaluation", { state: { candidate: selectedCandidate, evaluation: selectedEvaluation } })
                  }}
                  colorScheme="blue"
                >
                  Start Evaluation
                </Button>
                <Button
                  onClick={() => {
                   navigate("/main");
                  }}
                >
                  Cancel
                </Button>
              </ButtonGroup>
            </Box>
          </Stack>
        </Box>
      </Stack>
      </Flex>
    </Skeleton>
  );
}
