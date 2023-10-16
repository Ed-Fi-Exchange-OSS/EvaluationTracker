// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

import jwt_decode from "jwt-decode"

const setToken = (tokenResponse) => {
  sessionStorage.setItem('token', tokenResponse.access_token);
};

const getToken = () => {
  return sessionStorage.getItem('token');
};

const getLoggedInUserId = () => {
  const jwt = sessionStorage.getItem('token');
  return jwt_decode(jwt).sub;
};

const getLoggedInUserName = () => {
  const jwt = sessionStorage.getItem('token');
  return jwt_decode(jwt).name;
};

const getLoggedInUserRole = () => {
  const jwt = sessionStorage.getItem('token');
  return jwt_decode(jwt).role;
};

const clearToken = () => {
  sessionStorage.removeItem('token');
}

export { setToken, getToken, clearToken, getLoggedInUserId, getLoggedInUserName, getLoggedInUserRole }
