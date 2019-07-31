import { FormGroup, FormArray } from '@angular/forms';

interface objAndFormGroup<T> {
  obj: T;
  formGroup: FormGroup;
}

export class XItemGroup<T> {

  myFormArray = new FormArray([]);
  items: objAndFormGroup<T>[];

  constructor(mainForm: FormGroup, itemPropertyName: string = "items") {
    this.items = [];
    mainForm.addControl(itemPropertyName, this.myFormArray);
  }

  public add(myObject: T) {
    var fg = new FormGroup({});
    this.items.push({ obj: myObject, formGroup: fg });
    this.myFormArray.push(fg);
  }


}
