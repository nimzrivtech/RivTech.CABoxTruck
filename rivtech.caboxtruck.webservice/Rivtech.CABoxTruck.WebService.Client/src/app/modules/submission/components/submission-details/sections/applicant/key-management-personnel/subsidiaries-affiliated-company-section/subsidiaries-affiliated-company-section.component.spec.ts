import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SubsidiariesAffiliatedCompanySectionComponent } from './subsidiaries-affiliated-company-section.component';

describe('SubsidiariesAffiliatedCompanySectionComponent', () => {
  let component: SubsidiariesAffiliatedCompanySectionComponent;
  let fixture: ComponentFixture<SubsidiariesAffiliatedCompanySectionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SubsidiariesAffiliatedCompanySectionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SubsidiariesAffiliatedCompanySectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
