import React, { useContext, useState } from "react";
import Visibility from "@material-ui/icons/Visibility";
import VisibilityOff from "@material-ui/icons/VisibilityOff";
import { Paper, Card, CardHeader, CardContent, IconButton, CardActions, Button, TextField } from "@material-ui/core";
import { ApiContext } from "src/infrastructure/ApiContextProvider";
import { useHistory } from "react-router";
import { useSnackbar } from "notistack";
import { ValidationElement, FormValidator } from "src/infrastructure/validator";

interface ChangePasswordState {
    password: string;
    passwordRepeat: string;
    showPasswords: boolean;
}

export const ChangePassword = (props: any) => {
    const apiContext = useContext(ApiContext);
    const emptyPasswordState = { password: "", passwordRepeat: "", showPasswords: false } as ChangePasswordState;
    const [state, setState] = useState<ChangePasswordState>(emptyPasswordState);
    const { enqueueSnackbar } = useSnackbar();

    const validatorElements = [];
    validatorElements.push(new ValidationElement("password", state.password, { required: true, password: true }));
    validatorElements.push(new ValidationElement("passwordRepeat", state.passwordRepeat, { required: true, valueMustMachOtherElementWithName: "password" }));
    const [validator, setValidator] = useState(new FormValidator(validatorElements));

    let history = useHistory();

    const changePassword = async () => {
        await apiContext.post("user/resetPassword", { User: props.user, NewPassword: state.password, NewPasswordRepeat: state.passwordRepeat });
        enqueueSnackbar("Password successfully changed", { variant: "success", anchorOrigin: { vertical: "top", horizontal: "right" } });
        setState(emptyPasswordState);
        history.replace(`/users/${props.user.id}`);
        setValidator(validator.setPristine());
    };

    const handleChange = (prop: keyof ChangePasswordState) => (event: React.ChangeEvent<HTMLInputElement>) => {
        setState({ ...state, [prop]: event.target.value });
        setValidator(validator.updateValue(prop, event.target.value));
    };

    const handleBlur = (name: string) => {
        setValidator(validator.onBlur(name));
    };

    const handleClickShowPassword = () => {
        setState({ ...state, showPasswords: !state.showPasswords });
    };

    const handleMouseDownPassword = (event: React.MouseEvent<HTMLButtonElement>) => {
        event.preventDefault();
    };

    const handleKeyPress = (e: React.KeyboardEvent<HTMLDivElement>) => {
        if (e.key === "Enter") {
            if (validator.allowSave) {
                changePassword();
            }
        }
    };

    return (
        <Paper elevation={3}>
            <Card>
                <CardHeader title="Change password"></CardHeader>
                <CardContent>
                    <form action="">
                        <input type="text" readOnly={true} value={props.user.userName} autoComplete="username" style={{ display: "none" }} />

                        <TextField
                            id="change-password"
                            type={state.showPasswords ? "text" : "password"}
                            label="New password"
                            value={state.password}
                            autoComplete="new-password"
                            error={validator.reportError("password")}
                            helperText={validator.errorMessage("password")}
                            onChange={handleChange("password")}
                            onBlur={() => handleBlur("password")}
                        />

                        <TextField
                            id="change-password-repeat"
                            type={state.showPasswords ? "text" : "password"}
                            label="Repeat new password"
                            value={state.passwordRepeat}
                            autoComplete="new-password"
                            error={validator.reportError("passwordRepeat")}
                            helperText={validator.errorMessage("passwordRepeat")}
                            onChange={handleChange("passwordRepeat")}
                            onBlur={() => handleBlur("passwordRepeat")}
                            onKeyPress={handleKeyPress}
                        />

                        <IconButton aria-label="toggle password visibility" onClick={handleClickShowPassword} onMouseDown={handleMouseDownPassword}>
                            {state.showPasswords ? <Visibility /> : <VisibilityOff />}
                        </IconButton>

                        {/* <pre>{JSON.stringify(validator, null, 2)}</pre> */}
                    </form>
                </CardContent>
                <CardActions disableSpacing>
                    <Button variant="contained" color="primary" id="sendlogin" onClick={changePassword} disabled={!validator.allowSave}>
                        Change password
                    </Button>
                </CardActions>
            </Card>
        </Paper>
    );
};
