export interface LogRecordDto {
    id: number;
    timeStamp: Date;
    targetTemp1: number;
    actualTemp1: number;
    output1: boolean;
    targetTemp2: number;
    actualTemp2: number;
    output2: boolean;
}
