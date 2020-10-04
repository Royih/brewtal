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
    TextField,
    makeStyles,
    createStyles,
    Theme,
} from "@material-ui/core";
import { Brew } from "./Models";
import { useSnackbar } from "notistack";
import { FormValidator, ValidationElement } from "src/infrastructure/validator";

const useStyles = makeStyles((theme: Theme) =>
    createStyles({
        root: {
            display: "flex",
            flexWrap: "wrap",
        },
        margin: {
            margin: theme.spacing(1),
        },
        withoutLabel: {
            marginTop: theme.spacing(3),
        },
        textField: {
            width: 200,
        },
        formControl: {
            //   margin: theme.spacing(1),
            minWidth: 120,
        },
        selectEmpty: {
            marginTop: theme.spacing(2),
        },
    })
);

export interface IProps {
    brew: Brew;
}

export const BrewForm = (props: IProps) => {
    const classes = useStyles();
    const api = useContext(ApiContext);
    const history = useHistory();
    const [brew, setBrew] = useState<Brew>(props.brew);

    const validatorElements = [];
    validatorElements.push(new ValidationElement("name", brew.name, { required: true, minLength: 2, maxLength: 100 }));
    validatorElements.push(new ValidationElement("batchNumber", brew.batchNumber, { required: true }));
    

    const [validator, setValidator] = useState(new FormValidator(validatorElements));

    const { enqueueSnackbar } = useSnackbar();

    const save = async () => {
        const brewAfterSave = await api.post<Brew>("brew", { Brew: brew });
        if (brewAfterSave) {
            enqueueSnackbar("Brew successfully saved", { variant: "success", anchorOrigin: { vertical: "top", horizontal: "right" } });
            if (!brew.id) {
                history.replace(`/brew/${brewAfterSave.id}`);
            } else {
                setBrew(brewAfterSave);
                setValidator(validator.setPristine());
            }
        }
        // } else {
        //     const toSave = { ...user, id: undefined, newPassword: undefined, newPasswordRepeat: undefined };
        //     const saveResult = await api.post<CommandResultDto<IUser>>("user/create", { User: toSave, NewPassword: user.password, NewPasswordRepeat: user.passwordRepeat, Roles: roles });
        //     if (saveResult.data) {
        //         enqueueSnackbar("User successfully created", { variant: "success", anchorOrigin: { vertical: "top", horizontal: "right" } });
        //         history.replace(`/users/${saveResult.data.id}`);
        //     }
        // }
    };

    const handleTextChange = (key: string, newValue: string) => {
        const newState = { ...brew, [key]: newValue } as Brew;
        setBrew(newState);
        setValidator(validator.updateValue(key, newValue));
    };

    const handleBlur = (name: string) => {
        setValidator(validator.onBlur(name));
    };

    return (
        <div>
            {brew && (
                <Paper elevation={3}>
                    <Card>
                        <CardHeader title="Brew details"></CardHeader>
                        <CardContent>
                            <form action="" autoComplete="off">
                                <TextField
                                    id="details-name"
                                    label="Name"
                                    value={brew.name}
                                    disabled={false}
                                    required={true}
                                    autoComplete="off"
                                    error={validator.reportError("name")}
                                    helperText={validator.errorMessage("name")}
                                    onChange={(event) => handleTextChange("name", event.target.value)}
                                    className={classes.margin}
                                    onBlur={() => handleBlur("name")}
                                />
                                <TextField
                                    id="details-batchNumbver"
                                    label="Batch #"
                                    value={brew.batchNumber}
                                    disabled={false}
                                    required={true}
                                    autoComplete="off"
                                    error={validator.reportError("batchNumber")}
                                    helperText={validator.errorMessage("batchNumber")}
                                    onChange={(event) => handleTextChange("batchNumber", event.target.value)}
                                    className={classes.margin}
                                    onBlur={() => handleBlur("batchNumber")}
                                />
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
