import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NotesSectionComponent } from './notes-section.component';

describe('NotesSectionComponent', () => {
  let component: NotesSectionComponent;
  let fixture: ComponentFixture<NotesSectionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NotesSectionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NotesSectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
