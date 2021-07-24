import React from "react";
import { Dialog, DialogTitle, DialogContent, DialogContentText, DialogActions, Button } from "@material-ui/core";

export interface IConfirmProps {
    title: string;
    body: string;
    cancelButtonText?: string;
    proceedButtonText?: string;
    show: boolean;
    onCancelClick(): void;
    onProceedClick(): void;
}

export const Confirm = (props: IConfirmProps) => {
    //const [open, setOpen] = React.useState(false);

    const handleCancelClick = () => {
        console.log("Cancel click");
        props.onCancelClick();
        //setOpen(false);
    };

    const handleProceedClick = () => {
        props.onProceedClick();
        //setOpen(false);
    };

    if (props.show) {
        return (
            <Dialog open={true} onClose={handleCancelClick} aria-labelledby="confirm-delete-dialog-title">
                <DialogTitle id="confirm-delete-dialog-title">{props.title}</DialogTitle>
                <DialogContent>
                    <DialogContentText>{props.body}</DialogContentText>
                </DialogContent>
                <DialogActions>
                    <Button autoFocus onClick={handleCancelClick} color="primary">
                        {props.cancelButtonText || "Cancel"}
                    </Button>
                    <Button onClick={handleProceedClick} color="secondary">
                        {props.proceedButtonText || "Ok"}
                    </Button>
                </DialogActions>
            </Dialog>
        );
    }
    return null;
};
