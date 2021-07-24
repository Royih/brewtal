import React from "react";
import Alert from "@material-ui/lab/Alert";

export const AccessDenied = () => {
    return (
        <Alert severity="error">
            <h2>
                <i className="fas fa-user-lock mr-3"></i> Access denied
            </h2>
            You do not have access to this section of the app
        </Alert>
    );
};
