import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PostPaymentModalComponent } from './post-payment-modal.component';

describe('PostPaymentModalComponent', () => {
  let component: PostPaymentModalComponent;
  let fixture: ComponentFixture<PostPaymentModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PostPaymentModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PostPaymentModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
