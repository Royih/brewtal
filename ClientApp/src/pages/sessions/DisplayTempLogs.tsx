import { DataGrid, GridColDef, GridValueGetterParams } from "@material-ui/data-grid";
import { TemplogDto } from "./models";

function DisplayTempLogs(props: { logs: TemplogDto[] }) {
  function getActTemp(params: GridValueGetterParams) {
    const tempAsNumber = (params.getValue(params.id, "actualTemperature") as number) || 0;
    return (Math.round(tempAsNumber * 100) / 100).toFixed(2) + "°C" || "";
  }

  const columns: GridColDef[] = [
    { field: "id", headerName: "ID", width: 90 },
    { field: "timeStamp", headerName: "Time", width: 200 },
    { field: "actualTemperature", headerName: "Temp", width: 200 },
    {
      field: "actualTemperature2",
      headerName: "Temperature",
      type: "number",
      width: 200,
      valueGetter: getActTemp,
      sortComparator: (v1, v2, param1, param2) => param1.api.getCellValue(param1.id, "actualTemperature") - param2.api.getCellValue(param2.id, "actualTemperature"),
    },
  ];

  // const [logs, setLogs] = useState(props.logs);

  // const [sortModel, setSortModel] = useState<GridSortModel>([
  //   {
  //     field: "id",
  //     sort: "desc",
  //   },
  // ]);

  return (
    <div style={{ height: "300px", width: "100%" }}>
      <DataGrid
        rows={props.logs}
        columns={columns}
        pageSize={100}
        // sortModel={sortModel}
        // onSortModelChange={(model) => {
        //   console.log("Her...");
        //   setSortModel(model);
        // }}
      />
    </div>
  );
}
export default DisplayTempLogs;
