// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

import {
    Box,
    Button,
    ButtonGroup,
    Container,
    FormControl,
    FormLabel,
    HStack,
  Heading,
    Skeleton,
    Stack,
    StackDivider,
    Table,
    Tbody,
    Td,
    Text,
    Textarea,
    Th,
    Thead,
    Tr,
    VStack,
    useColorModeValue,
    useToast,
} from "@chakra-ui/react";
import React, { useEffect, useState } from "react";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import { useLocation, useNavigate, useParams } from "react-router-dom";
import Select from 'react-select';
import "../App.css";
import { get, post } from "../components/FetchHelpers";
import { AlertMessage } from "../components/AlertMessage";
import { getLoggedInUserId, getLoggedInUserName, getLoggedInUserRole } from "../components/TokenHelpers";
import { AlertMessageDialog } from "../components/AlertMessageDialog";

export default function EvaluationForm() {
  const [isEvaluationLoaded, setIsEvaluationLoaded] = useState(false);
  const [componentsDataLoaded, setComponentsDataLoaded] = useState(false);
  const [loggedInUser, setLoggedInUser] = useState({ "name": "", "role": null });
  const [evaluationDataLoaded, setEvaluationDataLoaded] = useState({});
  const [selectedCandidate, setSelectedCandidate] = useState({});
  const [selectedEvaluation, setSelectedEvaluation] = useState({});
  const [ratingLevelOptions, setRatingLevelOptions] = useState([]);
  const [evaluationMetadata, setEvaluationMetadata] = useState({});
  const [currentEvaluator, setCurrentEvaluator] = useState({});
  const [evaluationDate, setEvaluationDate] = useState(new Date());
  const [evaluationEndTime, setEvaluationEndTime] = useState(null);
  const [elementRatings, setElementRatings] = useState([]);
  const [objectiveNotes, setObjectiveNotes] = useState([]);
  const [performanceEvaluationData, setPerformanceEvaluationData] = useState(null);
  const [userHasAccessToEvaluation, setUserHasAccessToEvaluation] = useState(true);
  const [evaluationLoaded, setEvaluationLoaded] = useState(true);
  const [alertMessageText, setAlertMessageText] = useState(true);
  const { id } = useParams();
  const location = useLocation();
  const navigate = useNavigate();
  const toast = useToast();  
  const borderColor = useColorModeValue("gray.200", "gray.600"); 
  
  const setRatingLevel = (name, value) => {
    const elementRatingCopy = [...elementRatings];
    const rating = { "name": name, "value": value };

    const locatedIndex = elementRatingCopy.findIndex((element) => element.name === name);
    locatedIndex >= 0 ? elementRatingCopy[locatedIndex] = rating : elementRatingCopy.push(rating);

    setElementRatings(elementRatingCopy);
  };

  const setSelectedOptionRatingLevel = (evaluationMetadataReceived) => {
    const performanceEvaluationData = evaluationDataLoaded;
    const elementRatingCopy = [...elementRatings];
    const objectiveNotesCopy = [...objectiveNotes];
    const evaluationObjectivesCopy = [...evaluationMetadataReceived?.evaluationObjectives];
    performanceEvaluationData?.objectiveResults?.forEach((objectiveResults, i) => {
      const locatedIndex = evaluationObjectivesCopy.findIndex((element) => element.evaluationObjectiveId === objectiveResults.id);
      objectiveNotesCopy.push({ "objectiveId": objectiveResults.id, "name": evaluationObjectivesCopy[locatedIndex], "value": objectiveResults?.comment ?? "" });
      objectiveResults?.elements?.forEach((obj) => {
        elementRatingCopy.push({ "name": obj.id, "value": obj.score });
      });
    });
    setElementRatings(elementRatingCopy);
    setObjectiveNotes(objectiveNotesCopy);
  }

  // Retrieve an existing evaluation from API
  const loadExistingEvaluation = async () => {
    try {
      if (id) {
        const response = await get(`/api/PerformanceEvaluation/${id}`);
        // If the evaluation doesn't exist, show an error message
        if (response.status === 404) {
          setAlertMessageText("The requested evaluation does not exist");
          setEvaluationLoaded(false);
          return;
        }
        else if (!response.ok) {
          throw new Error("Failed to fetch performance evaluation"); 
        }

        // Check if the response is valid JSON
        const contentType = response.headers.get("Content-Type");
        if (!contentType || !contentType.includes("application/json")) {
          throw new Error("Response is not valid JSON");
        }

        const retrievedPerformanceEvaluationData = await response.json();
        setEvaluationDataLoaded(retrievedPerformanceEvaluationData);
        if (getLoggedInUserRole() !== 'Supervisor' && retrievedPerformanceEvaluationData.userId !== getLoggedInUserId()) {
          setAlertMessageText("You do not have access to the evaluation");
          setUserHasAccessToEvaluation(false);
          return;
        }
        setPerformanceEvaluationData(retrievedPerformanceEvaluationData);
        setEvaluationDate(new Date(retrievedPerformanceEvaluationData.startDateTime+"Z"));
        setCurrentEvaluator({ "evaluatorId": retrievedPerformanceEvaluationData.userId, "evaluatorName": retrievedPerformanceEvaluationData.evaluatorName });
        const candidateReceived = {
          candidateName: retrievedPerformanceEvaluationData.reviewedCandidateName,
          personId: retrievedPerformanceEvaluationData.reviewedPersonId,
          sourceSystemDescriptor: retrievedPerformanceEvaluationData.reviewedPersonSourceSystemDescriptor
        }
        const evaluationHeader = {
          id: retrievedPerformanceEvaluationData.evaluationId,
          performanceEvaluationTitle: retrievedPerformanceEvaluationData.performanceEvaluationTitle
        }
        sessionStorage.setItem("evaluation", JSON.stringify(evaluationHeader));
        setSelectedEvaluation(evaluationHeader);
        sessionStorage.setItem("candidate", JSON.stringify(candidateReceived));
        setSelectedCandidate(candidateReceived);
        const startDate = new Date(retrievedPerformanceEvaluationData.startDateTime);
        const endDate = new Date(retrievedPerformanceEvaluationData.startDateTime);
        if (endDate > startDate) {
          setEvaluationEndTime(new Date(endDate - startDate));
        }        
      }
    } catch (error) {
      console.error("Error fetching performance evaluation:", error);
    }
  };

  const loadDataForNewEvaluation = () => {
    setCurrentEvaluator({ "evaluatorId": getLoggedInUserRole(), "evaluatorName": getLoggedInUserName() });
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
  };   

  useEffect(() => {
    setIsEvaluationLoaded(false);
    setLoggedInUser({
      "name": getLoggedInUserName(),
      "role": getLoggedInUserRole()
    });
    
    if (id) {
      loadExistingEvaluation();
    }
    else {
      setCurrentEvaluator({ "evaluatorId": getLoggedInUserId(), "evaluatorName": getLoggedInUserName() });
      loadDataForNewEvaluation();
    }
    setIsEvaluationLoaded(true);
  }, [id]);

  
  const processRatingLevelOptions = async (ratingLevels) => {
    const processedRatingLevels = [{ "label": "N/A - Not Applicable", "value": -1 }, ...ratingLevels.map((level) => {
      return {
        "label": level.name,
        "value": level.ratingLevel
      }
    })];
    setRatingLevelOptions(processedRatingLevels); // Save to state
  }

  useEffect(() => {
    setComponentsDataLoaded(false);
    fetchEvaluationObjectives();
    setComponentsDataLoaded(selectedEvaluation.id ? true : false);
  }, [selectedEvaluation]);

  // Retrieve evaluation objectives from API
  const fetchEvaluationObjectives = async () => {
    try {
      if (selectedEvaluation.id) {
        const response = await get(`/api/Evaluation/${selectedEvaluation.id}`);
        console.info(JSON.stringify(response));
        if (!response.ok) {
          throw new Error("Failed to fetch evaluation objectives");
        }

        // Check if the response is valid JSON
        const contentType = response.headers.get("Content-Type");
        if (!contentType || !contentType.includes("application/json")) {
          throw new Error("Response is not valid JSON");
        }

        const evaluationData = await response.json();
        processRatingLevelOptions(evaluationData.ratingLevels);
        setEvaluationMetadata(evaluationData);
        setSelectedOptionRatingLevel(evaluationData);
      }
    } catch (error) {
      console.error("Error fetching evaluation objectives:", error);
    }
  };

  const getCompletedEvaluationData = () => {
    const completedEvaluation = {
    };
    completedEvaluation.evaluationId = selectedEvaluation.id;
    if (id) {
      completedEvaluation.performanceEvaluationId = id;
    }
    completedEvaluation.reviewedPersonId = selectedCandidate.personId;
    completedEvaluation.reviewedPersonSourceSystemDescriptor = selectedCandidate.sourceSystemDescriptor;
    completedEvaluation.evaluatorName = currentEvaluator.evaluatorName;
    completedEvaluation.reviewedCandidateName = selectedCandidate.candidateName;
    completedEvaluation.startDateTime = evaluationDate;
    !evaluationEndTime ?
      completedEvaluation.endDateTime = new Date() : completedEvaluation.endDateTime = evaluationEndTime;

    completedEvaluation.objectiveResults = evaluationMetadata.evaluationObjectives.flatMap((objective) => {

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
    return completedEvaluation;
  }

  const saveEvaluation = async () => {
    try {
      const completedEvaluation = getCompletedEvaluationData();
      const response = await post(`/api/EvaluationRating?userId=${getLoggedInUserId() }`, completedEvaluation);
      if (response.ok) {
        toast({
          title: "Success.",
          description: "Your evaluation has been successfully saved.",
          status: "success",
          duration: 5000,
          isClosable: true,
        });
        navigate("/main");
      }
      else {
        toast({
          title: "An error occurred.",
          description: "Unable to save the evaluation.",
          status: "error",
          duration: 5000,
          isClosable: true,
        });
      }
    } catch (e) {
      toast({
        title: "An error occurred.",
        description: "Unable to save the evaluation.",
        status: "error",
        duration: 5000,
        isClosable: true,
      });
      console.error("Error:", e);
    }
    return true;
  }

  const approveEvaluation = async () => {
    try {
      const completedEvaluation = getCompletedEvaluationData();
      const response = await post(`/api/EvaluationRating/Approve?userId=${getLoggedInUserId()}`, completedEvaluation);
      if (response.ok) {
        toast({
          title: "Success.",
          description: "Your approval has been successfully sent.",
          status: "success",
          duration: 5000,
          isClosable: true,
        });
        navigate("/main");
      }
      else {
        toast({
          title: "An error occurred.",
          description: "Unable to send the approval.",
          status: "error",
          duration: 5000,
          isClosable: true,
        });
      }
    } catch (e) {
      toast({
        title: "An error occurred.",
        description: "Unable to send the approval.",
        status: "error",
        duration: 5000,
        isClosable: true,
      });
      console.error("Error:", e);
    }
    return true;
  }

  // Rating drop down event handler
  const handleChangeRatingLevel = (e, actionProps) => {
    setRatingLevel(actionProps.name, e.value);
  };
  
  const getSelectedOptionRatingLevel = (name) => {
    const elementRatingCopy = [...elementRatings,
     {
       "codeValue": "N/A - Not Applicable",
        "ratingLevel": -1
      }];
    const ratingLevels = [...evaluationMetadata.ratingLevels,
    {
      "name": "N/A - Not Applicable",
      "ratingLevel": -1
    }];
    const locatedIndex = elementRatingCopy.findIndex((element) => element.name === name);
    if (locatedIndex >= 0) {
      const locatedIndexRatingOptions = ratingLevels.findIndex((element) => element.ratingLevel === elementRatingCopy[locatedIndex].value);
      const selectedValue = [{
        "label": ratingLevels[locatedIndexRatingOptions]?.name ?? "N/A - Not Applicable",
        "value": ratingLevels[locatedIndexRatingOptions]?.ratingLevel ?? -1,
      }];
      return selectedValue;
    }
    return [{
      "label": "N/A - Not Applicable",
      "value": "-1"
    }];
  }

  const handleNotesUpdates = (e) => {
    const objectiveNotesCopy = [...objectiveNotes];
    const note = { "objectiveId": e.target.id, "name": e.target.name, "value": e.target.value };

    const locatedIndex = objectiveNotesCopy.findIndex((objective) => objective.name.name === e.target.name && objective.objectiveId == e.target.id);
    locatedIndex >= 0 ? objectiveNotesCopy[locatedIndex] = note : objectiveNotesCopy.push(note);

    setObjectiveNotes(objectiveNotesCopy);
  };   

  return (<Skeleton isLoaded={isEvaluationLoaded && componentsDataLoaded} count={3.5} >
    <Container maxW={"7xl"} mb='10'>
        {(!userHasAccessToEvaluation || !evaluationLoaded) ? (<>
        <Stack textAlign={"center"}
          spacing={{ base: 4, sm: 6 }}
          direction={"column"}
          divider={
            <StackDivider
              borderColor={borderColor}
            />
          }
        > 
          <Heading
            lineHeight={1.1}
            mt={5}
            mb={5}
            fontSize={"3xl"}
            fontWeight={"700"}
          >
            Evaluation</Heading>
        </Stack>
        <Box textAlign="center" FontWeight="bold" >
          <AlertMessage status="warning" message={alertMessageText} />
        </Box>
        <Box textAlign="center">
          
            <ButtonGroup variant="outline" spacing="6">
              <Button
                onClick={() => {
                  navigate("/main");
                }}
              >
                Return
              </Button>
            </ButtonGroup>
          </Box>
        </>)
        : (<><Stack
          spacing={{ base: 4, sm: 6 }}
          direction={"column"}
          divider={
            <StackDivider
              borderColor={borderColor}
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
            <Box className="Box">{currentEvaluator.evaluatorName}</Box>
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
            {evaluationMetadata?.evaluationObjectives?.map((objective) => (
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
                            onChange={(e, action) => {
                              handleChangeRatingLevel(e, action)
                            }}
                            value={getSelectedOptionRatingLevel(element.evaluationElementId) }
                          />
                        </Td>
                        {index === 0 && <Td rowSpan="4"><Textarea id={objective.evaluationObjectiveId} name={objective.name} onBlur={handleNotesUpdates} rows={(objective.evaluationElements.length * 3) - 1} resize="none" borderColor="gray.300" defaultValue={(performanceEvaluationData ? performanceEvaluationData.objectiveResults.find(item => item.id === objective?.evaluationObjectiveId)?.comment ?? "" : "" ) } /></Td>}
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
              <AlertMessageDialog showIcon="warning" alertTitle="Save Evaluation" buttonColorScheme="blue" buttonText="Save" message="Are you sure you want to save the evaluation?" onYes={() => { saveEvaluation() }}></AlertMessageDialog>
              <AlertMessageDialog showIcon="warning" alertTitle="Cancel process" buttonText="Cancel" message="Are you sure you want to cancel this process? All unsaved changes will be lost" onYes={() => { navigate("/main"); }}></AlertMessageDialog>
              {loggedInUser.role === 'Supervisor' &&
                <AlertMessageDialog showIcon="warning" alertTitle="Approve Evaluation" buttonText="Approve" message="Are you sure you want to approve this evaluation?" onYes={ () => approveEvaluation() }></AlertMessageDialog>
                }
        </ButtonGroup>
        </Box> </>)}
    </Container>
  </Skeleton>
  );
}
