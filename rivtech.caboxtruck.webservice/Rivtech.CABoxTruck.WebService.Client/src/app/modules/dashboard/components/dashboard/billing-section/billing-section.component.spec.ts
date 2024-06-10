import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BillingSectionComponent } from './billing-section.component';

describe('BillingSectionComponent', () => {
  let component: BillingSectionComponent;
  let fixture: ComponentFixture<BillingSectionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BillingSectionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BillingSectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
