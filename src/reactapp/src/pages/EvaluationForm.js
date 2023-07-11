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

export default function EvaluationForm() {
    return (
        <Container maxW={"7xl"}>
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
                        Features
                    </Text>
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

// import React, { useEffect, useState } from 'react';

// export default function EvaluationForm() {
//     const [data, setData] = useState(null);

//     useEffect(() => {
//         fetchData();
//     }, []);

//     async function fetchData() {
//         try {
//             const key = 'populated';
//             const secret = 'populatedSecret';

//             const response = await fetch('https://localhost:443/api/data/v3/tpdm/evaluationObjectives', {
//                 method: 'GET',
//                 headers: {
//                     'accept': 'application/json',
//                     'authorization': `Basic ${btoa(`${key}:${secret}`)}`
//                 }
//             });

//             const jsonData = await response.json();
//             setData(jsonData);
//         } catch (error) {
//             console.error(error);
//         }
//     }

//     if (data === null) {
//         return <div>Loading...</div>;
//     }

//     return (
//         <div>
//             <h1>My Component</h1>
//         </div>
//     );
// }
