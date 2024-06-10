import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TaskSearchFilterComponent } from './task-search-filter.component';

describe('TaskSearchFilterComponent', () => {
  let component: TaskSearchFilterComponent;
  let fixture: ComponentFixture<TaskSearchFilterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TaskSearchFilterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TaskSearchFilterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
