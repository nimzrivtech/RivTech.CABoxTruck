import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewsInfoSectionComponent } from './news-info-section.component';

describe('NewsInfoSectionComponent', () => {
  let component: NewsInfoSectionComponent;
  let fixture: ComponentFixture<NewsInfoSectionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewsInfoSectionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewsInfoSectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
