import React from 'react';
import LoginForm from './pages/Login';
// 1. import `ChakraProvider` component
import { ChakraProvider } from '@chakra-ui/react'
import SignupForm from './pages/Signup';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Nav from './components/Navbar';
import EvaluationTable from './pages/Main';
import NewEvaluation from './pages/NewEvaluation';
import EvaluationForm from './pages/EvaluationForm';
import PageTracker from "./components/PageTracker";
import AuthenticatedRoute from './components/AuthenticatedRoute';
import UserListTable from './pages/UserList';
import UserProfile from "./pages/UserProfile";
import ResetPassword from "./pages/ResetPassword";
import ForgotPassword from "./pages/ForgotPassword";

import { ApplicationRoles } from "./constants";

function App() {
    // 2. Wrap ChakraProvider at the root of my app
  return (
    <ChakraProvider>
      <Router>
      <PageTracker/>
      <Nav />
      <Routes>
        <Route path="/" element={<LoginForm />} />
        <Route path="/signup" element={<SignupForm />} />
        <Route path="/login" element={<LoginForm />} />
        <Route path="/main" element={<AuthenticatedRoute roles={[ApplicationRoles.Evaluator, ApplicationRoles.Reviewer]}  element={<EvaluationTable />} />} />
        <Route path="/new" element={<AuthenticatedRoute roles={[ApplicationRoles.Evaluator, ApplicationRoles.Reviewer]} element={<NewEvaluation />} />} />
        <Route path="/evaluation/:id?" element={<AuthenticatedRoute roles={[ApplicationRoles.Evaluator, ApplicationRoles.Reviewer]} element={<EvaluationForm />} />} />
        <Route path="/users" element={<AuthenticatedRoute roles={[ApplicationRoles.Administrator]} element={<UserListTable />} />} />
        <Route path="/userProfile/:id?" element={<AuthenticatedRoute roles={[ApplicationRoles.Administrator]} element={<UserProfile />} />} />
        <Route path="/resetPassword/:id" element={<ResetPassword />} />
        <Route path="/forgotPassword" element={<ForgotPassword />} />
      </Routes>
      </Router>
    </ChakraProvider>
  );
}

export default App;
