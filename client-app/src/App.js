import React, { useState, useEffect } from 'react';
import { ThemeProvider } from '@mui/material/styles';
import { CssBaseline, AppBar, Toolbar, Typography, IconButton, Drawer, List, ListItem, ListItemIcon, ListItemText, Box, Grid } from '@mui/material';
import MenuIcon from '@mui/icons-material/Menu';
import WbSunnyIcon from '@mui/icons-material/WbSunny';
import CloudIcon from '@mui/icons-material/Cloud';
import Button from '@mui/material/Button';
import axios from 'axios';
import theme from './theme';
import WeatherCard from './WeatherCard';

function App() {
    const [drawerOpen, setDrawerOpen] = useState(false);
    const [weatherData, setWeatherData] = useState([]);

    const handleDrawerOpen = () => {
        setDrawerOpen(true);
    };

    const handleDrawerClose = () => {
        setDrawerOpen(false);
    };

    const handleButtonClick = async () => {
        const parametro = '3477'; // ID da cidade (exemplo)
        try {
            console.log('Chamando a API...');
            const response = await axios.get(`https://localhost:7144/api/ClimaTempo/Tempo15D/${parametro}`);
            console.log('Resposta recebida:', response);
            console.log('Dados da resposta:', response.data);

            if (response.data) {
                // Atualizar os dados do clima com os dados da API
                const formattedWeatherData = response.data.map(item => {
                    const icon = item.Temperature.Max < 20 ? <CloudIcon /> : <WbSunnyIcon />;
                    return {
                        date: item.Date,
                        temperatureMin: item.Temperature.Min,
                        temperatureMax: item.Temperature.Max,
                        description: item.Description,
                        icon: icon
                    };
                });

                setWeatherData(formattedWeatherData);
            } else {
                console.error('Dados da resposta inválidos:', response.data);
            }

        } catch (error) {
            console.error('Erro ao chamar a API:', error);
        }
    };

    useEffect(() => {
        handleButtonClick();
    }, []);

    return (
        <ThemeProvider theme={theme}>
            <CssBaseline />
            <Box p={3}>
                <Grid container spacing={3}>
                    {weatherData.map((data, index) => (
                        <Grid item xs={12} sm={6} md={4} key={index}>
                            <WeatherCard {...data} />
                        </Grid>
                    ))}
                </Grid>
            </Box>
        </ThemeProvider>
    );
}

export default App;