import React, { useState, useContext } from "react";
import { ApiContext } from "src/infrastructure/ApiContextProvider";
import { useHistory } from "react-router";
import {
    Card,
    CardHeader,
    CardContent,
    ButtonGroup,
    Button,
    CardActions,
    Paper,
    Box,
    TextField,
    FormControl,
    InputLabel,
    Select,
    MenuItem,
    makeStyles,
    createStyles,
    Theme,
    FormGroup,
    Checkbox,
    FormControlLabel,
    IconButton
} from "@material-ui/core";
import clsx from "clsx";
import { IUser, IRole } from "./Models";
import { useSnackbar } from "notistack";
import { FormValidator, ValidationElement } from "src/infrastructure/validator";
import Visibility from "@material-ui/icons/Visibility";
import VisibilityOff from "@material-ui/icons/VisibilityOff";
import { CommandResultDto, IDropdownValue } from "src/infrastructure/models";

const cultures: IDropdownValue[] = [
    { key: "nb-NO", value: "Norsk" },
    { key: "en-US", value: "English" }
];

const useStyles = makeStyles((theme: Theme) =>
    createStyles({
        root: {
            display: "flex",
            flexWrap: "wrap"
        },
        margin: {
            margin: theme.spacing(1)
        },
        withoutLabel: {
            marginTop: theme.spacing(3)
        },
        textField: {
            width: 200
        },
        formControl: {
            //   margin: theme.spacing(1),
            minWidth: 120
        },
        selectEmpty: {
            marginTop: theme.spacing(2)
        }
    })
);

