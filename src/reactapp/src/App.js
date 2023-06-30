import React from 'react';
import LoginForm from './pages/Login';
// 1. import `ChakraProvider` component
import { ChakraProvider } from '@chakra-ui/react'
import SignupForm from './pages/Signup';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { Link } from 'react-router-dom';
import Nav from './components/Navbar';

function App() {
    // 2. Wrap ChakraProvider at the root of my app
    return (
        <ChakraProvider>
            <Router>
                <Nav />
                <Routes>
                    <Route path="/" element={<SignupForm />} />
                    <Route path="/login" element={<LoginForm />} />
                </Routes>
            </Router>
        </ChakraProvider>
    );
}

export default App;
