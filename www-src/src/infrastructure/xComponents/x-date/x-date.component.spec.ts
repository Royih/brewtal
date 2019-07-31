import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { XDateComponent } from './x-date.component';

describe('XInputComponent', () => {
  let component: XDateComponent;
  let fixture: ComponentFixture<XDateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ XDateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(XDateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
