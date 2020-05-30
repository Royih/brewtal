import React, { useState, useEffect } from "react";
import { useHistory } from "react-router-dom";
import { Loading } from "../common/Loading";
import { UserContext } from "src/infrastructure/UserContextProvider";
import { ApiContext } from "src/infrastructure/ApiContextProvider";
import { ButtonGroup, Table, TableBody, TableHead, TableRow, TableCell, Paper, Box, Button, Typography, TableContainer } from "@material-ui/core";
import AddIcon from "@material-ui/icons/Add";

import { Brew } from "./Models";

export const Brews = () => {
    const [brews, setBrews] = useState<Brew[] | undefined>();
    const [loading, setLoading] = useState<boolean>(true);
    const userContext = React.useContext(UserContext);
    const api = React.useContext(ApiContext);
    const history = useHistory();

    const Contents = () => {
        if (loading) {
            return <Loading />;
        }
        return <Render />;
    };

    const Render = () => {
        if (brews) {
            const createNew = async () => {
                history.push("/brew/create");
            };
            return (
                <Box mt={3}>
                    <ButtonGroup variant="contained">
                        <Button color="primary" onClick={createNew} startIcon={<AddIcon />}>
                            Register new brew
                        </Button>
                    </ButtonGroup>
                    <Box mt={3}>
                        <Paper elevation={3}>
                            <TableContainer>
                                <Table>
                                    <TableHead>
                                        <TableRow>
                                            <TableCell>Batch #</TableCell>
                                            <TableCell>Name</TableCell>
                                            <TableCell>Brew date</TableCell>
                                            <TableCell>Batch size</TableCell>
                                            <TableCell>Current step</TableCell>
                                        </TableRow>
                                    </TableHead>
                                    <TableBody>
                                        {brews.map((brew: Brew) => (
                                            <TableRow key={brew.id} hover onClick={() => history.push("/brews/" + brew.id)}>
                                                <TableCell>{brew.batchNumber}</TableCell>
                                                <TableCell>{brew.name}</TableCell>
                                                <TableCell>{brew.beginMash}</TableCell>
                                                <TableCell>{brew.batchSize}l</TableCell>
                                                <TableCell>Todo</TableCell>
                                            </TableRow>
                                        ))}
                                    </TableBody>
                                </Table>
                            </TableContainer>
                        </Paper>
                    </Box>
                </Box>
            );
        }
        return null;
    };

    useEffect(() => {
        const fetchData = async () => {
            setLoading(true);
            const data = await api.get<Brew[]>("brews");
            setBrews(data);
            setLoading(false);
        };

        async function load() {
            await fetchData();
        }
        load();
    }, [api]);

    return (
        <div>
            <Typography variant="h3" gutterBottom>
                Brews
            </Typography>
            <Contents />
        </div>
    );
    // }
};
