import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import HomePage from './pages/homePage';
import AuthPage from './pages/AuthPage';
import ProfilePage from './pages/ProfilePage'; // Импорт страницы профиля


function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<AuthPage />} />
        <Route path="/home" element={<HomePage />} />
        <Route path="/profile/:region/:server/:username/:tag" element={<ProfilePage />} />
      </Routes>
    </Router>
  );
}

export default App;
