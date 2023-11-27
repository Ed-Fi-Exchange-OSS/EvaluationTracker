import React from 'react';
import { Alert, AlertIcon, AlertDescription } from "@chakra-ui/react";

const defaultErrorMessage = 'Something went wrong. Please try again later.';
// message can be string | string[]
const AlertMessage = ({ message, status='error' }) => {
  let content;
  if (Array.isArray(message)) {
    // If message is an array of strings, map over the array to create list items
    const listItems = message.map((item, index) => <li>{item}</li>);
    content = <ul>{listItems}</ul>;
  } else {
    // If message is a string, display it as is
    content = message;
  }
  return (
    <Alert status={status} borderRadius="md">
      <AlertDescription m={ 4 }>{content}</AlertDescription>
    </Alert>
  );
};

export { defaultErrorMessage, AlertMessage };
