import React, { useState } from "react";
import { Button, Typography, Paper, Box } from "@material-ui/core";

function CounterAsFunction() {
    // Declare   new state variable, which we'll call "count"
    const [count, setCount] = useState(0);

    return (
        <div>
            <Typography>You clicked {count} times</Typography>
            <Button variant="contained" color="primary" onClick={() => setCount(count + 1)}>
                Click me
            </Button>
        </div>
    );
}

export const Counter = () => {
    return (
        <Paper elevation={3}>
            <Box mt={3} p={3}>
                <CounterAsFunction />
                <CounterAsFunction />
                <CounterAsFunction />
            </Box>
        </Paper>
    );
};
