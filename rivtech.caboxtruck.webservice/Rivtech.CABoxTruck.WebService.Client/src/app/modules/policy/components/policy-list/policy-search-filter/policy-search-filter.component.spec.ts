import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PolicySearchFilterComponent } from './policy-search-filter.component';

describe('PolicySearchFilterComponent', () => {
  let component: PolicySearchFilterComponent;
  let fixture: ComponentFixture<PolicySearchFilterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PolicySearchFilterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PolicySearchFilterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
