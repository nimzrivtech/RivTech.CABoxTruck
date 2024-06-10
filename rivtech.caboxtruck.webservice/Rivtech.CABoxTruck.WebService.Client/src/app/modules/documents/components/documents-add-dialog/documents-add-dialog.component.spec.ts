import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DocumentsAddDialogComponent } from './documents-add-dialog.component';

describe('DocumentsAddDialogComponent', () => {
  let component: DocumentsAddDialogComponent;
  let fixture: ComponentFixture<DocumentsAddDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DocumentsAddDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DocumentsAddDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
