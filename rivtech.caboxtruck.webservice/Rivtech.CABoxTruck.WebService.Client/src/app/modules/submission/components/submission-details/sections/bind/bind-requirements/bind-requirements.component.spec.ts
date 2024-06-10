import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BindRequirementsComponent } from './bind-requirements.component';

describe('BindRequirementsComponent', () => {
  let component: BindRequirementsComponent;
  let fixture: ComponentFixture<BindRequirementsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BindRequirementsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BindRequirementsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
