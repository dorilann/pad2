import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import Header from '../components/Header';
import MasteryCard from '../components/MasteryCard';
import VictoryChart from '../components/VictoryChart';
import '../styles/ProfilePage.css';
import { fetchProfileData } from '../services/getUsersService';

const ProfilePage = () => {
  const { region, server, username, tag } = useParams();
  const [profileData, setProfileData] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        setLoading(true);
        const profileResponse = await fetchProfileData(region, server, username, tag);
        setProfileData(profileResponse);
      } catch (err) {
        console.error('Error fetching profile data:', err.message);
        setError(err.message);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [region, server, username, tag]);

  if (loading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>Error: {error}</div>;
  }

  if (!profileData || !profileData.account || !profileData.masteries) {
    return <div>No profile data available.</div>;
  }

  const { account, masteries } = profileData;

  return (
    <div>
      <Header />
      <div className="profile-header">
        <h1>
          {account.gameName}#{account.tagLine} ({server.toUpperCase()})
        </h1>
        <p>Level {account.summonerLevel}</p>
        <div className="profile-rating">
          <div>
            <strong>Rank:</strong> {account.tier} {account.rank}
          </div>
          <div>
            <strong>League Points:</strong> {account.leaguePoints}
          </div>
          <div>
            <strong>Wins:</strong> {account.wins}
          </div>
          <div>
            <strong>Losses:</strong> {account.losses}
          </div>
        </div>
      </div>

      <div className="victory-chart">
        <VictoryChart wins={account.wins} losses={account.losses} />
      </div>

      <div className="champions-section">
        <h2>Мастерство</h2>
        <div className="champions-list">
          {masteries.map((champion) => (
            <MasteryCard
              key={champion.championId}
              champion={{
                puuId: champion.puuId,
                championId: champion.championId,
                name: `Champion ID: ${champion.championId}`, // Имя чемпиона не указано в данных, поэтому отображаем ID
                level: champion.championLevel,
                points: champion.championPoints,
                pointsUntilNextLevel: champion.championPointsUntilNextLevel,
                chestGranted: champion.chestGranted,
                lastPlayTime: new Date(champion.lastPlayTime).toLocaleDateString(),
                imageUrl: `https://ddragon.leagueoflegends.com/cdn/12.23.1/img/champion/${champion.championId}.png`, // Пример URL для изображения чемпиона
              }}
            />
          ))}
        </div>
      </div>
    </div>
  );
};

export default ProfilePage;
