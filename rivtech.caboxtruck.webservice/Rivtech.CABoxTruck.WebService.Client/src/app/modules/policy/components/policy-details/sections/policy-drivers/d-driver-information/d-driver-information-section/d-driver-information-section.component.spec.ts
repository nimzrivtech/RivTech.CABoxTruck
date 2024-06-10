import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DDriverInformationSectionComponent } from './d-driver-information-section.component';

describe('DDriverInformationSectionComponent', () => {
  let component: DDriverInformationSectionComponent;
  let fixture: ComponentFixture<DDriverInformationSectionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DDriverInformationSectionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DDriverInformationSectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
