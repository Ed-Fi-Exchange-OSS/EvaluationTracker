// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

import { useEffect, useState } from "react";
import { useNavigate } from 'react-router-dom';
import {
  useToast,
} from '@chakra-ui/react';
import { getToken, isLoggedInUserInRole, validateAuthenticationToken } from "../components/TokenHelpers";
import { AlertMessage } from "../components/AlertMessage";

const AuthenticatedRoute = ({ element: authenticatedComponent, roles, ...rest }) => {
  let navigate = useNavigate();
  const toast = useToast();
  const [alertMessageText, setAlertMessageText] = useState(null);

  useEffect(() => {
    // getToken returns n0ull if the current session doesn't have a valid token.
    const validateToken = async () => {
      return await validateAuthenticationToken();
    };
    validateToken().then(isTokenValid => {
      if (!isTokenValid) {
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
    }
    if (roles && !roles.includes('*') && !isLoggedInUserInRole(roles)) {
      setAlertMessageText("You do not have access to the requested page");
    }
    else
      setAlertMessageText(null);
  });

  return <>{alertMessageText
    ? <AlertMessage status="warning" message={alertMessageText} />
    :  authenticatedComponent }</>;
}
 export default AuthenticatedRoute;
