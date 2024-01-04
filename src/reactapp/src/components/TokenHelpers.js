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
  return sessionStorage.getItem('refresh-token');
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


const refreshAuthenticationToken = async () => {
  try {
    const tokenRequest = {
      grant_type: "refresh_token",
      refresh_token: getRefreshToken(),
    };
    if (!isTokenExpired()) {
      return true;
    }
    else {
      const response = await postForm("/connect/token", tokenRequest);
      const message = await response.json();

      if (!response.ok) {
        console.error(message);
        return false;
      }
      // Token user and previous stored user must be the same.
      if (getUserIdFromToken() === getLoggedInUserId()) {
        setToken(message);
        return true;
      }
    }
  } catch (exception) {
    console.error(exception);
  }
  return false;
};


const clearToken = () => {
  sessionStorage.removeItem('token');
}

export { refreshAuthenticationToken, setToken, getToken, getRefreshToken, clearToken, getLoggedInUserId, getLoggedInUserName, getLoggedInUserFirstName, getLoggedInUserRole }
