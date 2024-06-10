import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PolicyRatingComponent } from './policy-rating.component';

describe('PolicyRatingComponent', () => {
  let component: PolicyRatingComponent;
  let fixture: ComponentFixture<PolicyRatingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PolicyRatingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PolicyRatingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
