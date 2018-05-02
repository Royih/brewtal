export interface PidStatusDto {
    pidId: number;
    pidName: string;
    targetTemp: number;
    currentTemp: number;
    output: boolean;
    outputValue: number;
}
