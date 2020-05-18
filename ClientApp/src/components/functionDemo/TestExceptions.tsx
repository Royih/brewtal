import React, { useContext } from "react";
import { ApiContext } from "src/infrastructure/ApiContextProvider";
import { Paper, Box, ButtonGroup, Button, Typography } from "@material-ui/core";

export const ThrowExceptions = () => {
    const api = useContext(ApiContext);
    
    const throw403 = () => {
        api.get("sampleData/throw403");
    };
    const throw404 = () => {
        api.get("brulle/bjeff/xyz");
    };
    const throw500 = () => {
        api.get("sampleData/throw500");
    };

    return (
        <Paper elevation={3}>
            <Box mt={3} p={3}>
            <Typography variant="h3">Testing of exceptions</Typography>
            <ButtonGroup>
                <Button variant="contained" color="secondary" onClick={throw403}>Throw 403 - Forbidden</Button>
                <Button variant="contained" color="secondary" onClick={throw404}>Throw 404 - Not found</Button>
                <Button variant="contained" color="secondary" onClick={throw500}>Throw 500 - Server error</Button>
            </ButtonGroup>
            {/* <pre>{JSON.stringify(state)}</pre>
            <pre>Valid: {JSON.stringify(formState.valid)}</pre>
            <pre>Dirty: {JSON.stringify(formState.dirty)}</pre>
            <pre>Controls: {JSON.stringify(formState.controls.length)}</pre> */}
        </Box>
        </Paper>
    );
};
