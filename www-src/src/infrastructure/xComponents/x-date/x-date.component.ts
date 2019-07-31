import { Component, OnInit, Input, Output, EventEmitter, ChangeDetectorRef, AfterViewChecked } from '@angular/core';
import { FormControl, FormGroup, Validators, FormArray, FormBuilder } from '@angular/forms';
import { formGroupNameProvider } from '@angular/forms/src/directives/reactive_directives/form_group_name';

@Component({
  selector: 'app-x-date',
  templateUrl: './x-date.component.html',
  styleUrls: ['./x-date.component.css']
})
export class XDateComponent implements OnInit, AfterViewChecked {

  @Input() name = '';
  @Input() value: any;
  @Input() type = 'text';
  @Input() autocomplete = 'off';
  @Input() label = '';
  @Input() placeholder = '';
  @Input() required = false;
  @Input() formGroup: FormGroup;
  @Input() noPadding = false;
  @Input() disabled = false;
  @Input() tech = 'Bootstrap'; // Material, Bootstrap

  formControl: FormControl;
  mainObject: any;

  constructor(private cdRef: ChangeDetectorRef) {

  }

  ngOnInit() {

    this.mainObject = this.value;
    this.value = this.value[this.name];

    const validators = [];
    if (this.required) {
      validators.push(Validators.required);
    }

    this.formControl = new FormControl(this.value, validators);

    this.formGroup.addControl(this.name, this.formControl);

    this.formControl.valueChanges.subscribe(val => {
      this.mainObject[this.name] = val;
    });

  }

  ngAfterViewChecked() {
    // https://stackoverflow.com/questions/39787038/how-to-manage-angular2-expression-has-changed-after-it-was-checked-exception-w
    this.cdRef.detectChanges();
  }


  getErrorMessage() {
    return this.formControl.hasError('required') ? 'This is a required field' : '';
  }

}
