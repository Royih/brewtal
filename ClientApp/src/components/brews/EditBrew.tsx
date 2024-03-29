import React, { useState, useEffect, useContext } from "react";
import { useParams, useHistory } from "react-router";
import { Loading } from "../common/Loading";
import { ApiContext } from "src/infrastructure/ApiContextProvider";
import { ButtonGroup, Button, Box, Typography } from "@material-ui/core";
import { Brew } from "./Models";
import { Confirm } from "../common/Confirm";
import { useSnackbar } from "notistack";
import { CommandResultDto } from "src/infrastructure/models";
import { BrewForm } from "./BrewForm";

export const EditBrew = () => {
  const api = useContext(ApiContext);
  const [brew, setBrew] = useState<Brew>();
  const [brewToSave, setBrewToSave] = useState<Brew | undefined>();
  const [loading, setLoading] = useState(true);
  const [confirmDelete, setConfirmDelete] = useState(false);
  const { id } = useParams();
  const history = useHistory();
  const { enqueueSnackbar } = useSnackbar();

  useEffect(() => {
    const fetchData = async () => {
      setBrew(await api.get<Brew>(id ? "brews/get/" + id : "brews/getNew"));
      setLoading(false);
    };
    fetchData();
  }, [id, api]);

  const save = async () => {
    if (brewToSave) {
      const brewAfterSave = await api.post<Brew>("brews/save", { Brew: brewToSave });
      console.log("Her-1");
      if (brewAfterSave) {
        enqueueSnackbar("Brew successfully saved", { variant: "success", anchorOrigin: { vertical: "top", horizontal: "right" } });
        if (!brewToSave.id) {
          history.replace(`/brew/edit/${brewAfterSave.id}`);
        } /*else {
          setBrew(brewAfterSave);
        }*/
        console.log("Her-2");
        setBrewToSave(undefined);
      }
    }
  };

  const handleDelete = async () => {
    setConfirmDelete(false);
    const saveResult = await api.post<CommandResultDto<any>>("brews/delete", { Brew: brew });
    if (saveResult) {
      enqueueSnackbar("Brew successfully deleted", { variant: "success", anchorOrigin: { vertical: "top", horizontal: "right" } });
      history.replace("/brews");
    } else {
      enqueueSnackbar("Brew deletion failed", { variant: "error", anchorOrigin: { vertical: "top", horizontal: "right" } });
    }
  };

  const formChanged = (newBrew: Brew) => {
    setBrewToSave(newBrew);
  };

  return (
    <div>
      {loading && <Loading />}
      {brew && (
        <div>
          <Typography variant="h3" gutterBottom>
            {brew.id ? brew.name : "Register new brew"}
          </Typography>
          <ButtonGroup>
            <Button variant="contained" color="default" onClick={() => history.goBack()}>
              Back
            </Button>
            <Button variant="contained" color="secondary" onClick={() => setConfirmDelete(true)}>
              Delete
            </Button>
            <Button variant="contained" color="primary" type="button" id="save" disabled={brewToSave === undefined} onClick={save}>
              Save
            </Button>
          </ButtonGroup>
          <Box mt={3}>
            <BrewForm brew={brew} onChange={formChanged} />
          </Box>
          <Confirm
            show={confirmDelete}
            title="Are you sure?"
            body={`Are you sure you want to delete the brew ${brew.name}`}
            proceedButtonText="Yes"
            onProceedClick={handleDelete}
            onCancelClick={() => setConfirmDelete(false)}
          ></Confirm>
          <pre>{JSON.stringify(brewToSave, null, 5)}</pre>
        </div>
      )}
    </div>
  );
};