export const EditUserDetails = (props: any) => {
    const classes = useStyles();
    const api = useContext(ApiContext);
    const history = useHistory();
    const [user, setUser] = useState<IUser>(props.user);
    const [roles, setRoles] = useState<IRole[]>(props.roles);
    const [showPasswords, setShowPasswords] = useState(false);

    const validatorElements = [];
    validatorElements.push(new ValidationElement("fullName", user.fullName, { required: true, minLength: 2, maxLength: 100 }));
    validatorElements.push(new ValidationElement("userName", user.userName, { required: true, email: true }));
    validatorElements.push(new ValidationElement("culture", user.culture, { required: true }));

    if (!user.id) {
        validatorElements.push(new ValidationElement("password", user.password, { required: true, password: true }));
        validatorElements.push(new ValidationElement("passwordRepeat", user.passwordRepeat, { required: true, valueMustMachOtherElementWithName: "password" }));
    }

    const [validator, setValidator] = useState(new FormValidator(validatorElements));

    const { enqueueSnackbar } = useSnackbar();

    const save = async () => {
        if (user.id) {
            const saveResult = await api.post<CommandResultDto<IUser>>("user", { User: user, Roles: roles });
            if (saveResult.data) {
                enqueueSnackbar("User details saved", { variant: "success", anchorOrigin: { vertical: "top", horizontal: "right" } });
                setUser(saveResult.data);
                setValidator(validator.setPristine());
            }
        } else {
            const toSave = { ...user, id: undefined, newPassword: undefined, newPasswordRepeat: undefined };
            const saveResult = await api.post<CommandResultDto<IUser>>("user/create", { User: toSave, NewPassword: user.password, NewPasswordRepeat: user.passwordRepeat, Roles: roles });
            if (saveResult.data) {
                enqueueSnackbar("User successfully created", { variant: "success", anchorOrigin: { vertical: "top", horizontal: "right" } });
                history.replace(`/users/${saveResult.data.id}`);
            }
        }
    };

    const handleCultureChange = (event: React.ChangeEvent<{ value: unknown }>) => {
        const newCulture = cultures.find(x => x.key === (event.target.value as string));
        const newState = { ...user, culture: newCulture } as IUser;
        setUser(newState);
        setValidator(validator.updateValue("culture", newCulture));
    };

    const handleTextChange = (key: string, newValue: string) => {
        const newState = { ...user, [key]: newValue } as IUser;
        setUser(newState);
        setValidator(validator.updateValue(key, newValue));
    };

    const handleRoleChange = (name: string) => (event: React.ChangeEvent<HTMLInputElement>) => {
        let newRoles = [...roles];
        const newRole = newRoles.find(x => x.name === name);
        if (newRole) {
            newRole.selected = !newRole.selected;
        }
        setRoles(newRoles);
        setValidator(validator.setDirty());
    };

    const handleBlur = (name: string) => {
        setValidator(validator.onBlur(name));
    };

    const handleClickShowPassword = () => {
        setShowPasswords(!showPasswords);
    };

    const handleMouseDownPassword = (event: React.MouseEvent<HTMLButtonElement>) => {
        event.preventDefault();
    };

    return (
        <div>
            {user && (
                <Paper elevation={3}>
                    <Card>
                        <CardHeader title="User details"></CardHeader>
                        <CardContent>
                            <form action="" autoComplete="off">
                                <TextField
                                    id="details-fullName"
                                    label="Name"
                                    value={user.fullName}
                                    disabled={false}
                                    required={true}
                                    autoComplete="name"
                                    error={validator.reportError("fullName")}
                                    helperText={validator.errorMessage("fullName")}
                                    onChange={event => handleTextChange("fullName", event.target.value)}
                                    className={classes.margin}
                                    onBlur={() => handleBlur("fullName")}
                                />
                                <TextField
                                    id="details-userName"
                                    label="Email/Login"
                                    value={user.userName}
                                    disabled={false}
                                    required={true}
                                    autoComplete="username"
                                    error={validator.reportError("userName")}
                                    helperText={validator.errorMessage("userName")}
                                    onChange={event => handleTextChange("userName", event.target.value)}
                                    className={classes.margin}
                                    onBlur={() => handleBlur("userName")}
                                />

                                <FormControl className={clsx(classes.margin, classes.formControl)}>
                                    <InputLabel id="details-culture-label">Culture</InputLabel>
                                    <Select
                                        labelId="details-culture-label"
                                        id="details-culture-select"
                                        value={user.culture?.key || ""}
                                        onChange={handleCultureChange}
                                        onBlur={() => handleBlur("culture")}
                                        error={validator.reportError("culture")}
                                    >
                                        {cultures.map((culture: IDropdownValue) => (
                                            <MenuItem key={culture.key} value={culture.key}>
                                                {culture.value}
                                            </MenuItem>
                                        ))}
                                    </Select>
                                </FormControl>
                                {/* <pre>{JSON.stringify(validator, null, 2)}</pre> */}
                                {roles && (
                                    <Box mt={3}>
                                        <h5>Roles</h5>
                                        <FormGroup row>
                                            {roles.map((role: IRole) => (
                                                <FormControlLabel
                                                    key={role.name}
                                                    control={<Checkbox color="default" checked={role.selected} onChange={handleRoleChange(role.name)} value={role.name} />}
                                                    label={role.name}
                                                />
                                            ))}
                                        </FormGroup>
                                    </Box>
                                )}

                                {!user.id && (
                                    <Box>
                                        <TextField
                                            id="details-password"
                                            type={showPasswords ? "text" : "password"}
                                            label="New password"
                                            value={user.password ||""}
                                            autoComplete="new-password"
                                            error={validator.reportError("password")}
                                            helperText={validator.errorMessage("password")}
                                            onChange={event => handleTextChange("password", event.target.value)}
                                            className={classes.margin}
                                            onBlur={() => handleBlur("password")}
                                        />

                                        <TextField
                                            id="details-password-repeat"
                                            type={showPasswords ? "text" : "password"}
                                            label="Repeat new password"
                                            value={user.passwordRepeat ||""}
                                            autoComplete="new-password"
                                            error={validator.reportError("passwordRepeat")}
                                            helperText={validator.errorMessage("passwordRepeat")}
                                            onChange={event => handleTextChange("passwordRepeat", event.target.value)}
                                            className={classes.margin}
                                            onBlur={() => handleBlur("passwordRepeat")}
                                        />

                                        <IconButton aria-label="toggle password visibility" onClick={handleClickShowPassword} onMouseDown={handleMouseDownPassword}>
                                            {showPasswords ? <Visibility /> : <VisibilityOff />}
                                        </IconButton>
                                    </Box>
                                )}
                            </form>
                        </CardContent>
                        <CardActions disableSpacing>
                            <ButtonGroup>
                                <Button variant="contained" color="primary" type="button" id="sendlogin" onClick={save} disabled={!validator.allowSave}>
                                    Save
                                </Button>
                            </ButtonGroup>
                        </CardActions>
                    </Card>
                </Paper>
            )}
        </div>
    );
};
