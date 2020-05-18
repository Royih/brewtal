import React, { useState, useEffect } from "react";
import { useHistory } from "react-router-dom";
import { Loading } from "../common/Loading";
import { UserContext, RoleTypes } from "src/infrastructure/UserContextProvider";
import { AccessDenied } from "../common/AccessDenied";
import { ApiContext } from "src/infrastructure/ApiContextProvider";
import { ButtonGroup, Table, TableBody, TableHead, TableRow, TableCell, Paper, Box, Button, Typography, TableContainer } from "@material-ui/core";
import AddIcon from "@material-ui/icons/Add";
import { IDropdownValue } from "src/infrastructure/models";

interface IUser {
    id: string;
    fullName: string;
    userName: string;
    tenantName: string;
    culture: IDropdownValue;
}

export const Users = () => {
    const [users, setUsers] = useState<IUser[] | undefined>();
    const [loading, setLoading] = useState<boolean>(true);
    const userContext = React.useContext(UserContext);
    const api = React.useContext(ApiContext);
    const history = useHistory();

    const Contents = () => {
        if (!userContext.hasRole(RoleTypes.Admin)) {
            return <AccessDenied />;
        }
        if (loading) {
            return <Loading />;
        }
        return <Render />;
    };

    const Render = () => {
        if (users) {
            const createNew = async () => {
                history.push("/user/create");
            };
            return (
                <Box mt={3}>
                    <ButtonGroup variant="contained">
                        <Button color="primary" onClick={createNew} startIcon={<AddIcon />}>
                            Create new user
                        </Button>
                    </ButtonGroup>
                    <Box mt={3}>
                        <Paper elevation={3}>
                            <TableContainer>
                                <Table>
                                    <TableHead>
                                        <TableRow>
                                            <TableCell>Name</TableCell>
                                            <TableCell>Login</TableCell>
                                            <TableCell>Roles</TableCell>
                                            <TableCell>Culture</TableCell>
                                        </TableRow>
                                    </TableHead>
                                    <TableBody>
                                        {users.map((user: IUser) => (
                                            <TableRow key={user.id} hover onClick={() => history.push("/users/" + user.id)}>
                                                <TableCell>{user.fullName}</TableCell>
                                                <TableCell>{user.userName}</TableCell>
                                                <TableCell></TableCell>
                                                <TableCell>{user.culture?.value}</TableCell>
                                            </TableRow>
                                        ))}
                                    </TableBody>
                                </Table>
                            </TableContainer>
                        </Paper>
                    </Box>
                </Box>
            );
        }
        return null;
    };

    useEffect(() => {
        const fetchData = async () => {
            setLoading(true);
            const data = await api.get<IUser[]>("user/list");
            setUsers(data);
            setLoading(false);
        };

        async function load() {
            await fetchData();
        }
        load();
    }, [api]);

    return (
        <div>
            <Typography variant="h3" gutterBottom>
                Users
            </Typography>
            <Contents />
        </div>
    );
    // }
};
