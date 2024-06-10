import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ToggleCollapseComponent } from './toggle-collapse.component';

describe('ToggleCollapseComponent', () => {
  let component: ToggleCollapseComponent;
  let fixture: ComponentFixture<ToggleCollapseComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ToggleCollapseComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ToggleCollapseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
