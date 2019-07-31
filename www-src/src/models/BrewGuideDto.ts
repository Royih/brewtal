
export interface BrewDto {
    id: number;
    batchNumber: number;
    initiated: string;
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
}

export interface StepDto {
    order: number;
    id: number;
    name: string;
    startTime: string;
    completeTime?: string;
    targetMashTemp: number;
    targetSpargeTemp: number;
    completeButtonText: string;
    instructions: string;
    showTimer: boolean;
}

export interface BrewGuideDto {
    setup: BrewDto;
    currentStep: StepDto;
}
