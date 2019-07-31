import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { XInputComponent } from './x-input.component';

describe('XInputComponent', () => {
  let component: XInputComponent;
  let fixture: ComponentFixture<XInputComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ XInputComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(XInputComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
