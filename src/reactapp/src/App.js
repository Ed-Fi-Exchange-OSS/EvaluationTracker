import React from 'react';
import LoginForm from './pages/Login';
// 1. import `ChakraProvider` component
import { ChakraProvider } from '@chakra-ui/react'
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Nav from './components/Navbar';
import EvaluationTable from './pages/Main';
import SignupForm from './pages/Signup';
import NewEvaluation from './pages/NewEvaluation';
import EvaluationForm from './pages/EvaluationForm';

function App() {
    // 2. Wrap ChakraProvider at the root of my app
    return (
        <ChakraProvider>
            <Router>
                <Nav />
                <Routes>
                    <Route path="/" element={<SignupForm />} />
                    <Route path="/login" element={<LoginForm />} />
                    <Route path="/main" element={<EvaluationTable />} />
                    <Route path="/new" element={<NewEvaluation />} />
                    <Route path="/evaluation" element={<EvaluationForm />} />"
                </Routes>
            </Router>
        </ChakraProvider>
    );
}

export default App;
