import React, { useContext, useState, useEffect } from "react";
import { Container, Typography, Card, CardContent, ButtonGroup, Button, CircularProgress, makeStyles, createStyles, Theme } from "@material-ui/core";
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

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    root: {
      flexGrow: 1,
    },
    button: {
      paddingLeft: 0,
      paddingRight: 0,
    },
    wrapper: {
      margin: theme.spacing(0.5),
      position: "relative",
    },
    buttonProgress: {
      position: "absolute",
      top: "50%",
      left: "50%",
      marginTop: -10,
      marginLeft: -10,
    },
    buttonWrapper: {
      margin: theme.spacing(1),
      position: "relative",
    },
  })
);

export const Pid = (props: PidInput) => {
  const id = props.id;
  const hubConnection = useContext(SignalrHubContext);
  const signalr = useContext(SignalrContext);

  const [pidStatus, setPidStatus] = useState<PidStatus>();
  const [newTarget, setNewTarget] = useState<number>();
  const [pendingChange, setPendingChange] = useState(false);

  const classes = useStyles();

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
    }, 1300);
    return () => clearTimeout(timer);
  }, [newTarget, id, hubConnection]);

  const addNewTarget = (increment: number) => {
    setNewTarget((currentValue) => {
      const newVal = (currentValue || pidStatus?.targetTemp || 0) + increment;
      const newValAdjusted = newVal < 0 ? 0 : newVal > 100 ? 100 : newVal;
      setPendingChange(newValAdjusted !== currentValue);
      console.log("test", newValAdjusted);
      return newValAdjusted;
    });
  };

  return (
    <Container>
      <Card elevation={3}>
        <CardContent>
          <Typography variant="h4">{pidStatus?.pidName}</Typography>
          <Typography>
            Temp Target: {Math.round(((pidStatus?.targetTemp || 0) + Number.EPSILON) * 100) / 100}ºC. Actual: {Math.round(((pidStatus?.currentTemp || 0) + Number.EPSILON) * 100) / 100}ºC
          </Typography>
          <Typography>
            Output Level: {Math.round(((pidStatus?.outputValue || 0) + Number.EPSILON) * 100) / 100}% Pin: {pidStatus?.output ? "Yes" : "No"}
          </Typography>

          <ButtonGroup size="small" variant="contained" color="primary" aria-label="contained primary button group">
            <Button size="small" onClick={() => addNewTarget(-5)} className={classes.button} disabled={newTarget !== undefined && newTarget <= 0}>
              <FirstPageIcon />
            </Button>
            <Button onClick={() => addNewTarget(-1)} className={classes.button} disabled={newTarget !== undefined && newTarget <= 0}>
              <SkipPrevIcon />
            </Button>
            <Button onClick={() => addNewTarget(-0.1)} className={classes.button} disabled={newTarget !== undefined && newTarget <= 0}>
              <FastRewindIcon />
            </Button>

            <Button color="default">
              {Math.round(((newTarget || 0) + Number.EPSILON) * 100) / 100}ºC
              {pendingChange ? <CircularProgress color="primary" className={classes.buttonProgress} size={20}></CircularProgress> : ""}
            </Button>

            <Button onClick={() => addNewTarget(0.1)} className={classes.button} disabled={newTarget !== undefined && newTarget >= 100}>
              <FastForwardIcon />
            </Button>
            <Button onClick={() => addNewTarget(1)} className={classes.button} disabled={newTarget !== undefined && newTarget >= 100}>
              <SkipNextIcon />
            </Button>
            <Button onClick={() => addNewTarget(5)} className={classes.button} disabled={newTarget !== undefined && newTarget >= 100}>
              <LastPageIcon />
            </Button>
          </ButtonGroup>
        </CardContent>
      </Card>
    </Container>
  );
};
