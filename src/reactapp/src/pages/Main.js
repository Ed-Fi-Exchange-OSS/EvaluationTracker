// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

import {
    Table,
    Thead,
    Tbody,
    Tr,
    Th,
    Td,
    TableContainer,
    Button,
    Box,
    Stack,
    Heading,
} from "@chakra-ui/react";
import "../App.css";
import React, { useEffect, useState } from "react";
import { get } from "../components/FetchHelpers";
import { getLoggedInUserId } from "../components/TokenHelpers";


//Created a table to display the data from react objects
export default function EvaluationTable() {
  const [EvaluationRatings, setEvaluationRatings] = useState([]);
  useEffect(() => {
    fetchEvaluationRatings();
  }, []);

  //const response = await get("/connect/token", tokenRequest);
// Retrieve evaluation ratings from API
  const fetchEvaluationRatings = async () => {
    try {
      const userId = getLoggedInUserId();

      const response = await get(`/api/EvaluationRating/${userId}`);

      if (!response.ok) {
        throw new Error("Failed to fetch performance evaluations");
      }

      const data = await response.json();

      setEvaluationRatings(data);
    } catch (error) {
      console.error("Error fetching evaluation ratings:", error);
    }
  };

  return(
      <Stack spacing={8} mt="24px" px={{ base: "10px", md: "30px" }} align="center">
      <Heading fontSize={{ base: "3xl", md: "5xl" }}>Evaluations</Heading>
      <TableContainer maxWidth="100%">
        <Table variant="simple" size="lg" className="responsiveTable">
          <Thead>
            <Tr>
              <Th className="responsiveTable th">Evaluation</Th>
              <Th className="responsiveTable th">Candidate</Th>
              <Th className="responsiveTable th">Evaluator</Th>
              <Th className="responsiveTable th">Date</Th>
              <Th className="responsiveTable th">Status</Th>
            </Tr>
          </Thead>
          <Tbody>
            {EvaluationRatings.map((row, i) => (
              <Tr key={i}>
                <Td>{row.performanceEvaluationTitle}</Td>
                <Td>{row.reviewedCandidateName}</Td> 
                <Td>{row.evaluatorName}</Td> 
                <Td>{new Date(row.actualDate).toLocaleDateString()}</Td> 
                <Td>Completed</Td>
              </Tr>
            ))}
          </Tbody>
        </Table>
      </TableContainer>
      <Box textAlign="center" mt="5" mb="10">
        <Button colorScheme="blue" onClick={() => (window.location.href = "/new")}>
          New Evaluation
        </Button>
      </Box>
    </Stack>
  );
}
