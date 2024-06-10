import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { XpRaterUpdateAlertComponent } from './xp-rater-update-alert.component';

describe('XpRaterUpdateAlertComponent', () => {
  let component: XpRaterUpdateAlertComponent;
  let fixture: ComponentFixture<XpRaterUpdateAlertComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ XpRaterUpdateAlertComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(XpRaterUpdateAlertComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
