export interface CommandResultDto<T> {
    success: boolean;
    errorMessages: string[];
    messages: string[];
    data: T;
}

export interface IDropdownValue {
    key: any;
    value: string;
}

export type KeyValueDto = {
    key: string;
    value: string;
};

export interface DeviceBaseDto {
    id: string;
    deviceNodeId: string;
    deviceGroupId: string;
    deviceGroupTitle: string;
    deviceGroupLocation: string;
    title: string;
    deviceType: string;
    lastReportedValue: string;
    lastReportedTimeStamp?: string;
    trackEvents: boolean;
    deviceRaw: HansecDeviceChangedMessageDevice;
}

export interface DeviceGroupDto {
    id: string;
    nodeId: number;
    title: string;
    location: string;
    batteryReplacedDate?: Date;
    batteryLevel?: number;
    lastBatteryReport?: string;
    mainDevice: DeviceBaseDto;
    devices: DeviceBaseDto[];
}

export interface TransitionDto {
    label: string;
    uiButtonLabel: string;
    nextState: string;
    triggedByEmit: string;
    uiColor: string;
}
export interface StateDto {
    label: string;
    name: string;
    initial: boolean;
    armed: boolean;
    uiColor: string;
    alarmTriggeres: boolean;
}
export interface HeartbeatDto {
    action: string;
    state: StateDto;
    transitions: TransitionDto[];
    transition: TransitionDto;
    transitionSecondsRemaining: number;
    timeStamp: Date;
    openDoors: string[];
    motion: string[];
    fire: string[];
    binarySwitchesOn: string[];
}
export interface ControlBinarySwitchDto {
    deviceNodeId: string;
    on: boolean;
}
export interface RemoteOperationDto {
    stringToEmit: string;
    controlBinarySwitch: ControlBinarySwitchDto;
}

export interface DeviceDto {
    id: string;
    deviceNodeId: string;
    deviceGroupId: string;
    deviceGroupTitle: string;
    deviceGroupLocation: string;
    title: string;
    deviceType: string;
    lastReportedValue: string;
    lastReportedTimeStamp: Date;
    trackEvents: boolean;
}

export interface EventDto {
    eventId: string;
    eventType: string;
    device: DeviceDto;
    timeStamp: Date;
    deviceLevel: string;
    stateName: string;
    transitionName: string;
    uiColor: string;
}

export interface TemperatureDto {
    deviceId: number;
    title: string;
    location: string;
    value: string;
    units: string;
    lastReportedTimeStamp: Date;
}

export interface DeviceStatusDto {
    id: string;
    title: string;
    nodeId: number;
    mainType: string;
    devices: number;
    location: string;
    hansecId: number;
    lastReportedTimeStamp: Date;
    lastReportedValue: string;
    hasBattery: boolean;
    batteryLevel: number;
    batteryLastReport: Date;
    batteryReplaced: Date;
}

export interface HansecDeviceChangedMessageDeviceMetrics {
    probeTitle: string;
    probeType: string;
    scaleTitle: string;
    icon: string;
    level: string;
    title: string;
}

export interface HansecDeviceChangedMessageDevice {
    creationTime: number;
    deviceType: string;
    h: number;
    id: string;
    location: number;
    metrics: HansecDeviceChangedMessageDeviceMetrics;
    permanently_Hidden: boolean;
    probeType: string;
    tags: string[];
    visibility: boolean;
    updateTime: number;
}

export interface HansecDeviceChangedMessage {
    action: string;
    device: HansecDeviceChangedMessageDevice;
    timeStamp: string;
    tenantId: string;
}
export interface SwitchDto extends DeviceDto {
    pendingOn: boolean;
    pendingOff: boolean;
    timeout: NodeJS.Timer;
}

export interface EventLogDto {
    eventId: string;
    timeStamp: string;
    eventType: string;
    stateName: string;
    transitionName: string;
    userName: string;
    deviceId: string;
    device: DeviceBaseDto;
    deviceGroupId: string;
    deviceGroup: DeviceGroupDto;
    deviceLevel: string;
    uIColor: string;
}
