export interface PidStatusDto {
    pidId: number;
    pidName: string;
    targetTemp: number;
    currentTemp: number;
    output: boolean;
    outputValue: number;
}

export interface PidStatusesDto {
    pids: PidStatusDto[];
    computedTime: Date;
    loggingToName: string;
}
