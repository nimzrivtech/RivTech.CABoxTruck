import { Injectable } from '@angular/core';
import { Sort } from '../../../shared/utilities/sort';
import { PagerService } from '../../../core/services/pager.service';

@Injectable(
  { providedIn: 'any'}
)
export class TableData {

    constructor(private pagerService: PagerService) {}

    sort(targetElement: HTMLElement, data: any[] = [], currentPage: number = 0): any [] {
      let pager: any = {};
      let pagedItems: any[] = [];
      const sort = new Sort();
      const elem = targetElement;
      const order = elem.getAttribute('data-order');
      const type = elem.getAttribute('data-type');
      const property = elem.getAttribute('data-name');

      if (order === 'desc') {
        data.sort(sort.startSort(property, order, type));
        elem.setAttribute('data-order', 'asc');
      } else {
        data.sort(sort.startSort(property, order, type));
        elem.setAttribute('data-order', 'desc');
      }

      pager = this.pagerService.getPager(data.length, currentPage);
      pagedItems = data.slice(pager.startIndex, pager.endIndex + 1);

      return pagedItems;
    }

    sortPagedData(targetElement: HTMLElement, data: any[] = [], currentPage: number = 0, currentPageSize: number = 10): any {
      let pager: any = {};
      let pagedItems: any[] = [];
      const sort = new Sort();
      const elem = targetElement;
      const order = elem.getAttribute('data-order');
      const type = elem.getAttribute('data-type');
      const property = elem.getAttribute('data-name');

      if (order === 'desc') {
        data.sort(sort.startSort(property, order, type));
        elem.setAttribute('data-order', 'asc');
      } else {
        data.sort(sort.startSort(property, order, type));
        elem.setAttribute('data-order', 'desc');
      }

      pager = this.pagerService.getPager(data.length, currentPage, currentPageSize);
      pagedItems = data.slice(pager.startIndex, pager.endIndex + 1);

      return {
        sortedData: data,
        pagedItems: pagedItems,
        sortOptions: {
          order,
          type,
          property
        }
      };
    }

    sortPagedDataWithProperty(property: string, order: string, data: any[] = [], currentPage: number = 0, currentPageSize: number = 10,fromDriver: number | null = null): any {
      let pager: any = {};
      let pagedItems: any[] = [];
      const sort = new Sort();

      if (order === 'desc') {
        data.sort(sort.startSort(property, order));
      } else {
        if(fromDriver === 1) 
          data.sort(sort.startSort(property, order,'date',fromDriver));
        else 
          data.sort(sort.startSort(property, order, 'date'));
      }

      pager = this.pagerService.getPager(data.length, currentPage, currentPageSize);
      pagedItems = data.slice(pager.startIndex, pager.endIndex + 1);
      
      return {
        sortedData: data,
        pagedItems: pagedItems,
        sortOptions: {
          order,
          property
        }
      };
    }
}
