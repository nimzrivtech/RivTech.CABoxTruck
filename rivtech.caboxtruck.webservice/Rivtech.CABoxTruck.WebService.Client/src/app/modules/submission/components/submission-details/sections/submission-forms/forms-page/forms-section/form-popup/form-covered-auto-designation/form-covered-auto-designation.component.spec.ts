import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormCoveredAutoDesignationComponent } from './form-covered-auto-designation.component';

describe('FormCoveredAutoDesignationComponent', () => {
  let component: FormCoveredAutoDesignationComponent;
  let fixture: ComponentFixture<FormCoveredAutoDesignationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormCoveredAutoDesignationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormCoveredAutoDesignationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
