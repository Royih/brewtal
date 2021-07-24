import { useEffect, useState } from "react";
import { useContext } from "react";
import { useParams } from "react-router";
import { ResponsiveContainer, CartesianGrid, XAxis, YAxis, Legend, Line, Tooltip, ComposedChart } from "recharts";
import { SignalrContext } from "src/infrastructure/SignalrContextProvider";
import { SessionDto } from "./models";
import moment from "moment";

export const Sessions = () => {
  const signalr = useContext(SignalrContext);
  const [data, setData] = useState<SessionDto>();

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
        const newLogs = [...curr.logs, signalr.newTempLog];
        return { ...curr, logs: newLogs } as SessionDto;
      }
      return curr;
    });
  }, [signalr.newTempLog]);

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

  return (
    <div>
      {data && (
        <ResponsiveContainer width="100%" height={400}>
          <ComposedChart
            width={500}
            height={300}
            data={data.logs.map((x) => {
              x.timeStampAsTicks = +new Date(x.timeStamp);
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
          </ComposedChart>
        </ResponsiveContainer>
      )}
      <pre>{JSON.stringify(data, null, 5)}</pre>
    </div>
  );
};
