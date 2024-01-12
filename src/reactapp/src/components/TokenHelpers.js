// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

import jwt_decode from "jwt-decode"

const setToken = (tokenResponse) => {
  if (tokenResponse)
    sessionStorage.setItem('token', tokenResponse.access_token);
};

const getToken = () => {
  const token = sessionStorage.getItem('token');
  if (token) {
    const payload = JSON.parse(atob(token.split('.')[1]));
    const expirationDate = new Date(payload.exp * 1000);
    if (expirationDate < new Date()) {
      sessionStorage.removeItem('token');
      return null; 
    }
    else {
      return token
    }
  }
  else {
    return null;
  }
};

const getLoggedInUserId = () => {
  const jwt = getToken();
  if (!jwt) return null;
  return jwt_decode(jwt).sub;
};

const getLoggedInUserName = () => {
  const jwt = getToken();
  if (!jwt) return null;
  return jwt_decode(jwt).name;
};

const getLoggedInUserFirstName = () => {
  const username = getLoggedInUserName();
  if (!username) return null;
  return username.split(" ")[0];
};

const getLoggedInUserRole = () => {
  const jwt = getToken();
  if (!jwt) return null;
  return jwt_decode(jwt).role;
};

const clearToken = () => {
  sessionStorage.removeItem('token');
}

export { setToken, getToken, clearToken, getLoggedInUserId, getLoggedInUserName, getLoggedInUserFirstName, getLoggedInUserRole }
