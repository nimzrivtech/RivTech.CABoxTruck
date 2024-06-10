import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { KeyManagementPersonnelInfoSectionComponent } from './key-management-personnel-info-section.component';

describe('KeyManagementPersonnelInfoSectionComponent', () => {
  let component: KeyManagementPersonnelInfoSectionComponent;
  let fixture: ComponentFixture<KeyManagementPersonnelInfoSectionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ KeyManagementPersonnelInfoSectionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(KeyManagementPersonnelInfoSectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
