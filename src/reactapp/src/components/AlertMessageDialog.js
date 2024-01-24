// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

import React from 'react';
import { AlertDialog, AlertDialogBody, AlertDialogFooter, AlertDialogHeader, AlertDialogContent, AlertDialogOverlay, Box, Button, Spinner, Text } from "@chakra-ui/react";
import { WarningIcon, InfoIcon, CheckCircleIcon } from "@chakra-ui/icons";

const AlertMessageDialog = ({ buttonText, buttonColorScheme, alertTitle, message, onYes, showIcon, buttonType = "yesno", buttonDisabled = false }) => {
  const [isOpen, setIsOpen] = React.useState(false);
  const [isButtonDisabled] = React.useState(buttonDisabled ?? false);
  const [isLoading, setIsLoading] = React.useState(false);
  const onClose = () => setIsOpen(false);
  const cancelRef = React.useRef();

  const handleYesClick = async () => {
    setIsLoading(true);
    await onYes();
    onClose();
    setIsLoading(false);
  };

  const getIcon = () => {
    switch (showIcon) {
      case 'warning':
        return <WarningIcon />;
      case 'error':
        return <CheckCircleIcon />;
      case 'info':
        return <InfoIcon />;
      default:
        return null;
    }
  };

  const getButtons = () => {
    switch (buttonType) {
      case 'yesno':
        return (
          <>
            <Button ref={cancelRef} onClick={onClose}>
              No
            </Button>
            <Button colorScheme="red" onClick={handleYesClick} ml={3}>
              Yes
            </Button>
          </>
        );
      case 'ok':
        return (
          <Button colorScheme="green" onClick={handleYesClick} ml={3}>
            OK
          </Button>
        );
      case 'yesnocancel':
        return (
          <>
            <Button ref={cancelRef} onClick={onClose}>
              No
            </Button>
            <Button colorScheme="blue" onClick={handleYesClick} ml={3}>
              Yes
            </Button>
            <Button colorScheme="gray" onClick={onClose} ml={3}>
              Cancel
            </Button>
          </>
        );
      default:
        return null;
    }
  };

  return (
    <>  
      <Button colorScheme={buttonColorScheme} isDisabled={isButtonDisabled} onClick={() => setIsOpen(!isButtonDisabled)}>{buttonText}</Button>
      <AlertDialog isOpen={isOpen} closeOnOverlayClick={false} leastDestructiveRef={cancelRef} onClose={onClose}>
        <AlertDialogOverlay>
          <AlertDialogContent>
            <AlertDialogHeader fontSize="lg" fontWeight="bold">
              { alertTitle }
            </AlertDialogHeader>
            <AlertDialogBody>
              {isLoading ? (<><Box display="flex" alignItems="center"><Spinner role="status" />
                <Text ml={4}>Loading...</Text></Box></>) 
                : (<>{getIcon()} {message} </>) }
            </AlertDialogBody>
            <AlertDialogFooter>
              {!isLoading && getButtons()}
            </AlertDialogFooter>
          </AlertDialogContent>
        </AlertDialogOverlay>
          </AlertDialog>
    </>
  );
}

export { AlertMessageDialog };
