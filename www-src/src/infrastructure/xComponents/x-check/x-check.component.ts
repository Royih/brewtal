import { Component, OnInit, Input, Output, EventEmitter, SimpleChange, SimpleChanges, OnChanges } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-x-check',
  templateUrl: './x-check.component.html',
  styleUrls: ['./x-check.component.css']
})
export class XCheckComponent implements OnInit, OnChanges {

  @Input() name = '';
  @Input() value = false;
  @Input() disabled = false;
  @Input() placeholder = '';
  @Input() formGroup: FormGroup;

  formControl: FormControl;

  constructor() {


  }

  ngOnInit() {

    this.formControl = new FormControl({ value: this.value, disabled: this.disabled });
    this.formGroup.addControl(this.name, this.formControl);

  }

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
