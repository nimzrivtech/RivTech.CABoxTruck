import { Component, EventEmitter, OnInit, TemplateRef } from '@angular/core';
import { BsModalService } from 'ngx-bootstrap';

@Component({
  selector: 'app-confirmation-modal',
  templateUrl: './confirmation-modal.component.html',
  styleUrls: ['./confirmation-modal.component.scss']
})
export class ConfirmationModalComponent implements OnInit {
  public event: EventEmitter<any> = new EventEmitter();
  message: string;

  constructor(private modalService: BsModalService) { }

  ngOnInit(): void { }

  confirm(): void {
    this.triggerEvent(true);
  }

  decline(): void {
    this.triggerEvent(false);
  }

  triggerEvent(res: any) {
    this.event.emit({ data: res, res: 200 });
  }
}
