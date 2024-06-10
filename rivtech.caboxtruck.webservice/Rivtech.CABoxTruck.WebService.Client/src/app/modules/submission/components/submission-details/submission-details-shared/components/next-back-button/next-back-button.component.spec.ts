import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NextBackButtonComponent } from './next-back-button.component';

describe('NextBackButtonComponent', () => {
  let component: NextBackButtonComponent;
  let fixture: ComponentFixture<NextBackButtonComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NextBackButtonComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NextBackButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
