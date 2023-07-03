import {
    Table,
    Thead,
    Tbody,
    Tfoot,
    Tr,
    Th,
    Td,
    TableCaption,
    TableContainer,
    Button,
    Box,
} from '@chakra-ui/react'

export default function EvaluationTable() {
    return (
        <TableContainer>
            <Table variant="simple" >
                <Thead>
                    <Tr>
                        <Th bg="blue.500" color="white">Evaluation</Th>
                        <Th bg="blue.500" color="white">Candidate</Th>
                        <Th bg="blue.500" color="white">Evaluator</Th>
                        <Th bg="blue.500" color="white" isDate>Date</Th>
                    </Tr>
                </Thead>
                <Tbody>
                    <Tr>
                        <Td>1</Td>
                        <Td>2</Td>
                        <Td>3</Td>
                        <Td>4</Td>
                    </Tr>
                    <Tr>
                        <Td>1</Td>
                        <Td>2</Td>
                        <Td>3</Td>
                        <Td>4</Td>
                    </Tr>
                    <Tr>
                        <Td>1</Td>
                        <Td>2</Td>
                        <Td>3</Td>
                        <Td>4</Td>
                    </Tr>
                </Tbody>
            </Table>
            <Box mt="5" textAlign="center">
                <Button onClick = {() => window.location.href='/new'}
                    bg={'blue.400'}
                    color={'white'}
                    _hover={{
                        bg: 'blue.500',
                    }}>New Evaluation</Button>
            </Box>
        </TableContainer>
    )
}
