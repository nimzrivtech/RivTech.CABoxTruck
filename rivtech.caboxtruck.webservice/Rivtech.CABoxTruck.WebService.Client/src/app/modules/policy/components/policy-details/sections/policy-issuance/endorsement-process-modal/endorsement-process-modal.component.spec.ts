import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EndorsementProcessModalComponent } from './endorsement-process-modal.component';

describe('EndorsementProcessModalComponent', () => {
  let component: EndorsementProcessModalComponent;
  let fixture: ComponentFixture<EndorsementProcessModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EndorsementProcessModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EndorsementProcessModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
