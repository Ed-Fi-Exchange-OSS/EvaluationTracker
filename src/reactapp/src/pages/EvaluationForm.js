// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

import {
  Box,
  Container,
  Stack,
  Text,
  VStack,
  Button,
  Heading,
  StackDivider,
  useColorModeValue,
  HStack,
  FormControl,
  FormLabel,
  ButtonGroup,
  Table,
  Textarea,
  Thead,
  Tbody,
  Tr,
  Th,
  Td,
} from "@chakra-ui/react";
import "../App.css";
import React, { useEffect, useState } from "react";
import { useLocation } from "react-router-dom";
import DatePicker from "react-datepicker";
import Select from 'react-select';
import { get, post } from "../components/FetchHelpers";
import { getLoggedInUserName, getLoggedInUserRole, getLoggedInUserId } from "../components/TokenHelpers";
import "react-datepicker/dist/react-datepicker.css";

export default function EvaluationForm() {
  const [loggedInUser, setLoggedInUser] = useState({ "name": "", "role": null });
  const [selectedCandidate, setSelectedCandidate] = useState({});
  const [selectedEvaluation, setSelectedEvaluation] = useState({});
  const [ratingLevelOptions, setRatingLevelOptions] = useState([]);
  const [evaluationMetadata, setEvaluationMetadata] = useState({});

  const [evaluationDate, setEvaluationDate] = useState(new Date());
  const [evaluationEndTime, setEvaluationEndTime] = useState(null);
  const [elementRatings, setElementRatings] = useState([]);
  const [objectiveNotes, setObjectiveNotes] = useState([]);


  const location = useLocation();

  // This will be replaced with data from the ODS/API once EPPETA-38 is complete
  const ratingLevels = [
    {
      "codeValue": "Improvement Needed",
      "maxRating": 1
    },
    {
      "codeValue": "Developing",
      "maxRating": 2
    },
    {
      "codeValue": "Proficient",
      "maxRating": 3
    },
    {
      "codeValue": "Acomplished",
      "maxRating": 4
    },
    {
      "codeValue": "Distinguished",
      "maxRating": 5
    }
  ];

  useEffect(() => {
    setLoggedInUser({
      "name": getLoggedInUserName(),
      "role": getLoggedInUserRole()
    });

    if (location?.state?.candidate) {
      sessionStorage.setItem("candidate", JSON.stringify(location.state.candidate));
      setSelectedCandidate(location.state.candidate);
    } else {
      setSelectedCandidate(JSON.parse(sessionStorage.getItem("candidate")));
    }

    if (location?.state?.evaluation) {
      sessionStorage.setItem("evaluation", JSON.stringify(location.state.evaluation));
      setSelectedEvaluation(location.state.evaluation);
    } else {
      setSelectedEvaluation(JSON.parse(sessionStorage.getItem("evaluation")));
    }
  }, []);

  const processRatingLevelOptions = async (ratingLevels) => {
    const processedRatingLevels = [{ "label": "N/A - Not Applicable", "value": -1 }, ...ratingLevels.map((level) => {
      return {
        "label": level.codeValue,
        "value": level.maxRating
      }
    })];
    setRatingLevelOptions(processedRatingLevels); // Save to state
  }

  useEffect(() => {
    fetchEvaluationObjectives();
  }, [selectedEvaluation]);

  // Retrieve evaluation objectives from API
  const fetchEvaluationObjectives = async () => {
    try {
      const response = await get(`/api/Evaluation/${selectedEvaluation.id}`);

      if (!response.ok) {
        throw new Error("Failed to fetch evaluation objectives");
      }

      // Check if the response is valid JSON
      const contentType = response.headers.get("Content-Type");
      if (!contentType || !contentType.includes("application/json")) {
        throw new Error("Response is not valid JSON");
      }

      const evaluationData = await response.json();

      processRatingLevelOptions(ratingLevels);
      setEvaluationMetadata(evaluationData);
    } catch (error) {
      console.error("Error fetching evaluation objectives:", error);
    }
  };

  const saveEvaluation = async () => {
    const completedEvaluation = {
    };

    completedEvaluation.reviewedPersonId = selectedCandidate.personId;
    completedEvaluation.reviewedPersonSourceSystemDescriptor = selectedCandidate.sourceSystemDescriptor;
    completedEvaluation.reviewedPersonName = selectedCandidate.candidateName;
    completedEvaluation.performanceEvaluationId = selectedEvaluation.id;
    completedEvaluation.startDateTime = evaluationDate;
    !evaluationEndTime ?
      completedEvaluation.endDateTime = new Date() : completedEvaluation.endDateTime = evaluationEndTime;

    completedEvaluation.objectiveResults = evaluationMetadata.flatMap((objective) => {

      // Map elements for this objective - Using flat map will either map the element if a rating exists
      // or will return empty if no score was provided for the element (i.e. N/A was chosen)
      const ratings = objective.evaluationElements.flatMap((element) => {
        const elementRatingValue = elementRatings.find((rating) => rating.name === element.evaluationElementId);
        return elementRatingValue?.value > -1 ? { id: element.evaluationElementId, score: elementRatingValue?.value } : [];
      });

      const objectiveNote = objectiveNotes.find((note) => note.objectiveId == objective.evaluationObjectiveId);

      const objectiveRating = {
        id: objective.evaluationObjectiveId,
        comment: objectiveNote?.value,
        elements: ratings
      }
      // If there are no score for the elements for this objective and no comment, ignore
      return (objectiveRating.comment || ratings.length > 0) ? objectiveRating : [];
    });

    try {
      const response = await post(`/api/EvaluationRating?userId=${getLoggedInUserId()}`, completedEvaluation);
    } catch (e) {
    }
  }

  // Rating drop down event handler
  const setRatingLevel = (e, actionProps) => {
    const elementRatingCopy = [...elementRatings];
    const rating = { "name": actionProps.name, "value": e.value };

    const locatedIndex = elementRatingCopy.findIndex((element) => element.name === actionProps.name);
    locatedIndex >= 0 ? elementRatingCopy[locatedIndex] = rating : elementRatingCopy.push(rating);

    setElementRatings(elementRatingCopy);
  };

  const handleNotesUpdates = (e) => {
    const objectiveNotesCopy = [...objectiveNotes];
    const note = { "objectiveId": e.target.id, "name": e.target.name, "value": e.target.value };

    const locatedIndex = objectiveNotesCopy.findIndex((objective) => objective.name === e.target.name && objective.objectiveId === e.target.id);
    locatedIndex >= 0 ? objectiveNotesCopy[locatedIndex] = note : objectiveNotesCopy.push(note);

    setObjectiveNotes(objectiveNotesCopy);
  };

  return (
    <Container maxW={"7xl"} mb='10'>
      <Stack
        spacing={{ base: 4, sm: 6 }}
        direction={"column"}
        divider={
          <StackDivider
            borderColor={useColorModeValue("gray.200", "gray.600")}
          />
        }
      >
        <VStack spacing={{ base: 4, sm: 2 }}>
          <Heading
            lineHeight={1.1}
            mt={5}
            mb={5}
            fontSize={"3xl"}
            fontWeight={"700"}
          >
            Evaluation Entry
          </Heading>
          <HStack spacing="0px" mb="5" className="responsiveHStack">
            <Box className="TitleBox">Evaluation</Box>
            <Box className="Box">{selectedEvaluation.performanceEvaluationTitle}</Box>
            <Box className="TitleBox">Candidate</Box>
            <Box className="Box">{selectedCandidate?.candidateName}</Box>
          </HStack>
          <HStack spacing="0px" mb="5" className="responsiveHStack">
            <Box className="TitleBox">Date</Box>
            <Box className="Box">{evaluationDate?.toLocaleDateString()}</Box>
            <Box className="TitleBox">Evaluator</Box>
            <Box className="Box">{loggedInUser.name}</Box>
          </HStack>
          <HStack display='flex' spacing="20px" mb="5">
          </HStack>
          <Text fontSize={"sm"}>
            The following clinical teacher evaluation form is
            divided into four domains as adopted by the State Board
            of Education. The Dimensions within each domain ensure
            clinical teachers have the knowledge and skills to teach
            in Texas public schools. Please complete the form by
            checking the appropriate box. Use Not Applicable (NA)
            when the element is not observed or is irrelevant to the
            particular setting/observation/evaluation.
          </Text>
          <Text fontSize={"sm"} as='b'>
            SCALE: {ratingLevelOptions.map((item) => {
              if (item.value) {
                return `${item.value} - ${item.label} `
              }
              return `${item.label} `
            })}
          </Text>
          <Text fontSize={"sm"}>
            ** Requires written “COMMENTS” specifying observed, shared or recorded evidence if scoring 2=Needs Improvement
          </Text>
          <Text fontSize={"sm"}>
            * Proficient is the goal N/A= Not Applicable (N/A)
          </Text>

          <HStack display='flex' spacing="20px" mb="5" mt="5">
            <FormControl style={{ width: '300px' }}>
              <FormLabel>Pre-Conference Start Time</FormLabel>
              <DatePicker selected={evaluationDate} dateFormat="MM/dd/yy hh:mm" timeFormat="hh:mm" showTimeSelect={true} onChange={(date) => setEvaluationDate(date)} />
            </FormControl>
            <FormControl style={{ width: '300px' }}>
              <FormLabel>End Time</FormLabel>
              <DatePicker selected={evaluationEndTime} dateFormat="hh:mm" timeFormat="hh:mm" showTimeSelect={true} showTimeSelectOnly={true} onChange={(date) => setEvaluationEndTime(date)} />
            </FormControl>
          </HStack>
        </VStack>
        <Box>
          <Box>
            {Object.entries(evaluationMetadata).map(([index, objective]) => (
              <Box key={objective.evaluationObjectiveId} mt={4}>
                <Heading fontSize="lg" fontWeight="bold">{objective.name}</Heading>
                <Table cellSpacing="10" cellPadding="5">
                  <Thead>
                    <Tr>
                      <Th minWidth="200px">Objective</Th>
                      <Th minWidth="150px">Rating</Th>
                      <Th>Comments</Th>
                    </Tr>
                  </Thead>
                  <Tbody>
                    {objective.evaluationElements.map((element, index) => (
                      <Tr key={index}>
                        <Td maxWidth="200px">{element.name}</Td>
                        <Td maxWidth="150px">
                          <Select name={element.evaluationElementId} id={element.evaluationElementId} options={ratingLevelOptions}
                            onChange={(e, action) => { setRatingLevel(e, action) }}
                          />
                        </Td>
                        {index === 0 && <Td rowSpan="4"><Textarea id={objective.evaluationObjectiveId} name={objective.name} onBlur={handleNotesUpdates} rows={(objective.evaluationElements.length * 3) - 1} resize="none" borderColor="gray.300" /></Td>}
                      </Tr>
                    ))}
                  </Tbody>
                </Table>
              </Box>
            ))}
          </Box>
        </Box>
        <Box mt="0" textAlign="center">
        </Box>
      </Stack>
      <Box textAlign="center">
        <ButtonGroup variant="outline" spacing="6">
          <Button
            onClick={() => {
              saveEvaluation();
              window.location.href = "/main";
            }}
            colorScheme="blue"
          >
            Save Evaluation
          </Button>
          <Button
            onClick={() => {
              window.location.href = "/main";
            }}
          >
            Cancel
          </Button>
          {loggedInUser.role === 'Supervisor' && <Button
            onClick={() => {
              window.location.href = "/main";
            }}
          >
            Approve Evaluation
          </Button>}
        </ButtonGroup>
      </Box>
    </Container>
  );
}
