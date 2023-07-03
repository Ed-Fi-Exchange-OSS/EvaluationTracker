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
    ButtonGroup
} from '@chakra-ui/react'

export default function NewEvaluation() {
    return (
        <Box maxW="500px" mx="auto" mt="8">
        <FormControl>
            <Heading align="center" mb="4">New Evaluation</Heading>


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
                <Button>Cancel</Button>
                </ButtonGroup>
            </Box>

        </Box>
        )
    }




