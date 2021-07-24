export interface RuntimeDto {
  id: number;
  currentSessionId: number | null;
  currentSession: SessionDto;
  startup: string;
}

export interface TemplogDto {
  id: number;
  sessionId: number;
  timeStamp: string;
  timeStampAsTicks: number;
  actualTemperature: number;
  session: SessionDto;
}

export interface SessionDto {
  id: number;
  startTime: string;
  targetTemp: number;
  minTemp: number;
  maxTemp: number;
  timeToReachTarget: string | null;
  logs: TemplogDto[];
}

export interface SessionLightDto {
  id: number;
  startTime: string;
  targetTemp: number;
}
