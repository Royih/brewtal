export interface PidStatus {
  pidId: number;
  pidName: string;
  targetTemp: number;
  currentTemp: number;
  output: boolean;
  outputValue: number;
  fridgeMode: boolean;
  minTemp: number;
  maxTemp: number;
  minTempTimeStamp: Date;
  maxTempTimeStamp: Date;
  errorSum: number;
  rpiCoreTemp: string;
}

export interface PidConfig {
  pIDKp: number;
  pIDKi: number;
  pIDKd: number;
}

export interface ManualOutput {
  output: number;
  name: string;
  value: boolean;
  automatic: boolean;
}

export type HardwareStatus = {
  pid: PidStatus;
  computedTime: Date;
  manualOutputs: ManualOutput[];
  pidConfig: PidConfig;
};
