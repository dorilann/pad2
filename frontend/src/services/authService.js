import api from './axiosConfig'; // Подключаем настроенный axios

// Функция для логина
export const login = async (username, password) => {
  try {
    const response = await api.post('/login', { username, password }); // Запрос на сервер
    const { token } = response.data; // Получаем токен из ответа
    localStorage.setItem('token', token); // Сохраняем токен в локальное хранилище
    return token;
  } catch (error) {
    if (error.response && error.response.data) {
      throw new Error(error.response.data.message || 'Login failed');
    }
    throw new Error('Network error or server is unavailable');
  }
};

// Функция для регистрации
export const register = async (username, password, confPassword) => {
  try {
    const response = await api.post('/register', {
      username,
      password,
      confPassword,
    });
    return response.data; // Возвращаем данные из успешного ответа
  } catch (error) {
    if (error.response && error.response.data) {
      throw new Error(error.response.data.message || 'Registration failed');
    }
    throw new Error('Network error or server is unavailable');
  }
};
