import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { QuickReferenceSectionComponent } from './quick-reference-section.component';

describe('QuickReferenceSectionComponent', () => {
  let component: QuickReferenceSectionComponent;
  let fixture: ComponentFixture<QuickReferenceSectionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ QuickReferenceSectionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(QuickReferenceSectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
