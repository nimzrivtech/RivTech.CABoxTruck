import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddQuoteConditionComponent } from './add-quote-condition.component';

describe('AddQuoteConditionComponent', () => {
  let component: AddQuoteConditionComponent;
  let fixture: ComponentFixture<AddQuoteConditionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddQuoteConditionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddQuoteConditionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
