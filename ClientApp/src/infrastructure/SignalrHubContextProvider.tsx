import React, { useState, useEffect } from "react";
import { HubConnectionBuilder, HubConnection, LogLevel } from "@microsoft/signalr";

const SignalRComHubPath = process.env.REACT_APP_API_PATH + "/comhub";
const SignalrHubContext = React.createContext<HubConnection | undefined>(undefined);
export { SignalrHubContext }; // Export it so it can be used by other Components

export const SignalrHubContextProvider = (props: any) => {
    const [hubConnection, setHubConnection] = useState<HubConnection>();

    useEffect(() => {
        const doStart = async () => {
            return await createHubConnection();
        };

        // Set the initial SignalR Hub Connection.
        const createHubConnection = async () => {
            // Trying to auto-reconnect SignalR trying n-times with increasing intervals

            // Build new Hub Connection, url is currently hard coded.
            const myHubConnection = new HubConnectionBuilder()
                .withUrl(SignalRComHubPath)
                .withAutomaticReconnect([2000, 5000, 10000, 12000, 15000, 20000, 25000, 30000, 45000])
                .configureLogging(LogLevel.Information)
                .build();
            setHubConnection(myHubConnection);
            try {
                await myHubConnection.start();
                console.log("Signalr started.");
            } catch (err) {
                console.error("Signalr error: ", err);
            }
        };
        doStart();
    }, []);

    return <SignalrHubContext.Provider value={hubConnection}>{props.children}</SignalrHubContext.Provider>;
};
