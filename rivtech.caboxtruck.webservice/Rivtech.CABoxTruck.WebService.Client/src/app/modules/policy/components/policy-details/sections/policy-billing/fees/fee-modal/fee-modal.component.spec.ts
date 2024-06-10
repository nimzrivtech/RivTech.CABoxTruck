import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FeeModalComponent } from './fee-modal.component';

describe('FeeModalComponent', () => {
  let component: FeeModalComponent;
  let fixture: ComponentFixture<FeeModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FeeModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FeeModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
