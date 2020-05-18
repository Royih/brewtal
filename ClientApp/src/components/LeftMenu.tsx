import React from "react";
import { createStyles, Theme, makeStyles } from "@material-ui/core/styles";
import SwipeableDrawer from "@material-ui/core/SwipeableDrawer";
import MenuIcon from "@material-ui/icons/Menu";
import List from "@material-ui/core/List";
import Divider from "@material-ui/core/Divider";
import ListItem from "@material-ui/core/ListItem";
import ListItemIcon from "@material-ui/core/ListItemIcon";
import ListItemText from "@material-ui/core/ListItemText";
import InboxIcon from "@material-ui/icons/MoveToInbox";
import HomeIcon from "@material-ui/icons/Home";
import IconButton from "@material-ui/core/IconButton";
import { useHistory } from "react-router-dom";

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

    const iOS = (process as any).browser && /iPad|iPhone|iPod/.test(navigator.userAgent);

    type DrawerSide = "top" | "left" | "bottom" | "right";
    const toggleDrawer = (side: DrawerSide, open: boolean) => (event: React.KeyboardEvent | React.MouseEvent) => {
        if (event && event.type === "keydown" && ((event as React.KeyboardEvent).key === "Tab" || (event as React.KeyboardEvent).key === "Shift")) {
            return;
        }

        setState({ ...state, [side]: open });
    };

    const GetListItem = (key: string, label: string, url: string, icon: JSX.Element | null) => (
        <ListItem
            button
            key={key}
            onClick={() => {
                history.push(url);
            }}
        >
            {icon && <ListItemIcon>{icon}</ListItemIcon>}
            <ListItemText primary={label} />
        </ListItem>
    );

    const GetLeftMenuItem = (index: number, type: string, key: string, label: string, url: string, icon: JSX.Element | null, display: boolean) => {
        return (
            display &&
            (type === "ListItem" ? (
                GetListItem(key, label, url, icon)
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
                    { type: "ListItem", key: "home", label: "Home", url: "/", icon: <HomeIcon />, display: true },
                    { type: "Divder", key: "", label: "", url: "", icon: null, display: currentUserIsAdmin },
                    { type: "ListItem", key: "users", label: "Users", url: "/users", icon: <InboxIcon />, display: currentUserIsAdmin },
                    { type: "Divder", key: "", label: "", url: "", icon: null, display: currentUserIsAdmin },
                    { type: "Title", key: "functionDemo", label: "Function demo", url: "", icon: null, display: currentUserIsAdmin },
                    { type: "ListItem", key: "counter", label: "Counter", url: "/counter", icon: <InboxIcon />, display: currentUserIsAdmin },
                    { type: "ListItem", key: "fetchData", label: "Fetch data", url: "/fetch-data", icon: <InboxIcon />, display: currentUserIsAdmin },
                    { type: "ListItem", key: "testSignalR", label: "Test signalR", url: "/test-signalr", icon: <InboxIcon />, display: currentUserIsAdmin },
                    { type: "ListItem", key: "throwExceptions", label: "Throw exceptions", url: "/throw-exceptions", icon: <InboxIcon />, display: currentUserIsAdmin },
                ].map(({ type, key, label, url, icon, display }, index) => GetLeftMenuItem(index, type, key, label, url, icon, display))}
            </List>
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
