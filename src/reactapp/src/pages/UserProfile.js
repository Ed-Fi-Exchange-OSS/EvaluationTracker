// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

import {
  ButtonGroup,
  Box,
  Center,
  Checkbox,
  CheckboxGroup,
  Container,
  FormControl,
  FormLabel,
  Heading,
  Input,
  Skeleton,
  Stack,
  StackDivider,
  useColorModeValue,
  useToast
} from "@chakra-ui/react";
import { AlertMessageDialog } from "../components/AlertMessageDialog"
import { useNavigate, useParams } from "react-router-dom";
import "../App.css";
import React, { useEffect, useState } from "react";
import { get, put } from "../components/FetchHelpers";
import { ApplicationRoles } from "../constants";

//Created a table to display the data from react objects
export default function UserProfile() {
  const [componentsDataLoaded, setComponentsDataLoaded] = useState(false);
  const [userData, setUserData] = useState([]);
  const [selectedItems, setSelectedItems] = useState([]);
  const roleOptions = Object.values(ApplicationRoles);
  const { id } = useParams();
  const borderColor = useColorModeValue("gray.200", "gray.600");
  const navigate = useNavigate();
  const toast = useToast();

  const handleCheckChange = (values) => {
    setSelectedItems(values);
  };

  const getCompletedData = () => {
    const userDataCompleted = {};
    userDataCompleted.email = userData.email;
    userDataCompleted.firstName = userData.firstName;
    userDataCompleted.lastName = userData.lastName
    userDataCompleted.id = id;
    userDataCompleted.roles = selectedItems;
    return userDataCompleted;
  }

  const saveRoles = async () => {
    try {
      const completedUserData = getCompletedData();
      const response = await put(`/accounts/${id}`, completedUserData);
      if (response.ok) {
        toast({
          title: "Success.",
          description: "The user roles have been updated successfully.",
          status: "success",
          duration: 5000,
          isClosable: true,
        });
        navigate("/users");
      }
      else {
        toast({
          title: "An error occurred.",
          description: "Unable to update the user roles.",
          status: "error",
          duration: 5000,
          isClosable: true,
        });
      }
    } catch (e) {
      toast({
        title: "An error occurred.",
        description: "Unable to update the user roles.",
        status: "error",
        duration: 5000,
        isClosable: true,
      });
      console.error("Error:", e);
    }
    return true;
  }
  
  useEffect(() => {
    // Retrieve evaluation ratings from API
    const fetchUserData = async (userId) => {
      try {
        let response;
        response = await get(`/accounts/${id}`);

        if (!response.ok) {
          throw new Error("Failed to fetch users data");
        }
        const data = await response.json();
        setUserData(data);
        setSelectedItems(data?.roles ?? []);
      } catch (error) {
        console.error("Error fetching users list:", error);
      }
    };
    fetchUserData().then(() => {
      setComponentsDataLoaded(true);
    });
  }, [id]);



  return (
    <Skeleton isLoaded={componentsDataLoaded}>
      <Container maxW={"7xl"} mb='10'>
        <Stack textAlign={"center"}
          spacing={{ base: 4, sm: 6 }}
          direction={"column"}
          divider={
            <StackDivider
              borderColor={borderColor}
            />
          }
        >
        <Heading
          lineHeight={1.1}
          mt={5}
          mb={5}
          fontSize={"3xl"}
          fontWeight={"700"}
        >
            User Profile</Heading>
        </Stack>
        <Center>
        <Stack textAlign={"center"} spacing={{ base: 4, sm: 6 }} direction={"column"} style={{ width: '80%' }}>
        <FormControl display="flex" alignItems="center">
          <FormLabel mb="0">First Name:</FormLabel>
          <Input value={userData.firstName} isReadOnly />
        </FormControl>
        <FormControl display="flex" alignItems="center">
          <FormLabel mb="0">Last Name:</FormLabel>
          <Input value={userData.lastName} isReadOnly />
        </FormControl>
        <FormControl display="flex" alignItems="center">
          <FormLabel mb="0">Email:</FormLabel>
          <Input value={userData.email} isReadOnly />
        </FormControl>
        <FormLabel>Roles</FormLabel>
        <CheckboxGroup colorScheme="green" defaultValue={selectedItems} onChange={handleCheckChange}>
              {roleOptions?.map((item, index) => (
            <Checkbox key={index} value={item}>
              {item}
            </Checkbox>
          ))}
            </CheckboxGroup>
            <Box textAlign="center">                                  
              <ButtonGroup variant="outline" spacing="6">
                <AlertMessageDialog showIcon="warning" alertTitle="Save Roles" buttonColorScheme="blue" buttonText="Save" message="Are you sure you want to save the user roles?" onYes={() => { saveRoles() }}></AlertMessageDialog>
                <AlertMessageDialog showIcon="warning" alertTitle="Cancel" buttonText="Cancel" message="Are you sure you want to cancel this process? All unsaved changes will be lost" onYes={() => { navigate("/users"); }}></AlertMessageDialog>
              </ButtonGroup>
            </Box>
          </Stack>
        </Center>
      </Container>
      </Skeleton>
  );
}
