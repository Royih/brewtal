import React, { useContext, useState, useEffect } from "react";
import { Paper, ButtonGroup, Button, Card, CardHeader, CardContent, TextField, makeStyles, createStyles, Theme, Container, Box } from "@material-ui/core";
import { SignalrContext } from "src/infrastructure/SignalrContextProvider";

interface IMessage {
    timeStamp: Date;
    userName: string;
    message: string;
}

const useStyles = makeStyles((theme: Theme) =>
    createStyles({
        root: {
            display: "flex",
            flexWrap: "wrap",
        },
        margin: {
            margin: theme.spacing(1),
        },
        withoutLabel: {
            marginTop: theme.spacing(3),
        },
        textField: {
            width: 200,
        },
        formControl: {
            //   margin: theme.spacing(1),
            minWidth: 120,
        },
        selectEmpty: {
            marginTop: theme.spacing(2),
        },
    })
);

export const TestSignalR = () => {
    const classes = useStyles();
    const signalR = useContext(SignalrContext);
    const [message, setMessage] = useState("");
    const [messages, setMessages] = useState([] as IMessage[]);

    useEffect(() => {
        if (signalR.hubConnection) {
            signalR.hubConnection.on("SendMessage", (msg: IMessage) => {
                setMessages((oldArray) => [...oldArray, msg]);
                console.log("Her..", msg);
            });
        }
    }, [signalR.hubConnection]);

    return (
        <Container>
            <Paper elevation={3}>
                <Card>
                    <CardHeader title="Testing of exceptions"></CardHeader>
                    <CardContent>
                        <form action="" autoComplete="off">
                            <TextField
                                id="details-message"
                                label="Message"
                                multiline
                                value={message}
                                disabled={false}
                                required={true}
                                onChange={(event) => setMessage(event.target.value)}
                                className={classes.margin}
                            />
                            <ButtonGroup>
                                <Button
                                    variant="contained"
                                    color="secondary"
                                    disabled={!message.trim()}
                                    onClick={() => {
                                        signalR.invoke("SendMessage", { message: message });
                                        setMessage("");
                                    }}
                                >
                                    Posty
                                </Button>
                            </ButtonGroup>
                        </form>
                    </CardContent>
                </Card>
            </Paper>
            {messages &&
                messages.length > 0 &&
                messages.map((msg: IMessage, index: number) => (
                    <Box mt={3} key={index}>
                        <Paper elevation={3}>
                            <Card>
                                <CardContent>
                                    {msg.userName}: {msg.message} ({msg.timeStamp})
                                </CardContent>
                            </Card>
                        </Paper>
                    </Box>
                ))}
        </Container>
    );
};
