import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { XSelectComponent } from './x-select.component';

describe('XSelectComponent', () => {
  let component: XSelectComponent;
  let fixture: ComponentFixture<XSelectComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ XSelectComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(XSelectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
