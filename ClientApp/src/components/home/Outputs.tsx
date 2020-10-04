import React, { useContext, useState, useEffect } from "react";
import { Container, Card, CardContent, Switch } from "@material-ui/core";
import { SignalrContext, ManualOutput } from "src/infrastructure/SignalrContextProvider";
import { SignalrHubContext } from "src/infrastructure/SignalrHubContextProvider";

export type IOutputsProps = {};

export const Outputs = (props: IOutputsProps) => {
    const hubConnection = useContext(SignalrHubContext);
    const signalr = useContext(SignalrContext);
    const [manualOutputs, setManualOutputs] = useState<ManualOutput[]>();

    useEffect(() => {
        setManualOutputs(signalr.hwStatus?.manualOutputs);
    }, [signalr]);

    // useEffect(() => {
    //     const timer = setTimeout(() => {
    //         //hubConnection?.invoke("UpdateTarget", { PidId: id, NewTargetTemp: newTarget });
    //         setPendingChange(false);
    //     }, 1300);
    //     return () => clearTimeout(timer);
    // }, [newTarget, hubConnection]);

    const toggle = (output: ManualOutput) => {
        hubConnection?.invoke("SetOutput", { Output: output.output, Value: !output.value }).then((res) => {
            console.log("jer...");
        });
    };

    return (
        <Container>
            <Card elevation={3}>
                <CardContent>
                    {manualOutputs &&
                        manualOutputs.length > 0 &&
                        manualOutputs.map((output: ManualOutput) => (
                            <div key={output.name}>
                                <strong>{output.name}</strong> &nbsp; &nbsp;
                                <Switch checked={output.value} onChange={() => toggle(output)} value={output.value} inputProps={{ "aria-label": "secondary checkbox" }} />
                            </div>
                        ))}
                        {/* <div>{JSON.stringify(manualOutputs,null,2)}</div> */}
                </CardContent>
            </Card>
        </Container>
    );
};
