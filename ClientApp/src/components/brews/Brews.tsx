import React, { useEffect } from "react";
import { Paper, Box } from "@material-ui/core";

export const Brews = (props: any) => {
    useEffect(() => {
        const fetchData = async () => {};
        fetchData();
    }, []);

    return (
        <Paper elevation={3}>
            <Box mt={3} p={3}>
                Hello world
            </Box>
        </Paper>
    );
};
