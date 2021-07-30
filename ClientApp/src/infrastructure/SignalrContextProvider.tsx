import { HubConnection, HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import React, { useState, useEffect } from "react";

export type SignalRState = {
  reconnect(): void;
  hubConnection: HubConnection | undefined;
  connectedSince: Date | null;
  disconnectedSince: Date | null;
};

const SignalRComHubPath = process.env.REACT_APP_API_PATH + "/comhub";
const SignalrContext = React.createContext({} as SignalRState);
export { SignalrContext }; // Export it so it can be used by other Components

export const SignalrContextProvider = (props: any) => {
  const [hubConnection, setHubConnection] = useState<HubConnection>();
  const [connectedSince, setConnectedSince] = useState<Date | null>(null);
  const [disconnectedSince, setDisconnectedSince] = useState<Date | null>(new Date());
  const [reconnecto, setReconnecto] = useState(false);

  const reconnect = () => {
    console.log("Reconnecting...");

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
      const reconnectIntervals: number[] = [2000, 5000, 5000, 5000, 5000, 5000, 10000, 12000, 15000, 20000, 25000, 30000, 45000];

      // Build new Hub Connection, url is currently hard coded.
      const hubConnection = new HubConnectionBuilder().withUrl(SignalRComHubPath).withAutomaticReconnect(reconnectIntervals).configureLogging(LogLevel.Information).build();

      setHubConnection(hubConnection);

      hubConnection.onclose(() => {
        setConnectedSince(null);
        setDisconnectedSince(new Date());
      });

      hubConnection.onreconnecting(() => {
        setConnectedSince(null);
        setDisconnectedSince(new Date());
      });

      hubConnection.onreconnected(() => {
        setConnectedSince(new Date());
        setDisconnectedSince(null);
      });

      try {
        await hubConnection.start();
        console.debug("Connection successful!");
        setConnectedSince(new Date());
        setDisconnectedSince(null);
      } catch (err) {
        console.error("Signalr error: ", err);
      }

      return () => {
        console.log("Cleanup Signalr");
        if (hubConnection) {
          hubConnection.stop();
        }
      };
    };

    doStart();
  }, [reconnecto]);

  return (
    <SignalrContext.Provider
      value={{
        reconnect: reconnect,
        hubConnection: hubConnection,
        connectedSince: connectedSince,
        disconnectedSince: disconnectedSince,
      }}
    >
      {hubConnection && props.children}
    </SignalrContext.Provider>
  );
};
