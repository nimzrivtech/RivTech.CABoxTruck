import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UnderWritingComponent } from './underwriting.component';

describe('UnderWritingComponent', () => {
  let component: UnderWritingComponent;
  let fixture: ComponentFixture<UnderWritingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UnderWritingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UnderWritingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
