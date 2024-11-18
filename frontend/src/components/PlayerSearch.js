import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import '../styles/PlayerSearch.css';
const regions = [
    { label: 'Americas', value: 'americas' },
    { label: 'Asia', value: 'asia' },
    { label: 'Esports', value: 'esports' },
    { label: 'Europe', value: 'europe' },
  ];
  
  const servers = [
    { label: 'BR1', value: 'br1' },
    { label: 'EUN1', value: 'eun1' },
    { label: 'EUW1', value: 'euw1' },
    { label: 'JP1', value: 'jp1' },
    { label: 'KR', value: 'kr' },
    { label: 'LA1', value: 'la1' },
    { label: 'LA2', value: 'la2' },
    { label: 'ME1', value: 'me1' },
    { label: 'NA1', value: 'na1' },
    { label: 'OC1', value: 'oc1' },
    { label: 'PH2', value: 'ph2' },
    { label: 'RU', value: 'ru' },
    { label: 'SG2', value: 'sg2' },
    { label: 'TH2', value: 'th2' },
    { label: 'TR1', value: 'tr1' },
    { label: 'TW2', value: 'tw2' },
    { label: 'VN2', value: 'vn2' },
  ];
  
  const PlayerSearch = () => {
    const [region, setRegion] = useState('americas');
    const [server, setServer] = useState('eun1');
    const [username, setUsername] = useState('');
    const [tag, setTag] = useState('');
    const navigate = useNavigate();
  
    const handleSearch = () => {
      if (username.trim() && tag.trim()) {
        navigate(`/profile/${region}/${server}/${username}/${tag}`);
      }
    };
  
    return (
      <div className="player-search">
        <select
          value={region}
          onChange={(e) => setRegion(e.target.value)}
          className="player-search-select"
        >
          {regions.map((region) => (
            <option key={region.value} value={region.value}>
              {region.label}
            </option>
          ))}
        </select>
  
        <select
          value={server}
          onChange={(e) => setServer(e.target.value)}
          className="player-search-select"
        >
          {servers.map((server) => (
            <option key={server.value} value={server.value}>
              {server.label}
            </option>
          ))}
        </select>
  
        <input
          type="text"
          placeholder="Enter Summoner Name"
          value={username}
          onChange={(e) => setUsername(e.target.value)}
          className="player-search-input"
        />
  
        <input
          type="text"
          placeholder="Enter Tag"
          value={tag}
          onChange={(e) => setTag(e.target.value)}
          className="player-search-input"
        />
  
        <button onClick={handleSearch} className="player-search-button">
          Search
        </button>
      </div>
    );
  };
  
  export default PlayerSearch;