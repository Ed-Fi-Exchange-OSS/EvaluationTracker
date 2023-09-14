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
    Select,
    Input,
  ButtonGroup,
  Table,
  TableCaption,
  Thead,
  Tbody,
  Tr,
  Th,
  Td,
} from "@chakra-ui/react";
import "../App.css";
import React, { useEffect, useState } from "react";
import { useLocation } from "react-router-dom";

// Get the evaluationElementsDictionary from EvaluationController
export default function EvaluationForm() {
  const [evaluationElementsDictionary, setEvaluationElementsDictionary] = useState({});
  const [candidate, setCandidate] = useState({});
  const [evaluation, setEvaluation] = useState({});
  const [evaluationDate, setEvaluationDate] = useState({});

  const location = useLocation();

  useEffect(() => {
    setCandidate(location.state.candidate);
    setEvaluation(location.state.evaluation);

    fetchEvaluationObjectives();
    setEvaluationDate(new Date())
  }, []);

  // Retrieve evaluation objectives from API
  // Needed to fetch to localhost:7065 with the full URL to avoid CORS issues?
  const fetchEvaluationObjectives = async () => {
    try {
      const response = await fetch("https://localhost:7065/api/Evaluation");
      if (!response.ok) {
        throw new Error("Failed to fetch evaluation objectives");
      }

      // Check if the response is valid JSON
      const contentType = response.headers.get("Content-Type");
      if (!contentType || !contentType.includes("application/json")) {
        throw new Error("Response is not valid JSON");
      }

      const data = await response.json();
      setEvaluationElementsDictionary(data);
    } catch (error) {
      console.error("Error fetching evaluation objectives:", error);
    }
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
                        <Box className="Box">T-TESS</Box>
                        <Box className="TitleBox">Candidate</Box>
                        <Box className="Box">Bob Smith</Box>
            </HStack>
            <HStack spacing="0px" mb="5" className="responsiveHStack">
              <Box className="TitleBox">Date</Box>
              <Box className="Box">04-10-2023</Box>
              <Box className="TitleBox">Evaluator</Box>
              <Box className="Box"></Box>
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
                    SCALE: ** 1=Needs Improvement     2= Developing     *3= Proficient        N/A= Not Applicable (N/A)
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
                            <Input type="Datetime" />
                        </FormControl>


                        <FormControl style={{ width: '300px' }}>
                            <FormLabel>End Time</FormLabel>
                            <Input type="Datetime" />
                        </FormControl>
                    </HStack>
                </VStack>
          <Box>
            <Text
              fontSize={{ base: "16px", lg: "18px" }}
              color={useColorModeValue("yellow.500", "yellow.300")}
              fontWeight={"500"}
              textTransform={"uppercase"}
              mb={"4"}
            >
              Evaluation Objectives and Elements
            </Text>
            <Box>
              {Object.entries(evaluationElementsDictionary).map(([objectiveTitle, elementTitles]) => (
                <Box key={objectiveTitle} mt={4}>
                  <Heading fontSize="lg" fontWeight="bold">{objectiveTitle}</Heading>
                  <Table variant="striped" size="sm" mt={2}>
                    <TableCaption>SCALE: ** 1=Needs Improvement     2= Developing     *3= Proficient        N/A= Not Applicable (N/A)</TableCaption>
                    <Thead>
                      <Tr>
                        <Th minWidth="200px">Element Titles</Th>
                        <Th minWidth="150px">Rating</Th>
                        <Th>Comments</Th>
                      </Tr>
                    </Thead>
                    <Tbody>
                      {elementTitles.map((elementTitle, index) => (
                        <Tr key={index}>
                          <Td maxWidth="200px">{elementTitle}</Td>
                          <Td maxWidth="150px">
                            <Select placeholder="Select rating">
                              <option value="1">1 - Needs Improvement</option>
                              <option value="2">2 - Developing</option>
                              <option value="3">3 - Proficient</option>
                              <option value="N/A">N/A - Not Applicable</option>
                            </Select>
                          </Td>
                          <Td>
                            <Input borderColor="gray.300" type="text" />
                          </Td>
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
                                window.location.href = "/evaluation";
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
                    </ButtonGroup>
                </Box>
        </Container>
    );
}
