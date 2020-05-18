import React from "react";
import { CircularProgress } from "@material-ui/core";

export const Loading = () => {
    return (
        <CircularProgress color='secondary'></CircularProgress>
    );
}