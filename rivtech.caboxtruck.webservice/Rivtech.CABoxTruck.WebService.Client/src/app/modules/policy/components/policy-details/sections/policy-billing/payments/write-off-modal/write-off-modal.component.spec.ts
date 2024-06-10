import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WriteOffModalComponent } from './write-off-modal.component';

describe('WriteOffModalComponent', () => {
  let component: WriteOffModalComponent;
  let fixture: ComponentFixture<WriteOffModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WriteOffModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WriteOffModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
