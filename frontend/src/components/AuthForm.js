import React, { useState } from 'react';
import '../styles/AuthForm.css';

const AuthForm = ({ onLogin, onRegister }) => {
  const [isRegister, setIsRegister] = useState(false);
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    setLoading(true);
    try {
      if (username.length < 3) {
        throw new Error("Username must be at least 3 characters long");
      }
      if (password.length < 6) {
        throw new Error("Password must be at least 6 characters long");
      }
      if (isRegister) {
        if (password === confirmPassword) {
          await onRegister(username, password, confirmPassword);
        } else {
          throw new Error("Passwords do not match");
        }
      } else {
        await onLogin(username, password);
      }
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  const toggleForm = () => {
    setIsRegister(!isRegister);
    setUsername('');
    setPassword('');
    setConfirmPassword('');
    setError('');
  };

  return (
    <form className="auth-form" onSubmit={handleSubmit}>
      <h2>{isRegister ? 'Register' : 'Login'}</h2>
      <input
        type="text"
        placeholder="Username"
        value={username}
        onChange={(e) => setUsername(e.target.value)}
        required
      />
      <input
        type="password"
        placeholder="Password"
        value={password}
        onChange={(e) => setPassword(e.target.value)}
        required
      />
      {isRegister && (
        <input
          type="password"
          placeholder="Confirm Password"
          value={confirmPassword}
          onChange={(e) => setConfirmPassword(e.target.value)}
          required
        />
      )}
      <button type="submit" disabled={loading}>
        {loading ? 'Loading...' : isRegister ? 'Register' : 'Login'}
      </button>
      {error && <p className="error-text">{error}</p>}
      <p className="toggle-text" onClick={toggleForm}>
        {isRegister ? 'Already have an account? Log in' : "Don't have an account? Register"}
      </p>
    </form>
  );
};

export default AuthForm;
