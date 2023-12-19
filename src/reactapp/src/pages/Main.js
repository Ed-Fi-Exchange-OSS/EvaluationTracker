// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

import {
    Button,
    Box,
    Stack, HStack,
  Heading,
  Skeleton
} from "@chakra-ui/react";
import { DeleteIcon } from '@chakra-ui/icons'
import { useNavigate } from "react-router-dom";
import Select from 'react-select';
import "../App.css";
import React, { useEffect, useState } from "react";
import { get } from "../components/FetchHelpers";
import { getLoggedInUserId, getLoggedInUserRole } from "../components/TokenHelpers";
import SortableTable from "../components/SortableTable";

//Created a table to display the data from react objects
export default function EvaluationTable() {
  const [EvaluationRatings, setEvaluationRatings] = useState([]);
  const loggedInUserRole = getLoggedInUserRole();
  const [selectedOptions, setSelectedOptions] = useState([]);
  const [statusOptions, setStatusOptions] = useState([]);
  const [componentsDataLoaded, setComponentsDataLoaded] = useState(false);
  const filter = selectedOptions.map((o) => o.value);
  const headers = [
    { name: 'Evaluation', dataField: 'performanceEvaluationTitle', sortable: true, visible: true, link: { url: '/evaluation/', dataField: 'performanceEvaluationRatingId' } },
    { name: 'Candidate', dataField: 'reviewedCandidateName', sortable: true, visible: true },
    { name: 'Evaluator', dataField: 'evaluatorName', sortable: true, visible: loggedInUserRole === 'Supervisor' },
    { name: 'Date', dataField: 'actualDate', sortable: true, visible: true, format: value => new Date(value).toLocaleDateString() },
    { name: 'Status', dataField: 'evaluationStatus', sortable: true, visible: true },
  ];
  const navigate = useNavigate();

  useEffect(() => {
    const userId = getLoggedInUserId();

    if (userId == null) {
      navigate("/login");
    }
    fetchEvaluationStatuses();
    fetchEvaluationRatings(userId);
    setComponentsDataLoaded(true);
  }, []);

  // const response = await get("/connect/token", tokenRequest);
  const fetchEvaluationStatuses = async () => {
    try {
      const response = await get('/api/Evaluation/EvaluationStatuses');

      if (!response.ok) {
        throw new Error("Failed to fetch evaluation statuses");
      }
      const data = await response.json();

      if (data) {
        setStatusOptions(data.map(o => ({ label: o.statusText, value: o.statusText })));
      }
    } catch (error) {
      console.error("Error fetching evaluation ratings:", error);
    }
  };
// Retrieve evaluation ratings from API
  const fetchEvaluationRatings = async (userId) => {
    try {
      let response;

      if (getLoggedInUserRole() === 'Supervisor') {
        response = await get('/api/EvaluationRating/');
      } else {
        response = await get(`/api/EvaluationRating/${userId}`);
      }

      if (!response.ok) {
        throw new Error("Failed to fetch performance evaluations");
      }

      const data = await response.json();

      setEvaluationRatings(data);
    } catch (error) {
      console.error("Error fetching evaluation ratings:", error);
    }
  };

  const clearFilter = () => {
    setSelectedOptions([]);
  };

  const filterExpression = (item) => {
    return filter.length === 0 || filter.includes(item.evaluationStatus);
  };

  return (
    <Skeleton isLoaded={componentsDataLoaded}>
      <Stack spacing={8} mt="24px" px={{ base: "10px", md: "30px" }} align="center">
      <Heading fontSize={{ base: "3xl", md: "5xl" }}>My Evaluations</Heading>
      <HStack spacing={0}>
        <Box flex="1">
      <Select
        value={selectedOptions}
        onChange={(option) => setSelectedOptions(option) }
        options={statusOptions}
        isMulti
        className="basic-multi-select"
        classNamePrefix="select"
        placeholder="Select status"
          /></Box>
        <Button colorScheme="red" onClick={clearFilter} m={0}><DeleteIcon></DeleteIcon></Button>
      </HStack>
      <SortableTable data={EvaluationRatings} headers={headers} paginate={true} itemsPerPage={50} filter={filterExpression} noRowsMessage="No evaluations to display" />
      <Box textAlign="center" mt="5" mb="10">
        <Button colorScheme="blue" onClick={() => (window.location.href = "/new")}>
          New Evaluation
        </Button>
      </Box>
      </Stack>
      </Skeleton>
  );
}
