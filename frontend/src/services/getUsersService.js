import api from './axiosConfig';

// Получение данных профиля
export const fetchProfileData = async (region, server, username, tag) => {
  try {
    const response = await api.get(
      `/account/${region}/${server}/${username}/${tag}`
    );
    return response.data; // Объект Profile с AccountModel и ChampionMastery
  } catch (error) {
    throw new Error(
      error.response?.data?.message || 'Failed to fetch profile data'
    );
  }
};
