import { PidStatusDto } from './PidStatusesDto';
import { ManualOutputDto } from '.';

export interface HardwareStatusDto {
    pids: PidStatusDto[];
    computedTime: Date;
    manualOutputs: ManualOutputDto[];
}
