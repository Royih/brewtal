import React, { useContext, useState, useEffect } from "react";
import { Container, Typography, Card, CardContent, makeStyles, createStyles, Theme, Switch, Slider, Box, Avatar, Collapse, Chip, Grid } from "@material-ui/core";
import { SignalrContext, PidStatus, PidConfig } from "src/infrastructure/SignalrContextProvider";
import { blue, deepOrange, green } from "@material-ui/core/colors";
import KeyboardArrowDownIcon from "@material-ui/icons/KeyboardArrowDown";
import KeyboardArrowUpIcon from "@material-ui/icons/KeyboardArrowUp";

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    root: {
      display: "flex",
      "& > *": {
        margin: theme.spacing(1),
      },
    },
    orange: {
      color: theme.palette.getContrastText(deepOrange[500]),
      backgroundColor: deepOrange[500],
      fontSize: "50px",
      width: "auto",
      height: "auto",
    },
    green: {
      color: theme.palette.getContrastText(green[500]),
      backgroundColor: green[500],
      fontSize: "50px",
      width: "auto",
      height: "auto",
    },
    blue: {
      color: theme.palette.getContrastText(blue[500]),
      backgroundColor: blue[500],
      fontSize: "50px",
      width: "auto",
      height: "auto",
    },
    defaultAvatar: {
      width: "auto",
      height: "auto",
    },
  })
);

export const Pid2 = () => {
  const signalr = useContext(SignalrContext);
  const classes = useStyles();

  const [pidStatus, setPidStatus] = useState<PidStatus>();
  const [pidConfig, setPidConfig] = useState<PidConfig | undefined>();
  const [showDebug, setShowDebug] = useState(false);

  useEffect(() => {
    setPidStatus(signalr.hwStatus?.pid);
    setPidConfig(signalr.hwStatus?.pidConfig);
  }, [signalr.hwStatus]);

  const updatePidMode = () => {
    signalr.hubConnection?.invoke("UpdatePidMode", { FridgeMode: !pidStatus?.fridgeMode });
  };

  function valuetext(value: number) {
    return `${value}°C`;
  }

  const updateValue = (event: object, value: number | number[]): void => {
    if (signalr.hubConnection && signalr.hubConnection.state === "Connected") {
      signalr.hubConnection?.invoke("UpdateTarget", { NewTargetTemp: value });
    }
  };

  const getClassName = (): string => {
    const currentTemp = pidStatus?.currentTemp || 0;
    const targetTemp = pidStatus?.targetTemp || 0;
    if (currentTemp - targetTemp > 1) {
      return classes.orange;
    }
    if (currentTemp - targetTemp < -1) {
      return classes.blue;
    }
    return classes.green;
  };

  return (
    <Container>
      <Card elevation={3}>
        <CardContent>
          <Typography id="discrete-slider" gutterBottom>
            Select desired temperature
          </Typography>
          <Box mt={5}>
            <Slider
              key={`slider-${pidStatus?.targetTemp || 0}`}
              defaultValue={pidStatus?.targetTemp || 0}
              getAriaValueText={valuetext}
              aria-labelledby="discrete-slider"
              valueLabelDisplay="on"
              step={1}
              marks
              min={0}
              max={100}
              onChangeCommitted={updateValue}
            />
          </Box>

          <Grid container spacing={0}>
            <Grid item xs={12}>
              <Avatar variant="square" className={getClassName()}>
                <Box p={1}>
                  <Typography variant="h6" style={{ fontSize: "10px" }}>
                    Temp
                  </Typography>
                  {Math.round(((pidStatus?.currentTemp || 0) + Number.EPSILON) * 100) / 100}ºC
                </Box>
              </Avatar>
            </Grid>
            <Grid item xs={6}>
              <Avatar variant="square" className={classes.defaultAvatar}>
                <Box p={1}>
                  <Typography variant="h6" style={{ fontSize: "10px" }}>
                    Output Level
                  </Typography>
                  {Math.round(((pidStatus?.outputValue || 0) + Number.EPSILON) * 100) / 100}%
                </Box>
              </Avatar>
            </Grid>
            <Grid item xs={6}>
              <Avatar variant="square" className={classes.defaultAvatar}>
                <Box p={1}>
                  <Typography variant="h6" style={{ fontSize: "10px" }}>
                    Output
                  </Typography>
                  {pidStatus?.output ? "On" : "Off"}
                </Box>
              </Avatar>
            </Grid>
            {!(pidStatus?.fridgeMode || false) && (
              <Grid item xs={12}>
                <Avatar variant="square" className={classes.defaultAvatar}>
                  <Box p={1}>
                    <Typography variant="h6" style={{ fontSize: "10px" }}>
                      Error sum
                    </Typography>
                    {Math.round(((pidStatus?.errorSum || 0) + Number.EPSILON) * 100) / 100}
                  </Box>
                </Avatar>
              </Grid>
            )}
          </Grid>

          <br />
          <Switch checked={pidStatus?.fridgeMode || false} onChange={() => updatePidMode()} value={1} inputProps={{ "aria-label": "secondary checkbox" }} />
          <b>FridgeMode: {pidStatus?.fridgeMode || false ? "ON" : "OFF"}</b>

          <hr />
          <Typography>RPI-Core-Temp: {pidStatus?.rpiCoreTemp}</Typography>

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
      <hr />
      <Box mt={3}>
        <Chip
          icon={showDebug ? <KeyboardArrowUpIcon /> : <KeyboardArrowDownIcon />}
          label="Show debug info"
          clickable
          color="default"
          onClick={() =>
            setShowDebug((curr) => {
              return !curr;
            })
          }
        />
      </Box>
      <Collapse in={showDebug} timeout="auto" unmountOnExit>
        <Box margin={1}>{!(pidStatus?.fridgeMode || false) && <pre>{JSON.stringify(pidConfig, null, 5)}</pre>}</Box>
        <Typography>
          Temp Target: {Math.round(((pidStatus?.targetTemp || 0) + Number.EPSILON) * 100) / 100}ºC. Actual: {Math.round(((pidStatus?.currentTemp || 0) + Number.EPSILON) * 100) / 100}ºC
        </Typography>
      </Collapse>
    </Container>
  );
};
