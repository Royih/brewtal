import React, { useContext, useState, useEffect } from "react";
import { Container, Typography, Card, CardContent, ButtonGroup, Button, CircularProgress } from "@material-ui/core";
import SkipPrevIcon from "@material-ui/icons/SkipPrevious";
import SkipNextIcon from "@material-ui/icons/SkipNext";
import FastForwardIcon from "@material-ui/icons/FastForward";
import FastRewindIcon from "@material-ui/icons/FastRewind";
import FirstPageIcon from "@material-ui/icons/FirstPage";
import LastPageIcon from "@material-ui/icons/LastPage";
import { SignalrContext, PidStatus } from "src/infrastructure/SignalrContextProvider";
import { SignalrHubContext } from "src/infrastructure/SignalrHubContextProvider";

export type PidInput = {
    id: number;
};

export const Pid = (props: PidInput) => {
    const id = props.id;
    const hubConnection = useContext(SignalrHubContext);
    const signalr = useContext(SignalrContext);

    const [pidStatus, setPidStatus] = useState<PidStatus>();
    const [newTarget, setNewTarget] = useState<number>();
    const [pendingChange, setPendingChange] = useState(true);

    useEffect(() => {
        const newPidStatus = signalr.hwStatus?.pids[id];
        setPidStatus((currentValue) => {
            if (!currentValue) {
                setPendingChange(false);
                setNewTarget(newPidStatus?.targetTemp);
            }
            return newPidStatus;
        });
    }, [signalr, id]);

    useEffect(() => {
        const timer = setTimeout(() => {
            hubConnection?.invoke("UpdateTarget", { PidId: id, NewTargetTemp: newTarget });
            setPendingChange(false);
        }, 3000);
        setPendingChange(true);
        return () => clearTimeout(timer);
    }, [newTarget, id, hubConnection]);

    const addNewTarget = (increment: number) => {
        setNewTarget((currentValue) => {
            const newVal = (currentValue || pidStatus?.targetTemp || 0) + increment;
            return newVal >= 0 && newVal <= 100 ? newVal : currentValue;
        });
    };

    return (
        <Container>
            <Card elevation={3}>
                <CardContent>
                    <Typography>Name: {pidStatus?.pidName}</Typography>
                    <Typography>Actual temp: {Math.round(((pidStatus?.currentTemp || 0) + Number.EPSILON) * 100) / 100}ºC</Typography>
                    <Typography>Target temp: {Math.round(((pidStatus?.targetTemp || 0) + Number.EPSILON) * 100) / 100}ºC</Typography>
                    <Typography>Output level: {Math.round(((pidStatus?.outputValue || 0) + Number.EPSILON) * 100) / 100}%</Typography>
                    <Typography>Output pin: {pidStatus?.output ? "Yes" : "No"}</Typography>

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
                            {Math.round(((newTarget || pidStatus?.targetTemp || 0) + Number.EPSILON) * 100) / 100}ºC
                            {pendingChange ? <CircularProgress color="primary" size={20}></CircularProgress> : ""}
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
