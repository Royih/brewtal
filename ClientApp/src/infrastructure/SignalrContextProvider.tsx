import React, { useState, useEffect, useContext } from "react";

import { HubConnectionBuilder, HubConnection, LogLevel } from "@microsoft/signalr";
import { ApiContext } from "./ApiContextProvider";
import { UserContext } from "./UserContextProvider";

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
}

export type HardwareStatus = {
    pids: PidStatus[];
    computedTime: Date;
    manualOutputs: ManualOutput[];
};

export type SignalRState = {
    status: SignalRStatus;
    hwStatus?: HardwareStatus;
    hubConnection?: HubConnection;
    invoke<T>(methodName: string, args: any): Promise<T>;
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
    const currentUser = useContext(UserContext);
    const invoke = async (methodName: string, args: any) => {
        args.userName = currentUser.user.name;
        args.timeStamp = new Date();
        return hubConnection?.invoke(methodName, args);
    };
    const api = useContext(ApiContext);
    const [status, setStatus] = useState<SignalRStatus>(SignalRStatus.Pending);
    const [hubConnection, setHubConnection] = useState<HubConnection>();
    const [hwStatus, setHWStatus] = useState<HardwareStatus>();

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

            myHubConnection.on("HarwareStatus", (hws: HardwareStatus) => {
                setHWStatus(hws);
            });
            myHubConnection.onclose(() => {
                setStatus(SignalRStatus.Reconnecting);
            });
            setHubConnection(myHubConnection);
            try {
                await myHubConnection.start();
                setStatus(SignalRStatus.Connected);
            } catch (err) {
                console.error("Signalr error: ", err);
                setStatus(SignalRStatus.Reconnecting);
            }
        };
        doStart();
    }, [api]);

    return <SignalrContext.Provider value={{ status: status, hwStatus: hwStatus, hubConnection: hubConnection, invoke: invoke }}>{props.children}</SignalrContext.Provider>;
};
