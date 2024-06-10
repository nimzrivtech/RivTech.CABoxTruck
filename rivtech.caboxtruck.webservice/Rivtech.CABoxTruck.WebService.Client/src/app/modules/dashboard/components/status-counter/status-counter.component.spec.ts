import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StatusCounterComponent } from './status-counter.component';

describe('StatusCounterComponent', () => {
  let component: StatusCounterComponent;
  let fixture: ComponentFixture<StatusCounterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StatusCounterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StatusCounterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
