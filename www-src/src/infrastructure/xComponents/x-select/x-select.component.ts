import { Component, OnInit, Input, Output, EventEmitter, SimpleChanges, ChangeDetectorRef } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Observable, of, Subscription } from 'rxjs';
import { XDataFetcher } from '../utils';

@Component({
  selector: 'app-x-select',
  templateUrl: './x-select.component.html',
  styleUrls: ['./x-select.component.css']
})
export class XSelectComponent implements OnInit {

  @Input() name = '';
  @Input() value: any;
  @Input() labelField: 'name';
  @Input() placeholder = '';
  @Input() required = false;
  @Input() formGroup: FormGroup;
  @Input() getOptions: XDataFetcher<any>;
  @Input() disabled = false;
  @Input() noPadding = false;

  formControl: FormControl;
  optionsLoaded = false;
  options: any[] = [];

  constructor(private cdRef: ChangeDetectorRef) {

  }

  ngOnInit() {

    const validators = [];
    if (this.required) {
      validators.push(Validators.required);
    }

    this.formControl = new FormControl({ value: this.value, disabled: this.disabled }, validators);
    this.formGroup.addControl(this.name, this.formControl);

    if (this.value) {
      this.options.push(this.value);
    }

  }

  ngAfterViewChecked() {
    // https://stackoverflow.com/questions/39787038/how-to-manage-angular2-expression-has-changed-after-it-was-checked-exception-w
    this.cdRef.detectChanges();
  }

  getErrorMessage() {
    return this.formControl.hasError('required') ? 'This is a required field' : '';
  }

  loadOptions(isOpen: boolean) {
    if (isOpen && !this.optionsLoaded) {
      this.getOptions.getOptions()
        .subscribe(data => {
          this.optionsLoaded = true;
          this.options = data;
        });
    }
  }

  select(option) {
    this.formControl.setValue(option);
  }

  isSelected = (val1: any, val2: any) => val1 && val2 && val1[this.labelField] == val2[this.labelField];

  ngOnChanges(changes: SimpleChanges) {
    if (this.formControl) {
      if (changes.disabled) {
        if (changes.disabled.currentValue) {
          this.formControl.disable();
        } else {
          this.formControl.enable();
        }

      }
    }


  }

}
