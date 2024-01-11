// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

import { useEffect } from "react";
import { useNavigate } from 'react-router-dom';
import {
  useToast,
} from '@chakra-ui/react';
import { getToken } from "../components/TokenHelpers";

const AuthenticatedRoute = ({ element: authenticatedComponent, ...rest }) => {
  let navigate = useNavigate();
  const toast = useToast();

  useEffect(() => {
    if (!getToken()) {
      toast({
        title: "Session has expired.",
        description: "Your session has expired.Please log in again to continue.",
        status: "warning",
        duration: 5000,
        isClosable: true,
      });
      navigate("/login");
      sessionStorage.clear();
    }
  });

  return <>{authenticatedComponent}</>;
}
 export default AuthenticatedRoute;
