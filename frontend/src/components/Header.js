import React from 'react';
import '../styles/Header.css';
import logo from '../assets/header2.png'; // Убедитесь, что изображение находится в папке assets и название файла совпадает

const Header = () => {
  return (
    <header className="header">
      <div className="header-content">
        <img src={logo} alt="Logo" className="header-logo" />
      </div>
    </header>
  );
};

export default Header;
