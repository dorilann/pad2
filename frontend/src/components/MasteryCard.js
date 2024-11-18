import React from 'react';
import '../styles/MasteryCard.css';

const MasteryCard = ({ champion }) => {
  const {
    puuId,
    championId,
    name,
    level,
    points,
    pointsUntilNextLevel,
    chestGranted,
    lastPlayTime,
    imageUrl,
  } = champion;

  return (
    <div className="mastery-card">
      <img src={imageUrl} alt={name} className="champion-image" />
      <h3>{name}</h3>
      <p><strong>PUUID:</strong> {puuId}</p>
      <p><strong>Level:</strong> {level}</p>
      <p><strong>Points:</strong> {points}</p>
      <p><strong>Points Until Next Level:</strong> {pointsUntilNextLevel}</p>
      <p><strong>Chest Granted:</strong> {chestGranted ? 'Yes' : 'No'}</p>
      <p><strong>Last Played:</strong> {lastPlayTime}</p>
    </div>
  );
};

export default MasteryCard;
