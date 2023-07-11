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
    Flex,
    Stack,
    Heading,
    useColorModeValue,
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

    return (
        <Stack spacing={8}>
            <Stack align={"center"}>
                <Heading mt="24px" fontSize={"5xl"}>
                    Evaluations
                </Heading>
            </Stack>
            <TableContainer style={{ maxWidth: "100%" }} className="responsiveTableContainer">
      <Table variant="simple" size="lg" className="responsiveTable">
        <Thead>
          <Tr>
            <Th bg="blue.500" color="white">
              Evaluation
            </Th>
            <Th bg="blue.500" color="white">
              Candidate
            </Th>
            <Th bg="blue.500" color="white">
              Evaluator
            </Th>
            <Th bg="blue.500" color="white">
              Date
            </Th>
            <Th bg="blue.500" color="white">
              Status
            </Th>
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
            <Box mt="5" textAlign="center" mb="10">
                <Button
                    onClick={() => (window.location.href = "/new")}
                    bg={"blue.400"}
                    color={"white"}
                    _hover={{
                        bg: "blue.500",
                    }}
                >
                    New Evaluation
                </Button>
            </Box>
        </Stack>
    );
}
