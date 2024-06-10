import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap';

@Component({
  selector: 'app-quote-proposal',
  templateUrl: './quote-proposal.component.html',
  styleUrls: ['./quote-proposal.component.scss']
})
export class QuoteProposalComponent implements OnInit {
  currentTaskMessage = '';
  dotanimation = '';
  showClose: boolean;
  modalRef: BsModalRef;

  constructor() { }

  ngOnInit() {
    this.dotAnimation();
  }

  dotAnimation() {
    let counter = 1;
    const intervalID = setInterval(() => {
      if (counter > 0 && counter < 5) {
        this.dotanimation = this.dotanimation + '.';
      } else {
        counter = 0;
        this.dotanimation = '.';
      }
      counter += 1;
    }, 700);
    return intervalID;
  }

  close() {
    this.modalRef.hide();
  }
}
