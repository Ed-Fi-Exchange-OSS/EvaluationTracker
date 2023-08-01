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
        <TableContainer style={{ maxWidth: "100%", marginLeft: "30px", marginRight: "30px" }} className="responsiveTableContainer">
      <Table variant="simple" size="lg" className="responsiveTable">
        <Thead>
          <Tr>
            <Th className="responsiveTable th">
              Evaluation
            </Th>
            <Th className="responsiveTable th">
              Candidate
            </Th>
            <Th className="responsiveTable th">
              Evaluator
            </Th>
            <Th className="responsiveTable th">
              Date
            </Th>
            <Th className="responsiveTable th">
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
          <Button colorScheme='blue' onClick={() => (window.location.href = "/new")}>

                    New Evaluation
                </Button>
            </Box>
        </Stack>
    );
}
