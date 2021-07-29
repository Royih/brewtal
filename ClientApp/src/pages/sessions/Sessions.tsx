import { useEffect, useState } from "react";
import { useContext } from "react";
import { useHistory, useParams } from "react-router";
import { ResponsiveContainer, CartesianGrid, XAxis, YAxis, Legend, Line, Tooltip, ComposedChart } from "recharts";
import { SignalrContext } from "src/infrastructure/SignalrContextProvider";
import { SessionDto, TemplogDto } from "./models";
import moment from "moment";
import { createStyles, FormControl, Grid, InputLabel, makeStyles, MenuItem, Select, TextField, Theme } from "@material-ui/core";
import Moment from "react-moment";
import React from "react";
import DisplayTempLogs from "./DisplayTempLogs";

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    formControl: {
      margin: theme.spacing(1),
      minWidth: 120,
    },
    selectEmpty: {
      marginTop: theme.spacing(2),
    },
  })
);

export const Sessions = () => {
  const classes = useStyles();

  const signalr = useContext(SignalrContext);
  const [data, setData] = useState<SessionDto>();
  const [gridData, setGridData] = useState<TemplogDto[]>();

  const history = useHistory();

  let { id } = useParams<{ id?: string }>();

  useEffect(() => {
    const getData = async () => {
      var myData = await signalr.hubConnection?.invoke<SessionDto>("GetSession", { SessionId: id ? +id : 0 });
      setData(myData);
    };
    if (signalr.hubConnection?.state === "Connected") {
      getData();
    }
  }, [signalr.hubConnection, signalr.hubConnection?.state, id]);

  useEffect(() => {
    setData((curr) => {
      if (signalr.newTempLog && curr && curr.id === signalr.newTempLog.sessionId) {
        const newLogs = [signalr.newTempLog, ...curr.logs];
        return { ...curr, logs: newLogs } as SessionDto;
      }
      return curr;
    });
  }, [signalr.newTempLog]);

  useEffect(() => {
    setGridData((curr) => {
      return data?.logs || curr;
    });
  }, [data]);

  const formatXAxis = (tickItem: Date) => {
    return moment(tickItem).format("HH:mm");
  };

  const interval = 1000 * 60; //1 seconds of ticks

  const getMinMax = (): [number, number] => {
    if (data) {
      const values = data.logs.map((x) => +new Date(x.timeStamp));
      if (values) {
        const min = Math.min(...values);
        const minRounded = Math.ceil(min / interval) * interval;
        return [minRounded, Math.max(...values)];
      }
    }
    return [0, 0];
  };
  const getTicks = (): number[] => {
    const minMax = getMinMax();
    let val = minMax[0];
    const result = [val];
    while (val < minMax[1]) {
      result.push((val += interval));
    }
    return result;
  };

  const handleChange = (event: React.ChangeEvent<{ value: unknown }>) => {
    history.push(`/sessions/${event.target.value}`);
  };

  return (
    <div>
      {data && (
        <div>
          <FormControl className={classes.formControl}>
            <InputLabel id="demo-simple-select-label">Session:</InputLabel>
            <Select labelId="demo-simple-select-label" id="demo-simple-select" value={id || data.allSessions[0].id} onChange={handleChange}>
              {data.allSessions &&
                data.allSessions.map((x) => (
                  <MenuItem key={x.id} value={x.id}>
                    Start time:&nbsp;<Moment>{x.startTime}</Moment>&nbsp; (<Moment fromNow>{x.startTime}</Moment>) Target-temp: {x.targetTemp}
                  </MenuItem>
                ))}
            </Select>
          </FormControl>

          <Grid container spacing={3}>
            <Grid item xs={2}>
              <TextField label="Target-temp" disabled={true} defaultValue={data.targetTemp + "°C"} className={classes.formControl} />
            </Grid>
            <Grid item xs={2}>
              <TextField label="Min-temp" disabled={true} defaultValue={(Math.round(data.minTemp * 100) / 100).toFixed(2) + "°C"} className={classes.formControl} />
            </Grid>
            <Grid item xs={2}>
              <TextField label="Max-temp" disabled={true} defaultValue={(Math.round(data.maxTemp * 100) / 100).toFixed(2) + "°C"} className={classes.formControl} />
            </Grid>
            <Grid item xs={2}>
              <TextField label="# of Logs" disabled={true} value={data.logs.length} className={classes.formControl} />
            </Grid>
          </Grid>

          <ResponsiveContainer width="100%" height={400}>
            <ComposedChart
              width={500}
              height={300}
              data={data.logs.map((x) => {
                x.timeStampAsTicks = +new Date(x.timeStamp);
                x.targetTemp = data.targetTemp;
                return x;
              })}
              margin={{
                top: 5,
                right: 30,
                left: 20,
                bottom: 5,
              }}
            >
              <CartesianGrid strokeDasharray="3 3" />
              <XAxis dataKey="timeStampAsTicks" tickFormatter={formatXAxis} type="number" domain={getMinMax()} ticks={getTicks()} />
              <YAxis label="°C" orientation="right" />
              <Tooltip
                labelStyle={{ color: "green" }}
                labelFormatter={function (value) {
                  return `${moment(new Date(value)).format("YYYY-MM-DD HH:mm:ss")}`;
                }}
              />
              <Legend />
              <Line type="monotone" dataKey="actualTemperature" stroke="#00FF00" activeDot={{ r: 8 }} />
              <Line type="monotone" dataKey="targetTemp" stroke="#FF0000" activeDot={{ r: 8 }} />
            </ComposedChart>
          </ResponsiveContainer>
        </div>
      )}
      {gridData && <DisplayTempLogs logs={gridData}></DisplayTempLogs>}

      {/* <pre>{JSON.stringify(data, null, 5)}</pre> */}
    </div>
  );
};
