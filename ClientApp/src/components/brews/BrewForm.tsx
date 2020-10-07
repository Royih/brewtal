import React, { useState, useEffect, useCallback } from "react";
import { Card, CardHeader, CardContent, Paper, TextField, makeStyles, createStyles, Theme } from "@material-ui/core";
import { Brew } from "./Models";
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
  onChange(brew?: Brew): void;
}

export const BrewForm = (props: IProps) => {
  const classes = useStyles();
  const [brew, setBrew] = useState<Brew>(props.brew);

  const validatorElements = [];
  validatorElements.push(new ValidationElement("name", brew.name, { required: true, minLength: 2, maxLength: 100 }));
  validatorElements.push(new ValidationElement("batchNumber", brew.batchNumber, { required: true }));
  validatorElements.push(new ValidationElement("beginMash", brew.beginMash, { required: true }));
  validatorElements.push(new ValidationElement("mashTemp", brew.mashTemp, { required: true, min: 30, max: 75 }));
  validatorElements.push(new ValidationElement("strikeTemp", brew.strikeTemp, { required: true, min: 30, max: 77 }));
  validatorElements.push(new ValidationElement("spargeTemp", brew.spargeTemp, { required: true, min: 30, max: 100 }));
  validatorElements.push(new ValidationElement("mashOutTemp", brew.mashOutTemp, { required: true, min: 50, max: 88 }));
  validatorElements.push(new ValidationElement("mashTimeInMinutes", brew.mashTimeInMinutes, { required: true, min: 30, max: 120 }));
  validatorElements.push(new ValidationElement("boilTimeInMinutes", brew.boilTimeInMinutes, { required: true, min: 30, max: 120 }));
  validatorElements.push(new ValidationElement("batchSize", brew.batchSize, { required: true, min: 20, max: 50 }));
  validatorElements.push(new ValidationElement("mashWaterAmount", brew.mashWaterAmount, { required: true, min: 10, max: 50 }));
  validatorElements.push(new ValidationElement("spargeWaterAmount", brew.spargeWaterAmount, { required: true, min: 10, max: 30 }));

  const [validator, setValidator] = useState(new FormValidator(validatorElements));

  const handleChange = (key: string, inputElement: HTMLInputElement | HTMLTextAreaElement) => {
    const newValue = inputElement.type === "number" ? +inputElement.value : inputElement.value;
    const newState = { ...brew, [key]: newValue } as Brew;
    setBrew(newState);
    setValidator(validator.updateValue(key, newValue));
  };

  const handleBlur = (name: string) => {
    setValidator(validator.onBlur(name));
  };

  const { onChange } = props;
  const propsOnChange = useCallback(onChange, []);

  useEffect(() => {
    propsOnChange(validator.allowSave ? brew : undefined);
  }, [validator, brew, propsOnChange]);

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
                  value={brew.name || ""}
                  disabled={false}
                  required={true}
                  autoComplete="off"
                  error={validator.reportError("name")}
                  helperText={validator.errorMessage("name")}
                  onChange={(event) => handleChange("name", event.target)}
                  className={classes.margin}
                  onBlur={() => handleBlur("name")}
                />
                <TextField
                  id="details-batch-number"
                  label="Batch #"
                  value={brew.batchNumber || 0}
                  disabled={false}
                  required={true}
                  autoComplete="off"
                  error={validator.reportError("batchNumber")}
                  helperText={validator.errorMessage("batchNumber")}
                  onChange={(event) => handleChange("batchNumber", event.target)}
                  className={classes.margin}
                  onBlur={() => handleBlur("batchNumber")}
                  type="number"
                />
                <TextField
                  id="details-mash-start"
                  label="Mash start"
                  type="datetime-local"
                  className={classes.margin}
                  required={true}
                  value={brew.beginMash.toString().substr(0, 16)}
                  error={validator.reportError("beginMash")}
                  helperText={validator.errorMessage("beginMash")}
                  onChange={(event) => handleChange("beginMash", event.target)}
                  onBlur={() => handleBlur("beginMash")}
                  InputLabelProps={{
                    shrink: true,
                  }}
                />
                <TextField
                  id="details-mash-temp"
                  label="Mash temp"
                  value={brew.mashTemp || 0}
                  required={true}
                  autoComplete="off"
                  error={validator.reportError("mashTemp")}
                  helperText={validator.errorMessage("mashTemp")}
                  onChange={(event) => handleChange("mashTemp", event.target)}
                  className={classes.margin}
                  onBlur={() => handleBlur("mashTemp")}
                  type="number"
                  inputProps={{ step: 0.1 }}
                />
                <TextField
                  id="details-strike-temp"
                  label="Strike temp"
                  value={brew.strikeTemp || ""}
                  required={true}
                  autoComplete="off"
                  error={validator.reportError("strikeTemp")}
                  helperText={validator.errorMessage("strikeTemp")}
                  onChange={(event) => handleChange("strikeTemp", event.target)}
                  className={classes.margin}
                  onBlur={() => handleBlur("strikeTemp")}
                  type="number"
                  inputProps={{ step: 0.1 }}
                />
                <TextField
                  id="details-sparge-temp"
                  label="Sparge temp"
                  value={brew.spargeTemp || 0}
                  required={true}
                  autoComplete="off"
                  error={validator.reportError("spargeTemp")}
                  helperText={validator.errorMessage("spargeTemp")}
                  onChange={(event) => handleChange("spargeTemp", event.target)}
                  className={classes.margin}
                  onBlur={() => handleBlur("spargeTemp")}
                  type="number"
                  inputProps={{ step: 0.1 }}
                />
                <TextField
                  id="details-mash-out-temp"
                  label="Mash out temp"
                  value={brew.mashOutTemp || 0}
                  required={true}
                  autoComplete="off"
                  error={validator.reportError("mashOutTemp")}
                  helperText={validator.errorMessage("mashOutTemp")}
                  onChange={(event) => handleChange("mashOutTemp", event.target)}
                  className={classes.margin}
                  onBlur={() => handleBlur("mashOutTemp")}
                  type="number"
                  inputProps={{ step: 0.1 }}
                />
                <TextField
                  id="details-mash-time"
                  label="Mash time"
                  value={brew.mashTimeInMinutes || 0}
                  required={true}
                  autoComplete="off"
                  error={validator.reportError("mashTimeInMinutes")}
                  helperText={validator.errorMessage("mashTimeInMinutes")}
                  onChange={(event) => handleChange("mashTimeInMinutes", event.target)}
                  className={classes.margin}
                  onBlur={() => handleBlur("mashTimeInMinutes")}
                  type="number"
                />
                <TextField
                  id="details-mash-time"
                  label="Boil time"
                  value={brew.boilTimeInMinutes || 0}
                  required={true}
                  autoComplete="off"
                  error={validator.reportError("boilTimeInMinutes")}
                  helperText={validator.errorMessage("boilTimeInMinutes")}
                  onChange={(event) => handleChange("boilTimeInMinutes", event.target)}
                  className={classes.margin}
                  onBlur={() => handleBlur("boilTimeInMinutes")}
                  type="number"
                />
                <TextField
                  id="details-batch-size"
                  label="Batch size"
                  value={brew.batchSize || 0}
                  required={true}
                  autoComplete="off"
                  error={validator.reportError("batchSize")}
                  helperText={validator.errorMessage("batchSize")}
                  onChange={(event) => handleChange("batchSize", event.target)}
                  className={classes.margin}
                  onBlur={() => handleBlur("batchSize")}
                  type="number"
                />
                <TextField
                  id="details-mash-water-amount"
                  label="Mash water amount"
                  value={brew.mashWaterAmount || 0}
                  required={true}
                  autoComplete="off"
                  error={validator.reportError("mashWaterAmount")}
                  helperText={validator.errorMessage("mashWaterAmount")}
                  onChange={(event) => handleChange("mashWaterAmount", event.target)}
                  className={classes.margin}
                  onBlur={() => handleBlur("mashWaterAmount")}
                  type="number"
                  inputProps={{ step: 0.1 }}
                />
                <TextField
                  id="details-sparge-water-amount"
                  label="Sparge water amount"
                  value={brew.spargeWaterAmount || 0}
                  required={true}
                  autoComplete="off"
                  error={validator.reportError("spargeWaterAmount")}
                  helperText={validator.errorMessage("spargeWaterAmount")}
                  onChange={(event) => handleChange("spargeWaterAmount", event.target)}
                  className={classes.margin}
                  onBlur={() => handleBlur("spargeWaterAmount")}
                  type="number"
                  inputProps={{ step: 0.1 }}
                />
              </form>

              {/* <pre>{JSON.stringify(brew, null, 5)}</pre> */}
            </CardContent>
          </Card>
        </Paper>
      )}
    </div>
  );
};
