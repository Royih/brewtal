import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { XCheckComponent } from './x-check.component';

describe('XInputComponent', () => {
  let component: XCheckComponent;
  let fixture: ComponentFixture<XCheckComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ XCheckComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(XCheckComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
