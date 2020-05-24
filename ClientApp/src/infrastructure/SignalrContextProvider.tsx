import React, { useState, useEffect, useContext } from "react";
import { SignalrHubContext } from "./SignalrHubContextProvider";

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
};
export enum SignalRStatus {
    Pending = "Pending",
    Connected = "Connected",
    Error = "Error",
    Ok = "Ok",
    Reconnecting = "Reconnecting",
}
const SignalrContext = React.createContext({} as SignalRState);
export { SignalrContext }; // Export it so it can be used by other Components

export const SignalrContextProvider = (props: any) => {
    const hubConnection = useContext(SignalrHubContext);

    const [status, setStatus] = useState<SignalRStatus>(SignalRStatus.Pending);
    const [hwStatus, setHWStatus] = useState<HardwareStatus>();

    useEffect(() => {
        console.log("Hubconnection state:", hubConnection?.state);

        if (hubConnection) {
            hubConnection.on("HarwareStatus", (hws: HardwareStatus) => {
                setHWStatus(hws);
            });
            hubConnection.onclose(() => {
                setStatus(SignalRStatus.Reconnecting);
            });
            setStatus(SignalRStatus.Connected);
        }
    }, [hubConnection]);

    return <SignalrContext.Provider value={{ status: status, hwStatus: hwStatus}}>{props.children}</SignalrContext.Provider>;
};
