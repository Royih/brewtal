import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { XConfirmDialogComponent } from './x-confirm-dialog.component';

describe('XConfirmDialogComponent', () => {
  let component: XConfirmDialogComponent;
  let fixture: ComponentFixture<XConfirmDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ XConfirmDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(XConfirmDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
