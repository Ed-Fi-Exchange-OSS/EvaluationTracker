// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

import {
  Center,
  Stack,
  Heading,
  Skeleton
} from "@chakra-ui/react";
import { useNavigate } from "react-router-dom";
import "../App.css";
import React, { useEffect, useState } from "react";
import { get } from "../components/FetchHelpers";
import { getLoggedInUserId, getLoggedInUserRole } from "../components/TokenHelpers";
import SortableTable from "../components/SortableTable";

//Created a table to display the data from react objects
export default function UserListTable() {
  const [userList, setUserList] = useState([]);
  const loggedInUserRole = getLoggedInUserRole();
  const [componentsDataLoaded, setComponentsDataLoaded] = useState(false);
  const headers = [
    { name: '', label: 'Edit', sortable: true, visible: loggedInUserRole === 'Administrator', link: { url: '/userProfile/', dataField: 'id' } },
    { name: 'First Name', dataField: 'firstName' , sortable: true, visible: true },
    { name: 'Last Name', dataField: 'lastName', sortable: true, visible: true },
    { name: 'E-mail', dataField: 'email', sortable: true, visible: true },
    { name: 'Roles', dataField: 'roles', sortable: true, visible: true }
  ];
  const navigate = useNavigate();

  useEffect(() => {
    const userId = getLoggedInUserId();

    if (userId == null) {
      navigate("/login");
    }
    fetchUserList().then(() => {
      setComponentsDataLoaded(true);
    });
  }, [navigate]);

// Retrieve evaluation ratings from API
  const fetchUserList = async (userId) => {
    try {
      let response;
      response = await get(`/accounts`);

      if (!response.ok) {
        throw new Error("Failed to fetch users list");
      }

      const data = await response.json();
      setUserList(data);
    } catch (error) {
      console.error("Error fetching users list:", error);
    }
  };
  
  return (
    <Skeleton isLoaded={componentsDataLoaded}>
      <Stack spacing={8} mt="24px" px={{ base: "10px", md: "30px" }} align="center">
        <Heading fontSize={{ base: "3xl", md: "5xl" }}>User List</Heading>
        <Center>
          <SortableTable data={userList} headers={headers} paginate={true} itemsPerPage={50} noRowsMessage="No user to display" />
        </Center>
      </Stack>
      </Skeleton>
  );
}
