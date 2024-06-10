import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClaimsHistorySectionComponent } from './claims-history-section.component';

describe('LiabilityClaimsInfoComponent', () => {
  let component: ClaimsHistorySectionComponent;
  let fixture: ComponentFixture<ClaimsHistorySectionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ClaimsHistorySectionComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClaimsHistorySectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
