import React, { useState, useEffect } from 'react';
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
    Input,
    Select,
    ButtonGroup,
    Table,
    TableCaption,
    Thead,
    Tbody,
    Tr,
    Th,
    Td,
} from "@chakra-ui/react";

function EvaluationTable(evaluationDictionary) {
    const [evaluationElementsDictionary, setEvaluationElementsDictionary] = useState({});
    //setEvaluationElementsDictionary(evaluationDictionary)

    useEffect(() => {
        setEvaluationElementsDictionary(evaluationDictionary)
    }, [evaluationDictionary]);

    return (
        <Box>
          {Object.entries(evaluationElementsDictionary).map(([objectiveTitle, elementTitles]) => (
              <Box key={objectiveTitle} mt={4}>

                  <Table variant="striped" size="sm" mt={2}>
                      <TableCaption>SCALE: ** 1=Needs Improvement     2= Developing     *3= Proficient        N/A= Not Applicable (N/A)</TableCaption>
                      <Thead>
                          <Tr>
                              <Th minWidth="200px">Element Titles</Th>
                              <Th minWidth="150px">Rating</Th>
                              <Th>Comments</Th>
                          </Tr>
                      </Thead>
                      {/*<Tbody>*/}
                      {/*    {elementTitles.map((elementTitle, index) => (*/}
                      {/*        <Tr key={index}>*/}
                      {/*            <Td maxWidth="200px">{elementTitle}</Td>*/}
                      {/*            <Td maxWidth="150px">*/}
                      {/*                <Select placeholder="Select rating">*/}
                      {/*                    <option value="1">1 - Needs Improvement</option>*/}
                      {/*                    <option value="2">2 - Developing</option>*/}
                      {/*                    <option value="3">3 - Proficient</option>*/}
                      {/*                    <option value="N/A">N/A - Not Applicable</option>*/}
                      {/*                </Select>*/}
                      {/*            </Td>*/}
                      {/*            <Td>*/}
                      {/*                <Input borderColor="gray.300" type="text" />*/}
                      {/*            </Td>*/}
                      {/*        </Tr>*/}
                      {/*    ))}*/}
                      {/*</Tbody>*/}
                  </Table>
              </Box>
          ))}
      </Box>
  );
}

export default EvaluationTable;
