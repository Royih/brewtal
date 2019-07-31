import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';


export interface DialogData {
  title: string;
  message: string;
  optionTrueLabelNotDefault: string;
  optionFalseLabelDefault: string;
}

@Component({
  selector: 'app-confirm-dialog',
  templateUrl: './x-confirm-dialog.component.html',
  styleUrls: ['./x-confirm-dialog.component.css']
})
export class XConfirmDialogComponent {

  constructor(
    public dialogRef: MatDialogRef<XConfirmDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData) { }


}
