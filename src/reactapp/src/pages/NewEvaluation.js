import {
    FormControl,
    FormLabel,
    FormErrorMessage,
    FormHelperText,
    Heading,
    Select,
    Input,
    Box,
    Button,
    ButtonGroup,
    Flex,
    Stack,
    useColorModeValue,
    Text,
    Checkbox,
    Link,
} from '@chakra-ui/react'

export default function NewEvaluation() {
    return (
        <Flex
            minH={'100vh'}
            align={'center'}
            justify={'center'}>
            <Stack spacing={8} mx={'auto'} maxW={'lg'} py={12} px={6}>
                <Stack align={'center'}>
                    <Heading fontSize={'6xl'}>New Evaluation</Heading>
                </Stack>
                <Box
                    rounded={'lg'}
                    bg={useColorModeValue('white', 'gray.700')}
                    boxShadow={'lg'}
                    alignItems={'center'}
                    w="100%"
                    p={8}>
                    <Stack spacing={4}>
                        <FormControl>
                            <FormLabel>Evaluation</FormLabel>
                            <Select placeholder="Select Evaluation">
                                <option value="1">T-TESS</option>
                                <option value="2">A-TESS</option>
                                <option value="3">B-TESS</option>
                            </Select>

                            <FormLabel>Candidate</FormLabel>
                            <Input type='candidate' />

                            <FormLabel>Date</FormLabel>
                            <Input
                                placeholder="Select Date"
                                size="md"
                                type="date"
                            />
                        </FormControl>

                        <Box mt="5" textAlign="center">
                            <ButtonGroup variant='outline' spacing='6'>
                                <Button colorScheme='blue'>Start Evaluation</Button>
                                <Button onClick={() => {window.location.href = "/main" } }>Cancel</Button>
                            </ButtonGroup>
                        </Box>
                    </Stack>
                </Box>
            </Stack>
        </Flex>
    );
}




