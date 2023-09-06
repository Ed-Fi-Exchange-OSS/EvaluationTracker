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
  Input,
  Box,
  Button,
  ButtonGroup,
  Flex,
  Stack,
  useColorModeValue,
} from "@chakra-ui/react";
import { useNavigate } from 'react-router-dom';

export default function NewEvaluation() {
  const [selectedEvaluation, setSelectedEvaluation] = useState(null);
  const [selectedCandidate, setSelectedCandidate] = useState(null);
  const [evaluationData, setEvaluationsData] = useState([]);
  const [candidateData, setCandidateData] = useState([]);

  useEffect(() => {
    fetchAvailableEvaluations();
    fetchCandidates();
  }, []);

  const navigate = useNavigate();

  const validate = () => {
    return false;
  };

  const fetchAvailableEvaluations = async () => {
    try {
      const response = await fetch("https://localhost:7065/api/PerformanceEvaluation");
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
      const response = await fetch("https://localhost:7065/api/Candidate");
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

  return (
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
                onChange={setSelectedEvaluation}
                options={evaluationData}
                getOptionValue={option => option.value}
                getOptionLabel={option => option.performanceEvaluationTitle}
              />

              <FormLabel>Candidate</FormLabel>
              <Select
                defaultValue={selectedCandidate}
                onChange={setSelectedCandidate}
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

            <Box mt="5" textAlign="center">
              <ButtonGroup variant="outline" spacing="6">
                <Button isDisabled={!(selectedEvaluation && selectedCandidate)}
                  onClick={() => {
                    navigate("/evaluation", { state: { candidate: selectedCandidate, evaluation: selectedEvaluation } })
                  }}
                  colorScheme="blue"
                >
                  Start Evaluation
                </Button>
                <Button
                  onClick={() => {
                    window.location.href = "/main";
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
  );
}
