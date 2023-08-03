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

//Created a table to display the data from react objects
export default function EvaluationTable() {
    const data = [
        {
            evaluation: "T-TESS",
            candidate: "Steven Mo",
            evaluator: "Jane Doe",
            date: new Date("2022-09-01"),
            completed: true,
        },
        {
            evaluation: "A-TESS",
            candidate: "JOJO",
            evaluator: "Jane Doe",
            date: new Date("2021-09-02"),
            completed: true,
        },
        {
            evaluation: "B-TESS",
            candidate: "Aidan Johnson",
            evaluator: "Kevin Clear",
            date: new Date("2023-09-03"),
            completed: false,
        },
        {
            evaluation: "B-TESS",
            candidate: "Aidan Johnson",
            evaluator: "Kevin Clear",
            date: new Date("2023-09-03"),
            completed: false,
        },
        {
            evaluation: "B-TESS",
            candidate: "Aidan Johnson",
            evaluator: "Kevin Clear",
            date: new Date("2023-09-03"),
            completed: false,
        },
        {
            evaluation: "B-TESS",
            candidate: "Aidan Johnson",
            evaluator: "Kevin Clear",
            date: new Date("2023-09-03"),
            completed: false,
        },
        {
            evaluation: "B-TESS",
            candidate: "Aidan Johnson",
            evaluator: "Kevin Clear",
            date: new Date("2023-09-03"),
            completed: false,
        },
        {
            evaluation: "B-TESS",
            candidate: "Aidan Johnson",
            evaluator: "Kevin Clear",
            date: new Date("2023-09-03"),
            completed: false,
        },
        {
            evaluation: "B-TESS",
            candidate: "Aidan Johnson",
            evaluator: "Kevin Clear",
            date: new Date("2023-09-03"),
            completed: false,
        },
        {
            evaluation: "B-TESS",
            candidate: "Aidan Johnson",
            evaluator: "Kevin Clear",
            date: new Date("2023-09-03"),
            completed: false,
        },
    ];

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
            {data.map((row, i) => (
              <Tr key={i}>
                <Td>{row.evaluation}</Td>
                <Td>{row.candidate}</Td>
                <Td>{row.evaluator}</Td>
                <Td>{row.date.toLocaleDateString()}</Td>
                <Td>
                  {row.completed ? "Completed" : "In Progress"}
                </Td>
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
