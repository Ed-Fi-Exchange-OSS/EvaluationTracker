import {
    Box,
    chakra,
    Container,
    Stack,
    Text,
    Image,
    Flex,
    VStack,
    Button,
    Heading,
    SimpleGrid,
    StackDivider,
    useColorModeValue,
    VisuallyHidden,
    List,
    ListItem,
    HStack,
    FormControl,
    FormLabel,
    FormErrorMessage,
    FormHelperText,
    Select,
    Input,
    ButtonGroup,
} from "@chakra-ui/react";
import "../App.css";
import React, { useEffect, useState } from "react";

// Get the evaluationElementsDictionary from EvaluationController
export default function EvaluationForm() {
  const [evaluationElementsDictionary, setEvaluationElementsDictionary] = useState({});

  useEffect(() => {
    fetchEvaluationObjectives();
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
        <Container maxW={"7xl"} mb='5'>
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
                        <Box className="TitleBox">Date</Box>
                        <Box className="Box">04-10-2023</Box>
                    </HStack>
                    <HStack display='flex' spacing="20px" mb="5">
                        <FormControl style={{ width: '300px' }}>
                            <FormLabel>District/Campus</FormLabel>
                            <Select placeholder="Select District/Campus">
                                <option value="1">T-TESS</option>
                                <option value="2">A-TESS</option>
                                <option value="3">B-TESS</option>
                            </Select>
                        </FormControl>

                        <FormControl style={{ width: '300px' }}>
                            <FormLabel>Cooperating Teacher</FormLabel>
                            <Input type="candidate" />
                        </FormControl>
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
              <table>
                <thead>
                  <tr>
                    <th>Objective Titles</th>
                    <th>Element Titles</th>
                  </tr>
                </thead>
                <tbody>
                  {Object.entries(evaluationElementsDictionary).map(([objectiveTitle, elementTitles]) => (
                    // For each objectiveTitle, map through its corresponding elementTitles
                    elementTitles.map((elementTitle, index) => (
                      <tr key={index}>
                        <td>{objectiveTitle}</td>
                        <td>{elementTitle}</td>
                      </tr>
                    ))
                  ))}
                </tbody>
              </table>
            </Box>
          </Box>
            </Stack>
                <Box mt="5" textAlign="center">
                    <ButtonGroup variant="outline" spacing="6">
                        <Button
                            onClick={() => {
                                window.location.href = "/evaluation";
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
        </Container>
    );
}
