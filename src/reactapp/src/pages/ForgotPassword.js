// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

import {
  Flex,
  Box,
  FormControl,
  FormLabel,
  Stack,
  Button,
  Heading,
  Text,
  useColorModeValue,
  useToast,
  Spinner
} from "@chakra-ui/react";
import { useState, React } from "react";
import { Formik, Form } from "formik";
import InputField from "../components/InputField";
import { post } from "../components/FetchHelpers";
import { defaultErrorMessage, AlertMessage } from "../components/AlertMessage";
import { useNavigate } from "react-router-dom";


export default function ForgotPassword() {
  const toast = useToast();
  const [error, setError] = useState(null);
  const [isLoading, setisLoading] = useState(false); 
  const navigate = useNavigate();


  const onSubmitLogin = async (values) => {    
    try {
      setisLoading(true);
      const response = await post("/accounts/ForgotPassword?email=" + values.email);
      
      if (response.ok) {
        toast({
          title: "Success.",
          description: "Your reset password request has been successfully sent.",
          status: "success",
          duration: 5000,
          isClosable: true,
        });
        navigate("/");
      }
      else {
        toast({
          title: "An error occurred.",
          description: "Unable to send the reset password request.",
          status: "error",
          duration: 5000,
          isClosable: true,
        });
      }
      setisLoading(false);
      return;
    } catch (exception) {
      setError(defaultErrorMessage);
      console.error(exception);
    }
    setisLoading(false);
  };

  return (
    <Flex minH={"100vh"} align={"center"} justify={"center"} bg={useColorModeValue("gray.50", "gray.800")}>
      <Stack spacing={8} mx={"auto"} maxW={"lg"} py={12} px={6}>
        <Stack align={"center"}>
          <Heading fontSize={"4xl"}>Forgot Password</Heading>
          <Text fontSize={"lg"} color={"gray.600"}>
            for teacher evaluation tracker
          </Text>
        </Stack>
        <Box rounded={"lg"} bg={useColorModeValue("white", "gray.700")} boxShadow={"lg"} p={8}>
          {isLoading ? (<><Box display="flex" alignItems="center"><Spinner role="status" />
            <Text ml={4}>Processing...</Text></Box></>)
            : 
          <Formik initialValues={{ email: "", password: "" }} onSubmit={onSubmitLogin}>
            <Form>
              <Stack spacing={4}>
                <FormControl id="email">
                  <FormLabel>Email address</FormLabel>
                  <InputField type="email" name="email" />
                </FormControl>
                <Stack spacing={10}>
                  {error && <AlertMessage message={error} />}
                  <Button
                    type="submit"
                    bg={"blue.400"}
                    color={"white"}
                    _hover={{
                      bg: "blue.500",
                    }}
                  >
                    Reset Password
                  </Button>
                </Stack>
              </Stack>
            </Form>
            </Formik>
          }
        </Box>
      </Stack>
      </Flex>
  );
}
