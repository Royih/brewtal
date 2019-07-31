import { Component, OnInit, Input, Output, EventEmitter, ChangeDetectorRef, AfterViewChecked } from '@angular/core';
import { FormControl, FormGroup, Validators, FormArray, FormBuilder } from '@angular/forms';
import { formGroupNameProvider } from '@angular/forms/src/directives/reactive_directives/form_group_name';

export interface InputProperties {
  maxLength: number;
  minLength: number;
  max: number;
  min: number;
  step: number;
}


@Component({
  selector: 'app-x-input',
  templateUrl: './x-input.component.html',
  styleUrls: ['./x-input.component.css']
})
export class XInputComponent implements OnInit, AfterViewChecked {

  @Input() name = '';
  @Input() value: any;
  @Input() type = 'text';
  @Input() autocomplete = 'off';
  @Input() properties: InputProperties = null;
  @Input() label = '';
  @Input() placeholder = '';
  @Input() postfix = '';
  @Input() multiLine: false;
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
    if (this.type === 'email') {
      validators.push(Validators.email);
    }
    if (this.type === 'text' && this.properties && this.properties.minLength) {
      validators.push(Validators.minLength(this.properties.minLength));
    }
    if (this.type === 'text' && this.properties && this.properties.maxLength) {
      validators.push(Validators.maxLength(this.properties.maxLength));
    }
    if (this.type === 'number' && this.properties && this.properties.min) {
      validators.push(Validators.min(this.properties.min));
    }
    if (this.type === 'number' && this.properties && this.properties.max) {
      validators.push(Validators.max(this.properties.max));
    }

    this.formControl = new FormControl(this.value, validators);

    this.formGroup.addControl(this.name, this.formControl);

    this.formControl.valueChanges.subscribe(val => {
      if (this.type === 'number') {
        this.mainObject[this.name] = +val;
      } else {
        this.mainObject[this.name] = val;
      }

    });

  }

  ngAfterViewChecked() {
    // https://stackoverflow.com/questions/39787038/how-to-manage-angular2-expression-has-changed-after-it-was-checked-exception-w
    this.cdRef.detectChanges();
  }

  getMinMaxLengthErrorMessage() {
    if (this.properties && this.properties.maxLength && this.properties.minLength) {
      return `Input must be between ${this.properties.minLength} and ${this.properties.maxLength} characters. `;
    } else if (this.properties && this.properties.minLength) {
      return `Input must be more than ${this.properties.minLength} characters. `;
    } else if (this.properties) {
      return `Input must be less than ${this.properties.maxLength} characters. `;
    } else {
      return null;
    }
  }

  getMinMaxErrorMessage() {
    if (this.properties && this.properties.max && this.properties.min) {
      return `Input must be between ${this.properties.min} and ${this.properties.max}. `;
    } else if (this.properties && this.properties.min) {
      return `Input must be more than ${this.properties.min}. `;
    } else if (this.properties) {
      return `Input must be less than ${this.properties.max}. `;
    } else {
      return null;
    }
  }

  getErrorMessage() {
    return this.formControl.hasError('required') ? 'This is a required field' :
      this.formControl.hasError('email') ? 'Not a valid email' :
        (this.formControl.hasError('minlength') || this.formControl.hasError('maxlength')) ? this.getMinMaxLengthErrorMessage() :
          (this.formControl.hasError('min') || this.formControl.hasError('max')) ? this.getMinMaxErrorMessage() :
            '';
  }

}
