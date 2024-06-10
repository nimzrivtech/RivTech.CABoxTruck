import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DocumentsSearchFilterComponent } from './documents-search-filter.component';

describe('DocumentsSearchFilterComponent', () => {
  let component: DocumentsSearchFilterComponent;
  let fixture: ComponentFixture<DocumentsSearchFilterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DocumentsSearchFilterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DocumentsSearchFilterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
