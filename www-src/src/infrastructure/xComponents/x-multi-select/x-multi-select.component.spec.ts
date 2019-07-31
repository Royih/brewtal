import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { XMultiSelectComponent } from './x-multi-select.component';

describe('XSelectComponent', () => {
  let component: XMultiSelectComponent;
  let fixture: ComponentFixture<XMultiSelectComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ XMultiSelectComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(XMultiSelectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
