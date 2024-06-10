import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { KeyManagementPersonnelComponent } from './key-management-personnel.component';

describe('KeyManagementPersonnelComponent', () => {
  let component: KeyManagementPersonnelComponent;
  let fixture: ComponentFixture<KeyManagementPersonnelComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ KeyManagementPersonnelComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(KeyManagementPersonnelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
