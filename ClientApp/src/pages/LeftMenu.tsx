import React, { useContext, useState } from "react";
import { createStyles, Theme, makeStyles } from "@material-ui/core/styles";
import SwipeableDrawer from "@material-ui/core/SwipeableDrawer";
import MenuIcon from "@material-ui/icons/Menu";
import List from "@material-ui/core/List";
import Divider from "@material-ui/core/Divider";
import ListItem from "@material-ui/core/ListItem";
import ListItemIcon from "@material-ui/core/ListItemIcon";
import ListItemText from "@material-ui/core/ListItemText";
import HomeIcon from "@material-ui/icons/Home";
import IconButton from "@material-ui/core/IconButton";
import RefreshIcon from "@material-ui/icons/Refresh";
import { useHistory } from "react-router-dom";
import { Confirm } from "src/components/Confirm";
import { SignalrContext } from "src/infrastructure/SignalrContextProvider";

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    list: {
      width: 250,
    },
    fullList: {
      width: "auto",
    },
    menuButton: {
      marginRight: theme.spacing(2),
    },
  })
);

export const LeftMenu = () => {
  const classes = useStyles();
  const history = useHistory();
  const currentUserIsAdmin = true;
  const [state, setState] = React.useState({
    top: false,
    left: false,
    bottom: false,
    right: false,
  });
  const signalr = useContext(SignalrContext);
  const [confirmShutdown, setConfirmShutdown] = useState(false);
  const iOS = (process as any).browser && /iPad|iPhone|iPod/.test(navigator.userAgent);

  type DrawerSide = "top" | "left" | "bottom" | "right";
  const toggleDrawer = (side: DrawerSide, open: boolean) => (event: React.KeyboardEvent | React.MouseEvent) => {
    if (event && event.type === "keydown" && ((event as React.KeyboardEvent).key === "Tab" || (event as React.KeyboardEvent).key === "Shift")) {
      return;
    }

    setState({ ...state, [side]: open });
  };

  const GetListItem = (key: string, label: string, url: string, icon: JSX.Element | null, onClick: any) => (
    <ListItem
      button
      key={key}
      onClick={(e: any) => {
        if (url) {
          history.push(url);
        } else {
          onClick(e);
        }
      }}
    >
      {icon && <ListItemIcon>{icon}</ListItemIcon>}
      <ListItemText primary={label} />
    </ListItem>
  );

  const GetLeftMenuItem = (index: number, type: string, key: string, label: string, url: string, icon: JSX.Element | null, display: boolean, onClick: any) => {
    return (
      display &&
      (type === "ListItem" ? (
        GetListItem(key, label, url, icon, onClick)
      ) : type === "Title" ? (
        <ListItem key={key}>
          <ListItemText primary={label} />
        </ListItem>
      ) : (
        <Divider key={"div_" + index} />
      ))
    );
  };

  const sideList = (side: DrawerSide) => (
    <div className={classes.list} role="presentation" onClick={toggleDrawer(side, false)} onKeyDown={toggleDrawer(side, false)}>
      <List>
        {[
          { type: "ListItem", key: "home", label: "Home", url: "/", icon: <HomeIcon />, display: true, onClick: null },
          { type: "Divder", key: "", label: "", url: "", icon: null, display: currentUserIsAdmin, onClick: null },
          { type: "ListItem", key: "reload", label: "Reload app", url: "", icon: <RefreshIcon />, display: true, onClick: () => window.location.reload() },
          {
            type: "ListItem",
            key: "shutdown",
            label: "Shutdown RPI",
            url: "",
            icon: <RefreshIcon />,
            display: true,
            onClick: (e: any) => {
              setConfirmShutdown(true);
              e.stopPropagation();
            },
          },
        ].map(({ type, key, label, url, icon, display, onClick }, index) => GetLeftMenuItem(index, type, key, label, url, icon, display, onClick))}
      </List>

      <Confirm
        title="Are you sure?"
        body="This will shut-down the RPI, and it will need to be powered on manually."
        show={confirmShutdown}
        onCancelClick={() => {
          setConfirmShutdown(false);
        }}
        onProceedClick={() => {
          if (signalr.hubConnection?.state === "Connected") {
            signalr.hubConnection?.invoke("Shutdown");
          }
          setConfirmShutdown(false);
        }}
      ></Confirm>
    </div>
  );

  return (
    <>
      <IconButton edge="start" className={classes.menuButton} color="inherit" aria-label="open drawer" onClick={toggleDrawer("left", true)}>
        <MenuIcon />
      </IconButton>
      <SwipeableDrawer open={state.left} onClose={toggleDrawer("left", false)} onOpen={toggleDrawer("left", true)} disableBackdropTransition={!iOS} disableDiscovery={iOS}>
        {sideList("left")}
      </SwipeableDrawer>
    </>
  );
};
