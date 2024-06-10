import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RiskSpecificsComponent } from './risk-specifics.component';

describe('RiskSpecificsComponent', () => {
  let component: RiskSpecificsComponent;
  let fixture: ComponentFixture<RiskSpecificsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RiskSpecificsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RiskSpecificsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
