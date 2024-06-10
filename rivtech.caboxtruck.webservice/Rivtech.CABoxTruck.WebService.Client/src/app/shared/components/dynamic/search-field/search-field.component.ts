/* eslint-disable @angular-eslint/no-output-on-prefix */
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { BaseComponent } from '../../../base-component';

@Component({
  selector: 'app-search-field',
  templateUrl: './search-field.component.html',
  styleUrls: ['./search-field.component.scss']
})
export class SearchFieldComponent extends BaseComponent implements OnInit {

  searchInput: string = '';
  @Output() onInputSearch = new EventEmitter<string>();

  constructor() {
    super();
  }

  ngOnInit() {
  }

  onSearch(): void {
    this.onInputSearch.emit(this.searchInput.toLowerCase().trim());
  }

  onChange(): void {
    if (this.searchInput.length === 0) {
      this.onInputSearch.emit(this.searchInput);
    }
  }
}
