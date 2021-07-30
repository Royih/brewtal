import { useContext } from "react";
import { SignalrContext } from "src/infrastructure/SignalrContextProvider";
import { createStyles, Theme, makeStyles } from "@material-ui/core/styles";
import { Button, ButtonGroup } from "@material-ui/core";
import { red } from "@material-ui/core/colors";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    redCard: {
      color: theme.palette.getContrastText(red[800]),
      backgroundColor: red[800],
      "&:hover": {
        backgroundColor: red[700],
      },
    },
  })
);

export const ReconnectSignalr = () => {
  const signalR = useContext(SignalrContext);
  const classes = useStyles();

  return signalR.hubConnection?.state !== "Connected" ? (
    <ButtonGroup fullWidth aria-label="fullwidth button group" style={{ height: "60px" }} className={classes.redCard}>
      <Button variant="contained" color="secondary" size={"small"} onClick={signalR.reconnect}>
        <FontAwesomeIcon icon="unlink" fixedWidth size="lg" style={{ marginRight: "10px" }} />
        Forbindelsen er brutt. Klikk for å koble til på nytt
      </Button>
    </ButtonGroup>
  ) : null;
};
