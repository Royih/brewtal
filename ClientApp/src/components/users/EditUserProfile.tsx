import React, { useState, useEffect, useContext } from "react";
import { useParams, useHistory } from "react-router";
import { Loading } from "../common/Loading";
import { ApiContext } from "src/infrastructure/ApiContextProvider";
import { ButtonGroup, Button, Box, Typography } from "@material-ui/core";
import { ChangePassword } from "./ChangePassword";
import { EditUserDetails } from "./EditUserDetails";
import { IUser, IRole } from "./Models";
import { Confirm } from "../common/Confirm";
import { CommandResultDto } from "src/infrastructure/models";
import { useSnackbar } from "notistack";

export const EditUserProfile = () => {
    const api = useContext(ApiContext);
    const [user, setUser] = useState<IUser>();
    const [roles, setRoles] = useState<IRole[]>();
    const [loading, setLoading] = useState(true);
    const [confirmDelete, setConfirmDelete] = useState(false);
    const { id } = useParams();
    const history = useHistory();
    const { enqueueSnackbar } = useSnackbar();

    useEffect(() => {
        const fetchData = async () => {
            if (id) {
                setUser(await api.get<IUser>("user/get/" + id));
                setRoles(await api.get<IRole[]>("user/listRoles/" + id));
            } else {
                setUser({ id: "", fullName: "", userName: "" } as IUser);
                setRoles(await api.get<IRole[]>("user/listRoles"));
            }
            setLoading(false);
        };
        fetchData();
    }, [id, api]);

    const handleDelete = async () => {
        setConfirmDelete(false);
        const saveResult = await api.post<CommandResultDto<any>>("user/delete", { User: user });
        if (saveResult.success) {
            enqueueSnackbar("User successfully deleted", { variant: "success", anchorOrigin: { vertical: "top", horizontal: "right" } });
            history.replace("/users");
        } else {
            enqueueSnackbar("User deletion failed", { variant: "error", anchorOrigin: { vertical: "top", horizontal: "right" } });
        }
    };

    return (
        <div>
            {loading && <Loading />}
            {user && roles && (
                <div>
                    <Typography variant="h3" gutterBottom>
                        {user.id ? "User profile for " + user.fullName : "Create new user profile"}
                    </Typography>
                    <ButtonGroup>
                        <Button variant="contained" color="default" onClick={() => history.goBack()}>
                            Back
                        </Button>
                        <Button variant="contained" color="secondary" onClick={() => setConfirmDelete(true)}>
                            Delete
                        </Button>
                    </ButtonGroup>
                    <Box mt={3}>
                        <EditUserDetails user={user} roles={roles} />
                    </Box>
                    {user.id && (
                        <Box mt={3}>
                            <ChangePassword user={user}></ChangePassword>
                        </Box>
                    )}
                    <Confirm
                        show={confirmDelete}
                        title="Are you sure?"
                        body={`Are you sure you want to delete the user ${user.fullName}`}
                        proceedButtonText="Yes"
                        onProceedClick={handleDelete}
                        onCancelClick={() => setConfirmDelete(false)}
                    ></Confirm>
                </div>
            )}
        </div>
    );
};
