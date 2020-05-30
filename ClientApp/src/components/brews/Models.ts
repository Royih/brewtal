export interface Brew {
    id: string;
    batchNumber: number;
    initiated: Date;
    beginMash: Date;
    name: string;
    mashTemp: number;
    strikeTemp: number;
    spargeTemp: number;
    mashOutTemp: number;
    mashTimeInMinutes: number;
    boilTimeInMinutes: number;
    batchSize: number;
    mashWaterAmount: number;
    spargeWaterAmount: number;
    notes: string;
    shoppingList: string;
    optimisticConcurrencyKey: string;
    steps: BrewStep[];
}

export interface BrewStep {
    order: number;
    name: string;
    startTime: Date;
    completeTime?: Date;
    targetMashTemp: number;
    targetSpargeTemp: number;
    completeButtonText: string;
    instructions: string;
    showTimer: boolean;
    floatValues: DataCaptureFloatValue[];
    intValues: DataCaptureIntValue[];
    stringValues: DataCaptureStringValue[];
}

export interface DataCaptureFloatValue {
    label: string;
    optional: boolean;
    value?: number;
    units: string;
}

export interface DataCaptureIntValue {
    label: string;
    optional: boolean;
    value?: number;
    units: string;
}

export interface DataCaptureStringValue {
    label: string;
    optional: boolean;
    value: string;
    units: string;
}