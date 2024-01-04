// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

import { useEffect } from "react";
import { useNavigate } from 'react-router-dom';
import {
  useToast,
} from '@chakra-ui/react';
import { getToken, isTokenExpired, refreshAuthenticationToken } from "../components/TokenHelpers";
import { defaultErrorMessage } from "../components/AlertMessage";

const AuthenticatedRoute = ({ element: authenticatedComponent, ...rest }) => {
  let navigate = useNavigate();
  const toast = useToast();

  const isTokenValid = async () => {
    try {
      if (!isTokenExpired()) {
        return true;
      }
      else {
        return await refreshAuthenticationToken();
      }
    } catch (exception) {
      console.error(exception);
      return false;
    }
  };

  useEffect(() => {
    if (!isTokenValid()) {
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
