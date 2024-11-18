import React from 'react';
import Header from '../components/Header';
import PlayerSearch from '../components/PlayerSearch';

const HomePage = () => {
    return (
      <div>
        <Header />
        <div style={{ display: 'flex', flexDirection: 'column', alignItems: 'center', marginTop: '20px' }}>
          <h1>Welcome to the League of Legends Profile Analyzer</h1>
          <p>Enter a Summoner's name to view their profile.</p>
          <PlayerSearch />
        </div>
      </div>
    );
  };
  
  export default HomePage;
