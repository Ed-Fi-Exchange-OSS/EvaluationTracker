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
  HStack,
  InputRightElement,
  Stack,
  Button,
  Heading,
  Text,
  useColorModeValue,
} from "@chakra-ui/react";
import { useState } from "react";
import { ViewIcon, ViewOffIcon } from "@chakra-ui/icons";
import { Formik, Form } from "formik";
import { isObject, mapValues } from "lodash-es";

import InputField from "../components/InputField";
import { post } from "../components/FetchHelpers";

export default function SignupForm() {
  const [showPassword, setShowPassword] = useState(false);

  const loadSignInPage = () => {
    // TODO: what if the site is running behind a proxy? Then / might be the wrong base
    // Address this in EPPETA-19
    window.location.href = "/login";
  };

  const onSubmitSignup = async (values) => {
    try {
      const response = await post("/accounts", values);
      const message = await response.json();

      if (response.ok) {
        console.info("User has been created");
        loadSignInPage();
        return;
      }

      if (message.error) {
        let alertMessage = message.error;
        if (isObject(message.error)) {
          // Converting .NET ModelState to something more readable
          alertMessage = JSON.stringify(mapValues(message.error, (v) => v.errors));
        }

        // TODO: improve display of errors in EPPETA-16
        alert(alertMessage);
        console.error(alertMessage);
        return false;
      } else if (response.status === 409) {
        alert("This email address has already been registered.");
        return false;
      }
    } catch (error) {
      console.error(error);
      return false;
    }
  };

  return (
    <Flex minH={"100vh"} align={"center"} justify={"center"} bg={useColorModeValue("gray.50", "gray.800")}>
      <Stack spacing={8} mx={"auto"} maxW={"lg"} py={12} px={6}>
        <Stack align={"center"}>
          <Heading fontSize={"4xl"} textAlign={"center"}>
            Sign up
          </Heading>
          <Text fontSize={"lg"} color={"gray.600"}>
            for teacher evaluation tracker
          </Text>
        </Stack>
        <Box rounded={"lg"} bg={useColorModeValue("white", "gray.700")} boxShadow={"lg"} p={8}>
          <Formik
            initialValues={{ firstName: "", lastName: "", email: "", password: "" }}
            onSubmit={onSubmitSignup}
          >
            <Form>
              <Stack spacing={4}>
                <HStack>
                  <Box>
                    <FormControl id="firstName" isRequired>
                      <FormLabel>First Name</FormLabel>
                      <InputField type="text" name="firstName" />
                    </FormControl>
                  </Box>
                  <Box>
                    <FormControl id="lastName">
                      <FormLabel>Last Name</FormLabel>
                      <InputField type="text" name="lastName" />
                    </FormControl>
                  </Box>
                </HStack>
                <FormControl id="email" isRequired>
                  <FormLabel>Email address</FormLabel>
                  <InputField type="email" name="email" />
                </FormControl>
                <FormControl id="password" isRequired>
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
                <Stack spacing={10} pt={2}>
                  <Button
                    type="submit"
                    loadingText="Submitting"
                    size="lg"
                    bg={"blue.400"}
                    color={"white"}
                    _hover={{
                      bg: "blue.500",
                    }}
                  >
                    Sign Up
                  </Button>
                </Stack>
                <Stack pt={6}>
                  <Text align={"center"}>
                    Already a user?
                    <Button
                      onClick={loadSignInPage}
                      spacing={2}
                      margin={2}
                      color={"blue.400"}
                      variant={"link"}
                    >
                      Login
                    </Button>
                  </Text>
                </Stack>
              </Stack>
            </Form>
          </Formik>
        </Box>
      </Stack>
    </Flex>
  );
}
