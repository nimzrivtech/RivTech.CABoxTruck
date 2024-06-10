import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddUpdateBindingComponent } from './add-update-binding.component';

describe('AddUpdateBindingComponent', () => {
  let component: AddUpdateBindingComponent;
  let fixture: ComponentFixture<AddUpdateBindingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [AddUpdateBindingComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddUpdateBindingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
