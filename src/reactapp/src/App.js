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
import UserProfile from "./pages/UserProfile";
import { ApplicationRoles } from "../constants";

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
        <Route path="/main" element={<AuthenticatedRoute roles={[ApplicationRoles.Mentor, ApplicationRoles.Supervisor]}  element={<EvaluationTable />} />} />
        <Route path="/new" element={<AuthenticatedRoute roles={[ApplicationRoles.Mentor, ApplicationRoles.Supervisor]} element={<NewEvaluation />} />} />
        <Route path="/evaluation/:id?" element={<AuthenticatedRoute roles={[ApplicationRoles.Mentor, ApplicationRoles.Supervisor]} element={<EvaluationForm />} />} />
        <Route path="/users" element={<AuthenticatedRoute roles={[ApplicationRoles.Administrator]} element={<UserListTable />} />} />
        <Route path="/userProfile/:id?" element={<AuthenticatedRoute roles={[ApplicationRoles.Administrator]} element={<UserProfile />} />} />
      </Routes>
      </Router>
    </ChakraProvider>
  );
}

export default App;
