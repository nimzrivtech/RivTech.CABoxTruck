import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GenericAddressComponent } from './generic-address.component';

describe('GenericAddressComponent', () => {
  let component: GenericAddressComponent;
  let fixture: ComponentFixture<GenericAddressComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GenericAddressComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GenericAddressComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
