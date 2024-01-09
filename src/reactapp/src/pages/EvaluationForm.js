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
import { AlertMessageDialog } from "../components/AlertMessageDialog"
import { isPageReload, savePageData, getStoredPageData } from "../components/PageTracker";

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
  const [evaluationEndTime, setEvaluationEndTime] = useState(new Date("1970-01-01"));
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

  /**
   * Update rating levels and the page data to store the selected options.
   * @param {any} name: rating id
   * @param {any} value: score id
   */
  const setRatingLevel = (name, value) => {
    const elementRatingCopy = [...elementRatings];
    const rating = { "name": name, "value": value };
    const locatedIndex = elementRatingCopy.findIndex((element) => element.name === name);
    locatedIndex >= 0 ? elementRatingCopy[locatedIndex] = rating : elementRatingCopy.push(rating);
    updateScore(Number(name), Number(value));
    setElementRatings(elementRatingCopy);
  };


  /**
   * Restores notes and notes from an existing (or saved) evaluation.
   * @param {any} evaluationMetadataReceived
   */
  const setSelectedOptionRatingLevel = (evaluationMetadataReceived) => {
    const performanceEvaluationData = evaluationDataLoaded;
    const elementRatingCopy = [...elementRatings];
    const objectiveNotesCopy = [...objectiveNotes];
    const evaluationObjectivesCopy = [...evaluationMetadataReceived?.evaluationObjectives];
    performanceEvaluationData?.objectiveResults?.forEach((objectiveResults, i) => {
      // Index of the evaluationObjective
      const locatedIndex = evaluationObjectivesCopy.findIndex((element) => element.evaluationObjectiveId === objectiveResults.id);
      // Add a note if doesn't exist or update an existing note.
      const noteLocaltedIndex = objectiveNotesCopy.findIndex((element) => element?.objectiveId === objectiveResults.id);
      if (noteLocaltedIndex > 0) {
        objectiveNotesCopy[noteLocaltedIndex] = objectiveResults?.comment ?? "";
      }
      else {
        objectiveNotesCopy.push({ "objectiveId": objectiveResults.id, "name": evaluationObjectivesCopy[locatedIndex], "value": objectiveResults?.comment ?? "" });
      }
      // Updates scores.
      objectiveResults?.elements?.forEach((obj) => {
        const elementRatingLocaltedIndex = elementRatingCopy.findIndex((element) => element?.name === obj.id);
        if (elementRatingLocaltedIndex > 0) {
          elementRatingCopy[elementRatingLocaltedIndex].value = obj.score;
        }
        else {
          elementRatingCopy.push({ "name": obj.id, "value": obj.score });
        }
      });
    });
    setElementRatings(elementRatingCopy);
    setObjectiveNotes(objectiveNotesCopy);
  }


  /**
   * Updates an score by elementId
   * @param {any} elementId
   * @param {any} newScore
   */
  const updateScore = (elementId, newScore) => {
    const evaluationDataLoadedCopy = { ...evaluationDataLoaded };
    let objectiveId = null;

    if (!evaluationDataLoadedCopy.objectiveResults) {
      evaluationDataLoadedCopy.objectiveResults = [];
    }
    // if item was not found we need to search by metadata
    for (let objective of evaluationMetadata?.evaluationObjectives) {
      // Check if any element in the current result has the given element ID
      if (objective.evaluationElements.some(e => e.evaluationElementId === elementId)) {
        // If found, return the ID of the current result
        objectiveId = objective.evaluationObjectiveId;
      }
    }
    let result = evaluationDataLoadedCopy.objectiveResults.find(r => r.id === objectiveId);
    if (!result) {
      const evaluationObjectiveIndex = evaluationMetadata?.evaluationObjectives.findIndex(element => element.evaluationObjectiveId === objectiveId);
      result = { id: Number(objectiveId), name: evaluationMetadata?.evaluationObjectives[evaluationObjectiveIndex], comment: '', elements: [] };
      evaluationDataLoadedCopy.objectiveResults.push(result);
    }
    // Find or create the element object and update the score if provided
    if (elementId !== undefined && newScore !== undefined) {
      let element = result.elements.find(e => e.id === elementId);
      if (!element) {
        element = { id: elementId, score: 0 };
        result.elements.push(element);
      }
      element.score = newScore;
    }
    savePageData(evaluationDataLoadedCopy);
    setEvaluationDataLoaded(evaluationDataLoadedCopy);
  };


  /**
   * Updates a comment
   * @param {any} resultId
   * @param {any} newComment
   */
  const updateEvaluationComment = (resultId, newComment) => {
    const evaluationDataLoadedCopy = { ...evaluationDataLoaded };
    // Find or create the result object
    let result = evaluationDataLoadedCopy?.objectiveResults?.find(r => r.id === resultId);
    if (!result) {
      if (!evaluationDataLoadedCopy.objectiveResults) {
        evaluationDataLoadedCopy.objectiveResults = [];
      }
      const evaluationObjectiveIndex = evaluationMetadata?.evaluationObjectives.findIndex(element => element.evaluationObjectiveId === resultId);
      result = { id: Number(resultId), name: evaluationMetadata?.evaluationObjectives[evaluationObjectiveIndex], comment: newComment ?? '', elements: [] };
      evaluationDataLoadedCopy.objectiveResults.push(result);
    }
    // Update the comment if provided
    if (newComment !== undefined) {
      result.comment = newComment;
    }
    savePageData(evaluationDataLoadedCopy);
    setEvaluationDataLoaded(evaluationDataLoadedCopy);
  }


  /**
   * Retrieves an existing evaluation from API
   * @returns
   */
  const loadExistingEvaluation = async () => {
    try {
      let page_session_data = {};
      const dataFromSession = getStoredPageData();
      // if the page has data it tries to load that. 
      if (isPageReload() && dataFromSession) {
        page_session_data = dataFromSession;
      }
      else if (id) {
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
        page_session_data = await response.json();
        const startDate = new Date(page_session_data.startDateTime);
        const endDate = new Date(page_session_data.endDateTime);
        const evaluationEndDate = (new Date(endDate - startDate));
        if (endDate > startDate) {
          page_session_data.evaluationEndDate = evaluationEndDate;
          setEvaluationEndTime(page_session_data.evaluationEndDate);
        }
        else {
          setEvaluationEndTime(null);
        }
        savePageData(page_session_data);
      }
      else {
        return;
      }
      if (id && getLoggedInUserRole() !== 'Supervisor' && page_session_data.userId !== getLoggedInUserId()) {
        setAlertMessageText("You do not have access to the evaluation");
        setUserHasAccessToEvaluation(false);
        return;
      }
      const currentStartDateTime = page_session_data?.startDateTime
        ? new Date((page_session_data.startDateTime.endsWith("Z") ? page_session_data.startDateTime : page_session_data.startDateTime + "Z"))
        : new Date();
      const currentEndDateTime = page_session_data?.startDateTime
        ? new Date((page_session_data.endDateTime.endsWith("Z") ? page_session_data.endDateTime : page_session_data.endDateTime + "Z"))
        : new Date();
      setEvaluationDate(currentStartDateTime);
      setEvaluationEndTime(currentEndDateTime);

      setCurrentEvaluator({ "evaluatorId": page_session_data.userId, "evaluatorName": page_session_data.evaluatorName });
      const candidateReceived = {
        candidateName: page_session_data.reviewedCandidateName,
        personId: page_session_data.reviewedPersonId,
        sourceSystemDescriptor: page_session_data.reviewedPersonSourceSystemDescriptor
      }
      const evaluationHeader = {
        id: page_session_data.evaluationId,
        performanceEvaluationTitle: page_session_data.performanceEvaluationTitle
      }
      sessionStorage.setItem("evaluation", JSON.stringify(evaluationHeader));
      setSelectedEvaluation(evaluationHeader);
      sessionStorage.setItem("candidate", JSON.stringify(candidateReceived));
      setSelectedCandidate(candidateReceived);
      setEvaluationDataLoaded(page_session_data);
      setPerformanceEvaluationData(page_session_data);
    } catch (error) {
      console.error("Error fetching performance evaluation:", error);
    }
  };


  /**
   * Prepares data to create a new evaluation.
   */
  const loadDataForNewEvaluation = () => {
    const pageInitialData = {};
    setCurrentEvaluator({ "evaluatorId": getLoggedInUserRole(), "evaluatorName": getLoggedInUserName() });
    pageInitialData.evaluatorName = getLoggedInUserName();
    pageInitialData.userId = getLoggedInUserRole();
    let candidateReceived = {};
    if (location?.state?.candidate) {
      sessionStorage.setItem("candidate", JSON.stringify(location.state.candidate));
      setSelectedCandidate(location.state.candidate);
      candidateReceived = location.state.candidate;
    } else {
      setSelectedCandidate(JSON.parse(sessionStorage.getItem("candidate")));
      candidateReceived = JSON.parse(sessionStorage.getItem("candidate"));
    }
    pageInitialData.reviewedCandidateName = candidateReceived.candidateName;
    pageInitialData.reviewedPersonId = candidateReceived.personId;
    pageInitialData.reviewedPersonSourceSystemDescriptor = candidateReceived.sourceSystemDescriptor;
    
    if (location?.state?.evaluation) {
      sessionStorage.setItem("evaluation", JSON.stringify(location.state.evaluation));
      setSelectedEvaluation(location.state.evaluation);
      pageInitialData.evaluationId = location.state.evaluation.id;
      pageInitialData.performanceEvaluationTitle = location.state.evaluation.performanceEvaluationTitle;

    } else {
      const evaluation = JSON.parse(sessionStorage.getItem("evaluation"));
      setSelectedEvaluation(evaluation);
      pageInitialData.evaluationId = evaluation.id;
      pageInitialData.performanceEvaluationTitle = evaluation.performanceEvaluationTitle;
    }
    pageInitialData.startDateTime = evaluationDate;
    pageInitialData.endDateTime = evaluationEndTime;
    pageInitialData.objectiveResults = [];
    if (!isPageReload()) {
      savePageData(pageInitialData);
      setEvaluationDataLoaded(pageInitialData);
    }
  };


  /**
  * Adds rating level options
  * @param {any} ratingLevels
  */
  const processRatingLevelOptions = async (ratingLevels) => {
    const processedRatingLevels = [{ "label": "N/A - Not Assessed", "value": -1 }, ...ratingLevels.map((level) => {
      return {
        "label": level.name,
        "value": level.ratingLevel
      }
    })];
    setRatingLevelOptions(processedRatingLevels); // Save to state
  }


  /**
   * Gets selected option rating to be used by the Select component
   * @param {any} name
   * @returns
   */
  const getSelectedOptionRatingLevel = (name) => {
    const elementRatingCopy = [...elementRatings,
    {
      "codeValue": "N/A - Not Assessed",
      "ratingLevel": -1
    }];
    const ratingLevels = [...evaluationMetadata.ratingLevels,
    {
      "name": "N/A - Not Assessed",
      "ratingLevel": -1
    }];
    const locatedIndex = elementRatingCopy.findIndex((element) => element.name === name);
    if (locatedIndex >= 0) {
      const locatedIndexRatingOptions = ratingLevels.findIndex((element) => element.ratingLevel === elementRatingCopy[locatedIndex].value);
      const selectedValue = [{
        "label": ratingLevels[locatedIndexRatingOptions]?.name ?? "N/A - Not Assessed",
        "value": ratingLevels[locatedIndexRatingOptions]?.ratingLevel ?? -1,
      }];
      return selectedValue;
    }
    return [{
      "label": "N/A - Not Assessed",
      "value": -1
    }];
  }


  /**
   *
   * Returns true if all the required scores are selected.
   * @returns
   */
  const areAllScoreSelected = () => {
    let hasPendingScores = false;
    const elementRatingCopy = [...elementRatings,
    {
      "codeValue": "N/A - Not Assessed",
      "ratingLevel": -1
      }];
    const evaluationMetadataCopy = { ...evaluationMetadata };
    if (evaluationMetadataCopy) {
      hasPendingScores = evaluationMetadataCopy?.evaluationObjectives?.some((objective) => { 
        return objective.evaluationElements.some((element) => {
        const locatedIndex = elementRatingCopy.findIndex((item) => item.name === element.evaluationElementId);
          if (locatedIndex >= 0) {
            return !(elementRatingCopy[locatedIndex].value >= 0)
          }
          return true;
        })
      });
    }
    return !hasPendingScores;
  }


  /**
  * Retrieves evaluation objectives from API
  */
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


  /**
   * Processes selected options and comments to save
   * @returns an object with the data to store
   */
  const getCompletedEvaluationData = () => {
    const completedEvaluation = {
    };
    completedEvaluation.evaluationId = selectedEvaluation.id;
    if (id) {
      completedEvaluation.EvaluationRatingId = id;
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


  /**
 * Event to handle Select component changes
 * @param {any} e
 */
  const handleChangeRatingLevel = (e, actionProps) => {
    setRatingLevel(actionProps.name, e.value);
  };


  /**
   * Event to update notes object
   * @param {any} e
   */
  const handleNotesUpdates = (e) => {
    const noteObjectiveId = Number(e.target.id);
    const objectiveNotesCopy = [...objectiveNotes];
    const note = { "objectiveId": noteObjectiveId, "name": { "name": e.target.name }, "value": e.target.value };

    const locatedIndex = objectiveNotesCopy.findIndex((objective) => objective.name.name === e.target.name && objective.objectiveId === noteObjectiveId);
    locatedIndex >= 0 ? objectiveNotesCopy[locatedIndex].value = e.target.value : objectiveNotesCopy.push(note);

    setObjectiveNotes(objectiveNotesCopy);
    updateEvaluationComment(noteObjectiveId, e.target.value, undefined, undefined);
  };

  /**
   * Event that updates start date
   */
  const handleStartDateChanged = (date) => {
    const pageDataCopy = { ...getStoredPageData() };
    setEvaluationDate(date);
    pageDataCopy.startDateTime = date;
    setEvaluationEndTime(null);
    pageDataCopy.endDateTime = null;
    savePageData(pageDataCopy);
  };

  /**
   * Event that updates end date
   */
  const handleEndDateChanged = (date) => {
    const pageDataCopy = { ...getStoredPageData() };
    setEvaluationEndTime(date);
    pageDataCopy.endDateTime = date;
    savePageData(pageDataCopy);
  };

  /**
 * Saves the evaluation and send data to backend
 * @returns
 */
  const saveEvaluation = async () => {
    try {
      const completedEvaluation = getCompletedEvaluationData();
      const response = await post(`/api/EvaluationRating?userId=${getLoggedInUserId()}`, completedEvaluation);
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


  /**
   * Approves the evaluation and send data to backend
   * @returns
   */
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


  useEffect(() => {
    if (isPageReload()) {
      setIsEvaluationLoaded(false);
      loadExistingEvaluation();
      setIsEvaluationLoaded(true);
    }
  }, []);


  useEffect(() => {
    setIsEvaluationLoaded(false);
    try {
      setLoggedInUser({
        "name": getLoggedInUserName(),
        "role": getLoggedInUserRole()
      });

      if (id || isPageReload()) {
        loadExistingEvaluation();
      }
      else {
        setCurrentEvaluator({ "evaluatorId": getLoggedInUserId(), "evaluatorName": getLoggedInUserName() });
        loadDataForNewEvaluation();
      }
    }
    catch (error) {
      console.error("Error:", error);
    }
    setIsEvaluationLoaded(true);
  }, [id]);


  useEffect(() => {
    if (selectedEvaluation.id) {
      setComponentsDataLoaded(false);
      fetchEvaluationObjectives();
      setComponentsDataLoaded(selectedEvaluation.id ? true : false);
    }
  }, [selectedEvaluation]);


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
              checking the appropriate box. Use Not Assessed (NA)
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
              * Proficient is the goal N/A= Not Assessed (N/A)
            </Text>

            <HStack display='flex' spacing="20px" mb="5" mt="5">
              <FormControl style={{ width: '300px' }}>
                <FormLabel>Pre-Conference Start Time</FormLabel>
                <DatePicker selected={evaluationDate} dateFormat="MM/dd/yy hh:mm" timeFormat="hh:mm" showTimeSelect={true} onChange={(date) => handleStartDateChanged(date)} />
              </FormControl>
              <FormControl style={{ width: '300px' }}>
                <FormLabel>End Time</FormLabel>
                <DatePicker selected={evaluationDate < evaluationEndTime ? evaluationEndTime : evaluationDate} dateFormat="hh:mm" timeFormat="hh:mm" showTimeSelect={true} showTimeSelectOnly={true} onChange={(date) => handleEndDateChanged(date)} />
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
                            <Select name={element.evaluationElementId} styles={{
                              control: (baseStyles, state) => ({
                                  ...baseStyles,
                                  borderColor: getSelectedOptionRatingLevel(element.evaluationElementId)[0].value === -1 ? 'red' : 'inherit'
                              }),
                            }} id={element.evaluationElementId} options={ratingLevelOptions}
                              onChange={(e, action) => {
                                handleChangeRatingLevel(e, action)
                              }}
                              value={getSelectedOptionRatingLevel(element.evaluationElementId)}
                            />
                          </Td>
                          {index === 0 && <Td rowSpan="4"><Textarea id={objective.evaluationObjectiveId} name={objective.name} onBlur={handleNotesUpdates} rows={(objective.evaluationElements.length * 3) - 1} resize="none" borderColor="gray.300" defaultValue={(performanceEvaluationData ? performanceEvaluationData?.objectiveResults?.find(item => item.id === objective?.evaluationObjectiveId)?.comment ?? "" : "")} /></Td>}
                        </Tr>
                      ))}
                    </Tbody>
                  </Table>
                </Box>
              ))}
            </Box>
          </Box>
          <Box mt="0" textAlign="center">
            {!areAllScoreSelected()
              && (<AlertMessage message="Fields highlighted in red are required" />)
            }
          </Box>
        </Stack>
          <Box textAlign="center">
            <ButtonGroup variant="outline" spacing="6">
              { (areAllScoreSelected()) &&
                <AlertMessageDialog showIcon="warning" alertTitle="Save Evaluation" buttonColorScheme="blue" buttonText="Save" message="Are you sure you want to save the evaluation?" onYes={() => { saveEvaluation() }}></AlertMessageDialog>
              }
              <AlertMessageDialog showIcon="warning" alertTitle="Cancel process" buttonText="Cancel" message="Are you sure you want to cancel this process? All unsaved changes will be lost" onYes={() => { navigate("/main"); }}></AlertMessageDialog>
              {(areAllScoreSelected() && loggedInUser.role === 'Supervisor') &&
                <AlertMessageDialog showIcon="warning" alertTitle="Approve Evaluation" buttonText="Approve" message="Are you sure you want to approve this evaluation?" onYes={() => approveEvaluation()}></AlertMessageDialog>
              }
            </ButtonGroup>
          </Box> </>)}
    </Container>
  </Skeleton>
  );
}
