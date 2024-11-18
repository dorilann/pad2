import React from 'react';
import AuthForm from '../components/AuthForm';
import { useNavigate } from 'react-router-dom';
import { login, register } from '../services/authService';
import '../styles/AuthForm.css';

const AuthPage = () => {
  const navigate = useNavigate();

  const handleLogin = async (username, password) => {
    try {
      await login(username, password);
      navigate('/home');
    } catch (error) {
      alert(error.message);
    }
  };

  const handleRegister = async (username, password, confirmPassword) => {
    try {
      await register(username, password, confirmPassword);
      alert("Registration successful! You can now log in.");
      navigate('/');
    } catch (error) {
      alert(error.message);
    }
  };

  return (
    <div className="auth-page">
      <AuthForm onLogin={handleLogin} onRegister={handleRegister} />
    </div>
  );
};

export default AuthPage;
