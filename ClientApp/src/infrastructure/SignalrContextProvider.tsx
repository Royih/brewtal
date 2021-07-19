import { HubConnection, HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import React, { useState, useEffect } from "react";

export interface ManualOutput {
  output: number;
  name: string;
  value: boolean;
  automatic: boolean;
}

export interface PidStatus {
  pidId: number;
  pidName: string;
  targetTemp: number;
  currentTemp: number;
  output: boolean;
  outputValue: number;
  fridgeMode: boolean;
  minTemp: number;
  maxTemp: number;
  minTempTimeStamp: Date;
  maxTempTimeStamp: Date;
  errorSum: number;
  rpiCoreTemp: string;
}
export interface PidConfig {
  pIDKp: number;
  pIDKi: number;
  pIDKd: number;
}

export type HardwareStatus = {
  pid: PidStatus;
  computedTime: Date;
  manualOutputs: ManualOutput[];
  pidConfig: PidConfig;
};

export type SignalRState = {
  beat: boolean;
  status: SignalRStatus;
  hwStatus: HardwareStatus | undefined;
  reconnect(): void;
  hubConnection: HubConnection | undefined;
  connectedSince: Date | null;
  disconnectedSince: Date | null;
};
export enum SignalRStatus {
  Pending = "Pending",
  Connected = "Connected",
  Error = "Error",
  Ok = "Ok",
  Reconnecting = "Reconnecting",
}
const SignalRComHubPath = process.env.REACT_APP_API_PATH + "/comhub";
const SignalrContext = React.createContext({} as SignalRState);
export { SignalrContext }; // Export it so it can be used by other Components

export const SignalrContextProvider = (props: any) => {
  const [status, setStatus] = useState<SignalRStatus>(SignalRStatus.Pending);
  const [hwStatus, setHWStatus] = useState<HardwareStatus | undefined>();
  const [beat, setBeat] = useState(false);
  const [hubConnection, setHubConnection] = useState<HubConnection>();
  const [connectedSince, setConnectedSince] = useState<Date | null>(null);
  const [disconnectedSince, setDisconnectedSince] = useState<Date | null>(new Date());
  const [reconnecto, setReconnecto] = useState(false);

  const reconnect = () => {
    console.log("Reconnecting...");
    setStatus(SignalRStatus.Reconnecting);
    setReconnecto((currentState) => {
      return !currentState;
    });
  };

  useEffect(() => {
    const doStart = async () => {
      return await createHubConnection();
    };

    // Set the initial SignalR Hub Connection.
    const createHubConnection = async () => {
      // Trying to auto-reconnect SignalR trying n-times with increasing intervals
      const reconnectIntervals: number[] = [2000, 5000, 10000, 12000, 15000, 20000, 25000, 30000, 45000];
      //const reconnectIntervals: number[] = [2000, 5000];

      // Build new Hub Connection, url is currently hard coded.
      const hubConnection = new HubConnectionBuilder().withUrl(SignalRComHubPath).withAutomaticReconnect(reconnectIntervals).configureLogging(LogLevel.Information).build();

      setHubConnection(hubConnection);

      hubConnection.on("HarwareStatus", (hws: HardwareStatus) => {
        setHWStatus(hws);
        setBeat((curr) => {
          return !curr;
        });
      });

      hubConnection.onclose(() => {
        setStatus(SignalRStatus.Error);
        setConnectedSince(null);
        setDisconnectedSince(new Date());
        setBeat(false);
      });

      hubConnection.onreconnecting(() => {
        setStatus(SignalRStatus.Reconnecting);
        setConnectedSince(null);
        setDisconnectedSince(new Date());
        setBeat(false);
      });

      hubConnection.onreconnected(() => {
        setStatus(SignalRStatus.Connected);
        setConnectedSince(new Date());
        setDisconnectedSince(null);
        setBeat(false);
      });

      try {
        await hubConnection.start();
        console.debug("Connection successful!");
        setStatus(SignalRStatus.Connected);
        setConnectedSince(new Date());
        setDisconnectedSince(null);
      } catch (err) {
        console.error("Signalr error: ", err);
        setStatus(SignalRStatus.Error);
      }

      return () => {
        console.log("Cleanup Signalr");
        if (hubConnection) {
          hubConnection.off("Heartbeat");
          hubConnection.off("DeviceChanged");
          hubConnection.stop();
        }
      };
    };

    doStart();
  }, [reconnecto]);

  return (
    <SignalrContext.Provider
      value={{
        beat: beat,
        status: status,
        hwStatus: hwStatus,
        reconnect: reconnect,
        hubConnection: hubConnection,
        connectedSince: connectedSince,
        disconnectedSince: disconnectedSince,
      }}
    >
      {props.children}
    </SignalrContext.Provider>
  );
};
