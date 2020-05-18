import { IDropdownValue } from "src/infrastructure/models";

export interface IUser {
    id: string;
    fullName: string;
    userName: string;
    tenantName: string;
    culture: IDropdownValue;
    password?: string;
    passwordRepeat?: string;

}
export interface IRole {
    name: string;
    selected: boolean;
}
