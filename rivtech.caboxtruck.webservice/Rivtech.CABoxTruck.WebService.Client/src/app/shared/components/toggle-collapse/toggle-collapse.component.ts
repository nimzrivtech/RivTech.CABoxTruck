import { Component, EventEmitter, HostListener, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-toggle-collapse',
  templateUrl: './toggle-collapse.component.html',
  styleUrls: ['./toggle-collapse.component.scss']
})
export class ToggleCollapseComponent implements OnInit {
  constructor() { }

  @Output() clickType = new EventEmitter();
  @Input() toggleName: string = "toggle";

  ngOnInit() {
  }

  public ToggleHiding(obj?): void {
    let element = document.querySelector(obj.currentTarget.attributes.getNamedItem('data-target').value) as any;
    obj.preventDefault();
    if(element.classList.contains('show')){
        obj.target.classList.add("icon-arrow-down");
        obj.target.classList.remove("icon-arrow-up");
    }
    else{
        obj.target.classList.add("icon-arrow-up");
        obj.target.classList.remove("icon-arrow-down");
    }
  }

  clickExpandCollapse(name  :string, evt: Event){
    let element = document.querySelector("[data-target='#" + name + "']") as HTMLElement;
    element.click();
    evt.preventDefault();
  }
}
