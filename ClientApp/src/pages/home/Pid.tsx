import { useContext, useState, useEffect } from "react";
import { Container, Typography, Card, CardContent, makeStyles, createStyles, Theme, Switch, Slider, Box, Avatar, Collapse, Chip, Grid } from "@material-ui/core";
import { SignalrContext } from "src/infrastructure/SignalrContextProvider";
import { blue, deepOrange, green } from "@material-ui/core/colors";
import KeyboardArrowDownIcon from "@material-ui/icons/KeyboardArrowDown";
import KeyboardArrowUpIcon from "@material-ui/icons/KeyboardArrowUp";
import { HardwareStatus } from "./models";

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

export const Pid = () => {
  const signalr = useContext(SignalrContext);
  const classes = useStyles();

  const [showDebug, setShowDebug] = useState(false);
  const [hwStatus, setHWStatus] = useState<HardwareStatus>();

  useEffect(() => {
    let mounted = true;
    signalr.hubConnection?.on("HarwareStatus", (hws: HardwareStatus) => {
      if (mounted) {
        setHWStatus(hws);
      }
    });
    return () => {
      mounted = false;
    };
  }, [signalr.hubConnection]);

  const updatePidMode = () => {
    signalr.hubConnection?.invoke("UpdatePidMode", { FridgeMode: !hwStatus?.pid?.fridgeMode });
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
    const currentTemp = hwStatus?.pid?.currentTemp || 0;
    const targetTemp = hwStatus?.pid?.targetTemp || 0;
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
              key={`slider-${hwStatus?.pid?.targetTemp || 0}`}
              defaultValue={hwStatus?.pid?.targetTemp || 0}
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
                  {Math.round(((hwStatus?.pid?.currentTemp || 0) + Number.EPSILON) * 100) / 100}ºC
                </Box>
              </Avatar>
            </Grid>
            <Grid item xs={6}>
              <Avatar variant="square" className={classes.defaultAvatar}>
                <Box p={1}>
                  <Typography variant="h6" style={{ fontSize: "10px" }}>
                    Output Level
                  </Typography>
                  {Math.round(((hwStatus?.pid?.outputValue || 0) + Number.EPSILON) * 100) / 100}%
                </Box>
              </Avatar>
            </Grid>
            <Grid item xs={6}>
              <Avatar variant="square" className={classes.defaultAvatar}>
                <Box p={1}>
                  <Typography variant="h6" style={{ fontSize: "10px" }}>
                    Output
                  </Typography>
                  {hwStatus?.pid?.output ? "On" : "Off"}
                </Box>
              </Avatar>
            </Grid>
            {!(hwStatus?.pid?.fridgeMode || false) && (
              <Grid item xs={12}>
                <Avatar variant="square" className={classes.defaultAvatar}>
                  <Box p={1}>
                    <Typography variant="h6" style={{ fontSize: "10px" }}>
                      Error sum
                    </Typography>
                    {Math.round(((hwStatus?.pid?.errorSum || 0) + Number.EPSILON) * 100) / 100}
                  </Box>
                </Avatar>
              </Grid>
            )}
          </Grid>

          <br />
          <Switch checked={hwStatus?.pid?.fridgeMode || false} onChange={() => updatePidMode()} value={1} inputProps={{ "aria-label": "secondary checkbox" }} />
          <b>FridgeMode: {hwStatus?.pid?.fridgeMode || false ? "ON" : "OFF"}</b>

          <hr />
          <Typography>RPI-Core-Temp: {hwStatus?.pid?.rpiCoreTemp}</Typography>

          {(hwStatus?.pid?.fridgeMode || false) && (
            <div>
              <p>
                <b>Min: </b>
                {Math.round(((hwStatus?.pid?.minTemp || 0) + Number.EPSILON) * 100) / 100}ºC ({hwStatus?.pid?.minTempTimeStamp.toLocaleString()})
              </p>
              <p>
                <b>Max: </b>
                {Math.round(((hwStatus?.pid?.maxTemp || 0) + Number.EPSILON) * 100) / 100}ºC ({hwStatus?.pid?.maxTempTimeStamp.toLocaleString()})
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
        <Box margin={1}>{!(hwStatus?.pid?.fridgeMode || false) && <pre>{JSON.stringify(hwStatus?.pidConfig, null, 5)}</pre>}</Box>
        <Typography>
          Temp Target: {Math.round(((hwStatus?.pid?.targetTemp || 0) + Number.EPSILON) * 100) / 100}ºC. Actual: {Math.round(((hwStatus?.pid?.currentTemp || 0) + Number.EPSILON) * 100) / 100}ºC
        </Typography>
      </Collapse>
    </Container>
  );
};
