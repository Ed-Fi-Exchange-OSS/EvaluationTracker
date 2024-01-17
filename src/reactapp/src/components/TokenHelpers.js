// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

import jwt_decode from "jwt-decode"
import { postForm, } from "./FetchHelpers";

const setToken = (tokenResponse) => {
  if (tokenResponse) {
    sessionStorage.setItem('token', tokenResponse.access_token);
    if (tokenResponse.refresh_token) {
      sessionStorage.setItem('refresh_token', tokenResponse.refresh_token);
    }
  }
};

const isTokenExpired = () => {
  const token = sessionStorage.getItem('token');
  if (token) {
    const payload = JSON.parse(atob(token.split('.')[1]));
    const expirationDate = new Date(payload.exp * 1000);
    return expirationDate < new Date();
  }
  else {
    return true;
  }
};


const getToken = () => {
  const token = sessionStorage.getItem('token');
  if (token) {
    if (isTokenExpired()) {
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


const getRefreshToken = () => {
  return sessionStorage.getItem('refresh_token');
};

const getUserIdFromToken = (jwt) => {
  if (!jwt) return null;
  return jwt_decode(jwt).sub;
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


const validateAuthenticationToken = async () => {
  try {
    const current_token = sessionStorage.getItem('token');
    const refresh_token = getRefreshToken();
    if (!refresh_token || !current_token) {
      return false;
    }
    const tokenRequest = {
      grant_type: "refresh_token",
      refresh_token: refresh_token,
    };
    if (!isTokenExpired()) {
      return true;
    }
    else {
      const response = await postForm("/connect/token", tokenRequest);
      if (!response.ok) {
        console.error("Error when try to refresh the token");
        return false;
      }
      const data = await response.json();
      if (getUserIdFromToken(data.access_token) === getUserIdFromToken(current_token)) {
          clearToken();
          setToken(data);
          return true
      }
    }
  } catch (exception) {
    console.error(exception);
  }
  return false;
};

const isLoggedInUserInRole = (roles) => {
  const jwt = getToken();
  if (!jwt) return null;
  const userRoles = jwt_decode(jwt).role;
  const currentUserRolesList = Array.isArray(userRoles) ? userRoles : [userRoles];
  const rolesToValidate = Array.isArray(roles) ? roles : [roles];
  return currentUserRolesList.some(item => rolesToValidate?.includes(item));
};

const clearToken = () => {
  sessionStorage.removeItem('token');
  sessionStorage.removeItem('refresh_token');
}

export { setToken, getToken, clearToken, getLoggedInUserId, getLoggedInUserName, getLoggedInUserFirstName, getLoggedInUserRole, isLoggedInUserInRole, validateAuthenticationToken }
