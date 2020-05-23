import React, { useContext, useEffect, useState } from "react";
import AppBar from "@material-ui/core/AppBar";
import { createStyles, Theme, makeStyles, useTheme } from "@material-ui/core/styles";
import Toolbar from "@material-ui/core/Toolbar";
import Typography from "@material-ui/core/Typography";
import IconButton from "@material-ui/core/IconButton";
import AccountCircle from "@material-ui/icons/AccountCircle";
import MoreIcon from "@material-ui/icons/MoreVert";
import { LeftMenu } from "./LeftMenu";
import { Menu, MenuItem, ListItemIcon, Fab } from "@material-ui/core";
import { useLocation } from "react-router";
import DarkModeIcon from "@material-ui/icons/Brightness4";
import LightModeIcon from "@material-ui/icons/Brightness5";
import HomeIcon from "@material-ui/icons/Home";
import HeartIcon from "@material-ui/icons/Favorite";
import HeartEmptyIcon from "@material-ui/icons/FavoriteBorder";
import { ThemeContext } from "src/infrastructure/ThemeContextProvider";
import { NavLink } from "react-router-dom";
import { UserContext } from "src/infrastructure/UserContextProvider";
import { SignalrContext } from "src/infrastructure/SignalrContextProvider";

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

export const Footer = (props: any) => {
    const classes = useStyles();
    const theme = useTheme();
    const themeContext = useContext(ThemeContext);
    const signalr = useContext(SignalrContext);
    const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);
    const [mobileMoreAnchorEl, setMobileMoreAnchorEl] = React.useState<null | HTMLElement>(null);
    const currentUser = useContext(UserContext);
    const [beat, setBeat] = useState(false);

    const isMenuOpen = Boolean(anchorEl);
    const isMobileMenuOpen = Boolean(mobileMoreAnchorEl);

    const handleProfileMenuOpen = (event: React.MouseEvent<HTMLElement>) => {
        setAnchorEl(event.currentTarget);
    };

    const handleMobileMenuClose = () => {
        setMobileMoreAnchorEl(null);
    };

    const handleMenuClose = () => {
        setAnchorEl(null);
        handleMobileMenuClose();
    };

    const handleMobileMenuOpen = (event: React.MouseEvent<HTMLElement>) => {
        setMobileMoreAnchorEl(event.currentTarget);
    };

    useEffect(() => {
        const fetchData = async () => {};
        fetchData();
    }, []);

    useEffect(() => {
        setBeat((currentBeat: any) => !currentBeat);
    }, [signalr.hwStatus]);

    let location = useLocation();

    const Displayheart = () => {
        if (beat) {
            return <HeartIcon style={{ color: "#dc004e" }} />;
        }
        return <HeartEmptyIcon />;
    };

    return (
        <div className={classes.grow}>
            <AppBar position="fixed" color="primary" className={classes.appBar}>
                <Toolbar>
                    <LeftMenu />
                    <Displayheart />
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
