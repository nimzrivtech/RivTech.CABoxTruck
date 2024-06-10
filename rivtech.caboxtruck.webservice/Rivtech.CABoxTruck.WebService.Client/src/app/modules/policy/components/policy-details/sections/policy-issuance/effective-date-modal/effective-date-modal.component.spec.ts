import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EffectiveDateModalComponent } from './effective-date-modal.component';

describe('EffectiveDateModalComponent', () => {
  let component: EffectiveDateModalComponent;
  let fixture: ComponentFixture<EffectiveDateModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EffectiveDateModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EffectiveDateModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
