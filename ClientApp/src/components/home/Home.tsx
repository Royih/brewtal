import React from "react";
import { Grid, makeStyles, createStyles, Theme } from "@material-ui/core";

import { Pid } from "./Pid";

const useStyles = makeStyles((theme: Theme) =>
    createStyles({
        root: {
            flexGrow: 1,
        },
        paper: {
            padding: theme.spacing(2),
            textAlign: "center",
            color: theme.palette.text.secondary,
        },
    })
);

export const Home = () => {
    const classes = useStyles();

    return (
        <div className={classes.root}>
            <Grid container spacing={3}>
                <Grid item xs={12} sm={6}>
                    <Pid id={0}></Pid>
                </Grid>
                <Grid item xs={12} sm={6}>
                    <Pid id={1}></Pid>
                </Grid>
            </Grid>
        </div>
    );
};
