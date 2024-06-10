import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DownloadMaterialsSectionComponent } from './download-materials-section.component';

describe('DownloadMaterialsSectionComponent', () => {
  let component: DownloadMaterialsSectionComponent;
  let fixture: ComponentFixture<DownloadMaterialsSectionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DownloadMaterialsSectionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DownloadMaterialsSectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
