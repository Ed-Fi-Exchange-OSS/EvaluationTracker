// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

// Adapted from https://chakra-ui.com/community/recipes/advanced-formik-integration

import { useField } from "formik";
import {
  Input as ChakraInput,
} from "@chakra-ui/react";


const InputField = ({ name, ...props }) => {
  const [field] = useField(name);
  return <ChakraInput {...props} {...field} />;
};

export default InputField;
