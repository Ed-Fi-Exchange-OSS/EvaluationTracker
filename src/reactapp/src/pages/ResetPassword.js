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
  Stack,
  Button,
  Heading,
  Text,
  useColorModeValue,
  useToast,
  Skeleton
} from "@chakra-ui/react";
import { useState } from "react";
import { ViewIcon, ViewOffIcon } from "@chakra-ui/icons";
import { Formik, Form } from "formik";
import { useNavigate, useParams } from "react-router-dom";
import InputField from "../components/InputField";
import { get, post } from "../components/FetchHelpers";
import { defaultErrorMessage, AlertMessage } from "../components/AlertMessage";
import { useEffect } from "react";

export default function ResetPassword() {
  const { id } = useParams();
  const navigate = useNavigate();
  const toast = useToast();
  const [showPassword, setShowPassword] = useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState(false);
  const [error, setError] = useState(null);
  const [invalidTokenError, setInvalidTokenError] = useState(null);
  const [isLoaded, setIsLoaded] = useState(false);
  const expiredLink = "We’re sorry, but the password reset link you clicked has expired. This can happen if you waited too long to reset your password, or if you already used this link before.";

  

  useEffect(() => {
    const validateToken = async () => {
      try {
        const response = await get('/accounts/ValidatePasswordResetToken/?passwordResetToken=' + id);

        if (!response.ok) {
          console.error("Failed to validate token");
          setInvalidTokenError(expiredLink);
        }
      } catch (error) {
        console.error("Error validating token:", error);
      }
      setIsLoaded(true);
    };
    try {

      if (id) {
        validateToken();
      }
      else {
        setInvalidTokenError(expiredLink);
        setIsLoaded(true);
      }
    }
    catch (error) {
      console.error("Error:", error);
      setIsLoaded(true);
    }
    setIsLoaded(true);
  }, [id]);

  
  const onSubmitSignup = async (values) => {
   
    try {
      if (!values.password) return;
      // if password don't match, stop the flow.
      if (values.password !== values.confirmPassword) {
        setError('Passwords do not match');
        return;
      }
      setIsLoaded(false);
      const response = await post(`/accounts/ResetPassword?passwordResetToken=${id}&newPass=${values.password}`);
      if (response.ok) {
        toast({
          title: "Success.",
          description: "Your password has been successfully reseted.",
          status: "success",
          duration: 5000,
          isClosable: true,
        });
        navigate("/");
      }
      else {
        setIsLoaded(true);
        toast({
          title: "An error occurred.",
          description: "Unable to reset the password.",
          status: "error",
          duration: 5000,
          isClosable: true,
        });
        const message = await response.json();
        let listErrors = [];
        let alertMessage;
        // Message error Handling
        if (message.validationError) {
          if (message.validationError.errors) {
            listErrors = message.validationError.errors;
          }
          else if (message.validationError.error) {
            listErrors = message.validationError.error.errors;
          }
          else if (message.validationError.Password) {
            listErrors = message.validationError.Password.errors;
          }
          if (listErrors) {
            alertMessage = listErrors.map((v) => v.errorMessage);
            setError(alertMessage);
            console.error(alertMessage);
          }
          return;
        } else if (response.status === 409) {
          setError("This email address has already been registered.");
          return false;
        }
        else if (message.message) {
          setError(message.message);
          return false;
        }
      }      
    } catch (error) {
      console.error(error);
      setError(defaultErrorMessage);
      setIsLoaded(true);
      return false;
    }
    setIsLoaded(true);
  };

  return (
    <Skeleton isLoaded={isLoaded} count={3.5} >
<Flex minH={"100vh"} align={"center"} justify={"center"} bg={useColorModeValue("gray.50", "gray.800")}>
      <Stack spacing={8} mx={"auto"} maxW={"lg"} py={12} px={6}>
        <Stack align={"center"}>
          <Heading fontSize={"4xl"} textAlign={"center"}>
            Reset Password
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
               {!invalidTokenError && <>
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
                <FormControl id="confirmPassword" isRequired>
                  <FormLabel>Confirm Password</FormLabel>
                  <InputGroup>
                    <InputField type={showPassword ? "text" : "password"} name="confirmPassword" />
                    <InputRightElement h={"full"}>
                      <Button
                        variant={"ghost"}
                        onClick={() => setShowConfirmPassword((showConfirmPassword) => !showConfirmPassword)}
                      >
                        {showConfirmPassword ? <ViewIcon /> : <ViewOffIcon />}
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
                    Reset Password
                  </Button>
                </Stack></>}
                  <Stack spacing={10} textAlign={"left"} pt={2}>
                  {error && <AlertMessage message={error} />}
                </Stack>
              </Stack>
            </Form>
          </Formik>
        </Box>
      </Stack>
    </Flex>
    </Skeleton>
  );
}
