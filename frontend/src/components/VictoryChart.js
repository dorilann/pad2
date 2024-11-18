import React from 'react';
import { Bar } from 'react-chartjs-2';
import { Chart as ChartJS, BarElement, CategoryScale, LinearScale, Tooltip, Legend } from 'chart.js';

// Регистрация компонентов Chart.js
ChartJS.register(BarElement, CategoryScale, LinearScale, Tooltip, Legend);

const VictoryChart = ({ wins, losses }) => {
  const data = {
    labels: ['Wins', 'Losses'],
    datasets: [
      {
        label: 'Games',
        data: [wins, losses],
        backgroundColor: ['#4CAF50', '#F44336'], // Зеленый и красный
        borderColor: ['#388E3C', '#D32F2F'],
        borderWidth: 1,
      },
    ],
  };

  const options = {
    responsive: true,
    plugins: {
      legend: {
        display: false, // Скрыть легенду
      },
      tooltip: {
        callbacks: {
          label: (context) => `${context.raw} games`,
        },
      },
    },
    scales: {
      x: {
        grid: {
          display: false,
        },
      },
      y: {
        beginAtZero: true,
        ticks: {
          stepSize: 1,
        },
      },
    },
  };

  return <Bar data={data} options={options} />;
};

export default VictoryChart;
