import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PostPaymentCheckComponent } from './post-payment-check.component';

describe('PostPaymentCheckComponent', () => {
  let component: PostPaymentCheckComponent;
  let fixture: ComponentFixture<PostPaymentCheckComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PostPaymentCheckComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PostPaymentCheckComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
