import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { QuoteConditionComponent } from './quote-condition.component';

describe('QuoteConditionComponent', () => {
  let component: QuoteConditionComponent;
  let fixture: ComponentFixture<QuoteConditionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ QuoteConditionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(QuoteConditionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
