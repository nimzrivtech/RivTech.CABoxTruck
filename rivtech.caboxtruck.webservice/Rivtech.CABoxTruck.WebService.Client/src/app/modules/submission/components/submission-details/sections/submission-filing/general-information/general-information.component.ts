import { Component, OnInit, ViewChild } from '@angular/core';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { DataTableDirective } from 'angular-datatables';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-general-information',
  templateUrl: './general-information.component.html',
  styleUrls: ['./general-information.component.scss']
})
export class GeneralInformationComponent implements OnInit {

  dropdownList = [];
  selectedItems = [];
  dropdownSettings: IDropdownSettings = {};
  
  selectedFilings: string[];
  hasRequiredFilings: boolean =  true;
  hasDifferentDotInFiveYears: boolean = true;

  // table settings
  @ViewChild(DataTableDirective)
  dtElement: DataTableDirective;
  dtOptions: DataTables.Settings = {};
  dtTrigger: Subject<any> = new Subject();
  isDtInitialized: boolean = false;

  constructor() { }

  ngOnInit() {
    this.dropdownList = [
      { item_id: 1, item_text: 'Required 1' },
      // { item_id: 2, item_text: 'Bangaluru' },
      // { item_id: 3, item_text: 'Pune' },
      // { item_id: 4, item_text: 'Navsari' },
      { item_id: 5, item_text: 'Required 2' }
    ];
    this.selectedItems = [
    ];
    this.dropdownSettings = {
      singleSelection: false,
      idField: 'item_id',
      textField: 'item_text',
      selectAllText: 'Select All',
      unSelectAllText: 'Unselect All',
      itemsShowLimit: 3,
      allowSearchFilter: true
    };
    
    this.dtOptions = {
      pagingType: 'full_numbers',
      responsive: true,
      processing: true,
      destroy:true
    };

    this.renderTable();

  }

  renderTable(): void {
    if(this.isDtInitialized){
      this.dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
        // Destroy the table first
        dtInstance.destroy();
        // Call the dtTrigger to rerender again
        this.dtTrigger.next();
      });
    }
    else{
      this.dtTrigger.next();
      this.isDtInitialized = true;
    }
  }

  onItemSelect(item: any) {
    console.log(item);
  }
  onSelectAll(items: any) {
    console.log(items);
  }
  hasRequiredFilingsChanged($event) : void {
    this.selectedFilings = [];
  }

}
