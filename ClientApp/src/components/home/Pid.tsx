import React, { useContext, useState, useEffect } from "react";
import { Container, Typography, Card, CardContent, ButtonGroup, Button, CircularProgress, makeStyles, createStyles, Theme, Switch } from "@material-ui/core";
import SkipPrevIcon from "@material-ui/icons/SkipPrevious";
import SkipNextIcon from "@material-ui/icons/SkipNext";
import FastForwardIcon from "@material-ui/icons/FastForward";
import FastRewindIcon from "@material-ui/icons/FastRewind";
import FirstPageIcon from "@material-ui/icons/FirstPage";
import LastPageIcon from "@material-ui/icons/LastPage";
import { SignalrContext, PidStatus, PidConfig } from "src/infrastructure/SignalrContextProvider";

export type PidInput = {};

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
  const signalr = useContext(SignalrContext);

  const [pidStatus, setPidStatus] = useState<PidStatus>();
  const [pidConfig, setPidConfig] = useState<PidConfig | undefined>();
  const [newTarget, setNewTarget] = useState<number>();
  const [pendingChange, setPendingChange] = useState(false);

  const classes = useStyles();

  useEffect(() => {
    const newPidStatus = signalr.hwStatus?.pid;
    setPidStatus((currentValue) => {
      if (!currentValue) {
        setPendingChange(false);
        setNewTarget(newPidStatus?.targetTemp);
      }
      return newPidStatus;
    });
    setPidConfig(signalr.hwStatus?.pidConfig);
  }, [signalr.hwStatus]);

  useEffect(() => {
    const timer = setTimeout(() => {
      signalr.hubConnection?.invoke("UpdateTarget", { NewTargetTemp: newTarget });
      setPendingChange(false);
    }, 1300);
    return () => clearTimeout(timer);
  }, [newTarget, signalr.hubConnection]);

  const addNewTarget = (increment: number) => {
    setNewTarget((currentValue) => {
      const newVal = (currentValue || pidStatus?.targetTemp || 0) + increment;
      const newValAdjusted = newVal < 0 ? 0 : newVal > 100 ? 100 : newVal;
      setPendingChange(newValAdjusted !== currentValue);
      //console.log("test", newValAdjusted);
      return newValAdjusted;
    });
  };

  const updatePidMode = () => {
    signalr.hubConnection?.invoke("UpdatePidMode", { FridgeMode: !pidStatus?.fridgeMode });
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
            Output Level: {Math.round(((pidStatus?.outputValue || 0) + Number.EPSILON) * 100) / 100}% Output: {pidStatus?.output ? "On" : "Off"}
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

          <br />
          <Switch checked={pidStatus?.fridgeMode || false} onChange={() => updatePidMode()} value={1} inputProps={{ "aria-label": "secondary checkbox" }} />
          <b>FridgeMode: {pidStatus?.fridgeMode || false ? "ON" : "OFF"}</b>
          {!(pidStatus?.fridgeMode || false) && <pre>{JSON.stringify(pidConfig, null, 5)}</pre>}
          {(pidStatus?.fridgeMode || false) && (
            <div>
              <p>
                <b>Min: </b>
                {Math.round(((pidStatus?.minTemp || 0) + Number.EPSILON) * 100) / 100}ºC ({pidStatus?.minTempTimeStamp.toLocaleString()})
              </p>
              <p>
                <b>Max: </b>
                {Math.round(((pidStatus?.maxTemp || 0) + Number.EPSILON) * 100) / 100}ºC ({pidStatus?.maxTempTimeStamp.toLocaleString()})
              </p>
            </div>
          )}
        </CardContent>
      </Card>
    </Container>
  );
};
