import React from 'react';
import { Card, CardContent, Typography, Box } from '@mui/material';
import WbSunnyIcon from '@mui/icons-material/WbSunny';
import CloudIcon from '@mui/icons-material/Cloud';

function WeatherCard({ date, temperatureMin, temperatureMax, description, icon }) {
    return (
        <Card style={{ margin: '1em' }}>
            <CardContent>
                <Typography variant="h5" component="div">
                    {new Date(date).toLocaleDateString()}
                </Typography>
                <Typography variant="body2" color="text.secondary">
                    {new Date(date).toLocaleTimeString()}
                </Typography>
                <Typography variant="h6" component="div">
                    {temperatureMin}°C - {temperatureMax}°C
                </Typography>
                <Box display="flex" alignItems="center">
                    {icon}
                    <Typography variant="body1" component="div" style={{ marginLeft: '0.5em' }}>
                        {description}
                    </Typography>
                </Box>
            </CardContent>
        </Card>
    );
}

export default WeatherCard;