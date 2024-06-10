import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DDriverInformationComponent } from './d-driver-information.component';

describe('DDriverInformationComponent', () => {
  let component: DDriverInformationComponent;
  let fixture: ComponentFixture<DDriverInformationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DDriverInformationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DDriverInformationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
