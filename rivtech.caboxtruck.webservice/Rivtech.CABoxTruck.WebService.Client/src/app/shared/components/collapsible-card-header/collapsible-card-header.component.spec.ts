import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CollapsibleCardHeaderComponent } from './collapsible-card-header.component';

describe('CollapsibleCardHeaderComponent', () => {
  let component: CollapsibleCardHeaderComponent;
  let fixture: ComponentFixture<CollapsibleCardHeaderComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CollapsibleCardHeaderComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CollapsibleCardHeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
