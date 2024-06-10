import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UwDashboardComponent } from './uw-dashboard.component';

describe('UwDashboardComponent', () => {
  let component: UwDashboardComponent;
  let fixture: ComponentFixture<UwDashboardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UwDashboardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UwDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
