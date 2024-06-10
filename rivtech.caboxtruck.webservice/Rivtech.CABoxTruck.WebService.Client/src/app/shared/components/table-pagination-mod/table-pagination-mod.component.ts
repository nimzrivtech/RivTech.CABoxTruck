import { EventEmitter } from '@angular/core';
import { Output } from '@angular/core';
import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-table-pagination-mod',
  templateUrl: './table-pagination-mod.component.html',
  styleUrls: ['./table-pagination-mod.component.scss']
})
export class TablePaginationModdedComponent implements OnInit {
  @Input() pager: any = {};
  @Input() loading: boolean;

  @Output() setPage: EventEmitter<number> = new EventEmitter<number>();

  constructor() { }

  ngOnInit() {
  }

  setPageNumber(page: number) {
    this.setPage.emit(page);
  }
}
