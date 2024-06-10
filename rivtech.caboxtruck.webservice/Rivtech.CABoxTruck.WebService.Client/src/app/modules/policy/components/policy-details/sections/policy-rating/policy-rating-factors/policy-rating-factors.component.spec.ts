import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PolicyRatingFactorsComponent } from './policy-rating-factors.component';

describe('PolicyRatingFactorsComponent', () => {
  let component: PolicyRatingFactorsComponent;
  let fixture: ComponentFixture<PolicyRatingFactorsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PolicyRatingFactorsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PolicyRatingFactorsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
