import React, { useContext, useEffect } from "react";
import AppBar from "@material-ui/core/AppBar";
import { createStyles, Theme, makeStyles, useTheme } from "@material-ui/core/styles";
import Toolbar from "@material-ui/core/Toolbar";
import IconButton from "@material-ui/core/IconButton";
import { Box, Fab, Typography } from "@material-ui/core";
import { useLocation } from "react-router";
import DarkModeIcon from "@material-ui/icons/Brightness4";
import LightModeIcon from "@material-ui/icons/Brightness5";
import HomeIcon from "@material-ui/icons/Home";
import { ThemeContext } from "src/infrastructure/ThemeContextProvider";
import { NavLink } from "react-router-dom";
import { SignalrContext, SignalRStatus } from "src/infrastructure/SignalrContextProvider";
import { LeftMenu } from "../LeftMenu";
import { ReconnectSignalr } from "./ReconnectSignalr";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    text: {
      padding: theme.spacing(2, 2, 0),
    },
    paper: {
      paddingBottom: 50,
    },
    list: {
      marginBottom: theme.spacing(2),
    },
    subheader: {
      backgroundColor: theme.palette.background.paper,
    },
    appBar: {
      top: "auto",
      bottom: 0,
    },
    grow: {
      flexGrow: 1,
    },
    fabButton: {
      position: "absolute",
      zIndex: 1,
      top: -40,
      left: 0,
      right: 0,
      margin: "0 auto",
    },
    sectionDesktop: {
      display: "none",
      [theme.breakpoints.up("md")]: {
        display: "flex",
      },
    },
    sectionMobile: {
      display: "flex",
      [theme.breakpoints.up("md")]: {
        display: "none",
      },
    },
  })
);

export const Footer = () => {
  const classes = useStyles();
  const theme = useTheme();
  const themeContext = useContext(ThemeContext);
  const signalR = useContext(SignalrContext);

  useEffect(() => {
    const fetchData = async () => {};
    fetchData();
  }, []);

  let location = useLocation();

  const Displayheart = () => {
    if (signalR.status === SignalRStatus.Connected || signalR.status === SignalRStatus.Ok) {
      if (signalR.beat) {
        return <FontAwesomeIcon icon="heartbeat" style={{ color: "#dc004e" }} size="lg" fixedWidth />;
      }
      return <FontAwesomeIcon icon="heartbeat" size="lg" fixedWidth />;
    }
    return <FontAwesomeIcon icon="heart-broken" size="lg" fixedWidth spin={true} />;
  };

  return (
    <div className={classes.grow}>
      <AppBar position="fixed" color="primary" className={classes.appBar}>
        <ReconnectSignalr />
        <Toolbar>
          <LeftMenu />
          <Displayheart />
          <Box ml={2}>
            <Typography>{signalR.status !== SignalRStatus.Ok ? signalR.status.toString() : ""}</Typography>
          </Box>
          <div className={classes.grow} />

          {location.pathname !== "/" && (
            <Fab color="secondary" aria-label="add" className={classes.fabButton} component={NavLink} to="/">
              <HomeIcon />
            </Fab>
          )}
          <IconButton onClick={themeContext.toggleMode}>
            {theme.palette.type === "light" && <LightModeIcon />}
            {theme.palette.type === "dark" && <DarkModeIcon />}
          </IconButton>
        </Toolbar>
      </AppBar>
    </div>
  );
};
