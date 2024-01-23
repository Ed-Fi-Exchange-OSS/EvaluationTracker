// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

import {
  Flex,
  Box,
  FormControl,
  FormLabel,
  InputGroup,
  InputRightElement,
  Checkbox,
  Stack,
  Link,
  Button,
  Heading,
  Text,
  useColorModeValue,
} from "@chakra-ui/react";
import { useState, React } from "react";
import { ViewIcon, ViewOffIcon } from "@chakra-ui/icons";
import { Formik, Form } from "formik";
import InputField from "../components/InputField";
import { postForm } from "../components/FetchHelpers";
import { setToken, isLoggedInUserInRole } from "../components/TokenHelpers";
import { defaultErrorMessage, AlertMessage } from "../components/AlertMessage";
import { useNavigate } from "react-router-dom";
import { ApplicationRoles } from "../constants";

export default function LoginForm() {
  const [showPassword, setShowPassword] = useState(false);
  const [error, setError] = useState(null);
  const navigate = useNavigate();

  const loadEvaluationsPage = () => {
    if (isLoggedInUserInRole([ApplicationRoles.Administrator])) {
      navigate("/users");
    }
    else {
      navigate("/main");
    }
    
  };
  const onSubmitLogin = async (values) => {    
    const tokenRequest = {
      grant_type: "password",
      username: values.email,
      password: values.password,
    };

    try {
      const response = await postForm("/connect/token", tokenRequest);
      const message = await response.json();

      if (!response.ok) {
        console.error(message);
        // TODO use proper user notifications in EPPETA-18
        setError(message.error_description);
        return;
      }
      setToken(message);
      console.info("Successful sign-in");
      loadEvaluationsPage();
      return;
    } catch (exception) {
      setError(defaultErrorMessage);
      console.error(exception);
    }
  };

  return (
    <Flex minH={"100vh"} align={"center"} justify={"center"} bg={useColorModeValue("gray.50", "gray.800")}>
      <Stack spacing={8} mx={"auto"} maxW={"lg"} py={12} px={6}>
        <Stack align={"center"}>
          <Heading fontSize={"4xl"}>Sign in to your account</Heading>
          <Text fontSize={"lg"} color={"gray.600"}>
            for teacher evaluation tracker
          </Text>
        </Stack>
        <Box rounded={"lg"} bg={useColorModeValue("white", "gray.700")} boxShadow={"lg"} p={8}>
          <Formik initialValues={{ email: "", password: "" }} onSubmit={onSubmitLogin}>
            <Form>
              <Stack spacing={4}>
                <FormControl id="email">
                  <FormLabel>Email address</FormLabel>
                  <InputField type="email" name="email" />
                </FormControl>
                <FormControl id="password">
                  <FormLabel>Password</FormLabel>
                  <InputGroup>
                    <InputField type={showPassword ? "text" : "password"} name="password" />
                    <InputRightElement h={"full"}>
                      <Button
                        variant={"ghost"}
                        onClick={() => setShowPassword((showPassword) => !showPassword)}
                      >
                        {showPassword ? <ViewIcon /> : <ViewOffIcon />}
                      </Button>
                    </InputRightElement>
                  </InputGroup>
                </FormControl>
                <Stack spacing={10}>
                  <Stack direction={{ base: "column", sm: "row" }} align={"start"} justify={"space-between"}>
                    <Checkbox>Remember me</Checkbox>
                    <Link color={"blue.400"}>Forgot password?</Link>
                  </Stack>
                  {error && <AlertMessage message={error} />}
                  <Button
                    type="submit"
                    bg={"blue.400"}
                    color={"white"}
                    _hover={{
                      bg: "blue.500",
                    }}
                  >
                    Sign in
                  </Button>
                </Stack>
              </Stack>
            </Form>
          </Formik>
        </Box>
      </Stack>
      </Flex>
  );
}
