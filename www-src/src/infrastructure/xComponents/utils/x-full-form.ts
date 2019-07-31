import { Observable, of } from "rxjs";
import { Injectable } from "@angular/core";


export interface XFullFormInterface {
    saveData(): Promise<boolean>;
    loadData(): Promise<boolean>;
}

export interface FullForm<T> {
    saveData(component: T): Promise<boolean>;
    loadData(component: T): Promise<boolean>;
}

@Injectable()
export class XMyFullForm implements FullForm<XFullFormInterface> {

    saveData(component: XFullFormInterface): Promise<boolean> {
        if (component.saveData) {
            return component.saveData().then(res => {
                return true;
            })
        }
        return of(true).toPromise();
    }

    loadData(component: XFullFormInterface): Promise<boolean> {
        if (component.loadData) {
            return component.loadData().then(res => {
                return true;
            })
        }
        return of(true).toPromise();
    }

    constructor() {

    }



} 