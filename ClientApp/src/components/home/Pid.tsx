import React, { useContext, useState, useEffect } from "react";
import { Container, Typography, Card, CardContent, ButtonGroup, Button, CircularProgress } from "@material-ui/core";
import SkipPrevIcon from "@material-ui/icons/SkipPrevious";
import SkipNextIcon from "@material-ui/icons/SkipNext";
import FastForwardIcon from "@material-ui/icons/FastForward";
import FastRewindIcon from "@material-ui/icons/FastRewind";
import FirstPageIcon from "@material-ui/icons/FirstPage";
import LastPageIcon from "@material-ui/icons/LastPage";
import { SignalrContext, PidStatus } from "src/infrastructure/SignalrContextProvider";

export type PidInput = {
    id: number;
};

export const Pid = (props: PidInput) => {
    const signalr = useContext(SignalrContext);

    const [pid, setPid] = useState<PidStatus>();
    const [newTarget, setNewTarget] = useState<number>();
    const [pendingChange, setPendingChange] = useState(false);
    const [ready, setReady] = useState(false);

    useEffect(() => {
        setPid(signalr.hwStatus?.pids[props.id]);
    }, [signalr.hwStatus]);

    useEffect(() => {
        setReady(true);
    }, []);

    useEffect(() => {
        if (ready) {
            const timer = setTimeout(() => {
                signalr.invoke("UpdateTarget", { PidId: props.id, NewTargetTemp: newTarget });
                setPendingChange(false);
            }, 3000);
            setPendingChange(true);
            return () => clearTimeout(timer);
        }
    }, [newTarget]);

    const addNewTarget = (increment: number) => {
        let newValue = (newTarget || pid?.targetTemp || 0) + increment;
        if (newValue < 0) {
            newValue = 0;
        }
        if (newValue > 100) {
            newValue = 100;
        }
        setNewTarget(newValue);
    };

    return (
        <Container>
            <Card elevation={3}>
                <CardContent>
                    <Typography>Name: {pid?.pidName}</Typography>
                    <Typography>Actual temp: {Math.round(((pid?.currentTemp || 0) + Number.EPSILON) * 100) / 100}ºC</Typography>
                    <Typography>Target temp: {Math.round(((pid?.targetTemp || 0) + Number.EPSILON) * 100) / 100}ºC</Typography>
                    <Typography>Output level: {Math.round(((pid?.outputValue || 0) + Number.EPSILON) * 100) / 100}%</Typography>
                    <Typography>Output pin: {pid?.output ? "Yes" : "No"}</Typography>

                    <ButtonGroup variant="contained" color="primary" aria-label="contained primary button group">
                        <Button onClick={() => addNewTarget(-5)}>
                            <FirstPageIcon />
                        </Button>
                        <Button onClick={() => addNewTarget(-1)}>
                            <SkipPrevIcon />
                        </Button>
                        <Button onClick={() => addNewTarget(-0.1)}>
                            <FastRewindIcon />
                        </Button>
                        <Button color="default">
                            {Math.round(((newTarget || pid?.targetTemp || 0) + Number.EPSILON) * 100) / 100}ºC
                            {pendingChange ? <CircularProgress color='primary' size={20}></CircularProgress> : ""}
                        </Button>
                        <Button onClick={() => addNewTarget(0.1)}>
                            <FastForwardIcon />
                        </Button>
                        <Button onClick={() => addNewTarget(1)}>
                            <SkipNextIcon />
                        </Button>
                        <Button onClick={() => addNewTarget(5)}>
                            <LastPageIcon />
                        </Button>
                    </ButtonGroup>
                </CardContent>
            </Card>
        </Container>
    );
};
