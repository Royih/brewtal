import React, { useState, useEffect } from "react";
import { ApiContext } from "src/infrastructure/ApiContextProvider";
import { Paper, Box, Typography, TableHead, TableCell, Table, TableRow, TableBody } from "@material-ui/core";
import { Loading } from "../common/Loading";

interface IForecast {
    dateFormatted: string;
    temperatureC: number;
    temperatureF: number;
    summary: string;
}

export const FetchData = () => {
    const [forecasts, setForecasts] = useState<IForecast[] | undefined>();
    const [loading, setLoading] = useState<boolean>(true);
    const api = React.useContext(ApiContext);

    useEffect(() => {
        const populateWeatherData = async () => {
            const data = await api.get<IForecast[]>("SampleData/WeatherForecasts");
            setForecasts(data);
            setLoading(false);
        };
        async function load() {
            await populateWeatherData();
        }
        load();
    }, [api]);

    return (
        <Paper elevation={3}>
            <Box mt={3} p={3}>
                <Typography variant="h3" gutterBottom>
                    Weather forecast
                </Typography>
                <p>This component demonstrates fetching data from the server.</p>
                {loading && <Loading></Loading>}
                {forecasts && (
                    <Table>
                        <TableHead>
                            <TableRow>
                                <TableCell>Date</TableCell>
                                <TableCell>Temp. (C)</TableCell>
                                <TableCell>Temp. (F)</TableCell>
                                <TableCell>Summary</TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {forecasts.map((forecast: IForecast) => (
                                <TableRow key={forecast.dateFormatted}>
                                    <TableCell>{forecast.dateFormatted}</TableCell>
                                    <TableCell>{forecast.temperatureC}</TableCell>
                                    <TableCell>{forecast.temperatureF}</TableCell>
                                    <TableCell>{forecast.summary}</TableCell>
                                </TableRow>
                            ))}
                        </TableBody>
                    </Table>
                )}
            </Box>
        </Paper>
    );
};
