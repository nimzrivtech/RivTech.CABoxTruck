import { Injectable } from '@angular/core';
import { TablePaginationState } from 'app/shared/models/table.pagination.state';

@Injectable({
  providedIn: 'root',
})

export class PagerService {
tableState: TablePaginationState;

getPager(totalItems: number, currentPage: number = 1, pageSize: number = 10) {
  const totalPages = Math.ceil(totalItems / pageSize);
  let startPage: number, endPage: number;

  if (totalPages <= 5) {
    startPage = 1;
    endPage = totalPages;
  } else {
    if (currentPage <= 3) {
        startPage = 1;
        endPage = 5;
    } else if (currentPage + 1 >= totalPages) {
        startPage = totalPages - 4;
        endPage = totalPages;
    } else {
        if ((totalPages - (currentPage - 2)) === 5) {
            startPage = currentPage - 1;
          endPage = currentPage + 3;
        } else {
            startPage = currentPage - 2;
        endPage = currentPage + 2;
        }
      }
    }

    const startIndex = (currentPage - 1) * pageSize;
    const endIndex = Math.min(startIndex + pageSize - 1, totalItems - 1);

    // let pages = _.range(startPage, endPage + 1);
    const pages = [];
    for (let i = startPage; i <= endPage; i++) {
      pages.push(i);
    }

    this.tableState = new TablePaginationState();
    this.tableState.totalItems = totalItems;
    this.tableState.currentPage = currentPage;
    this.tableState.pageSize = pageSize;
    this.tableState.totalPages = totalPages;
    this.tableState.startPage = startPage;
    this.tableState.endPage = endPage;
    this.tableState.startIndex = startIndex;
    this.tableState.endIndex = endIndex;
    this.tableState.pages = pages;

    return this.tableState;
  }

  getPager8Items(totalItems: number, currentPage: number = 1, pageSize: number = 8) {
    const totalPages = Math.ceil(totalItems / pageSize);
    let startPage: number, endPage: number;
    if (totalPages <= 5) {
      startPage = 1;
      endPage = totalPages;
    } else {
      if (currentPage <= 3) {
          startPage = 1;
          endPage = 5;
      } else if (currentPage + 1 >= totalPages) {
          startPage = totalPages - 4;
          endPage = totalPages;
      } else {
        if ((totalPages - (currentPage - 2)) === 5) {
            startPage = currentPage - 1;
          endPage = currentPage + 3;
        } else {
            startPage = currentPage - 2;
        endPage = currentPage + 2;
        }
      }
    }

    const startIndex = (currentPage - 1) * pageSize;
    const endIndex = Math.min(startIndex + pageSize - 1, totalItems - 1);

    // let pages = _.range(startPage, endPage + 1);
    const pages = [];
    for (let i = startPage; i <= endPage; i++) {
      pages.push(i);
    }

    this.tableState = new TablePaginationState();
    this.tableState.totalItems = totalItems;
    this.tableState.currentPage = currentPage;
    this.tableState.pageSize = pageSize;
    this.tableState.totalPages = totalPages;
    this.tableState.startPage = startPage;
    this.tableState.endPage = endPage;
    this.tableState.startIndex = startIndex;
    this.tableState.endIndex = endIndex;
    this.tableState.pages = pages;

    return this.tableState;
  }

  getPager5Items(totalItems: number, currentPage: number = 1, pageSize: number = 5) {
    const totalPages = Math.ceil(totalItems / pageSize);
    let startPage: number, endPage: number;
    if (totalPages <= 5) {
      startPage = 1;
      endPage = totalPages;
    } else {
      if (currentPage <= 3) {
          startPage = 1;
          endPage = 5;
      } else if (currentPage + 1 >= totalPages) {
          startPage = totalPages - 4;
          endPage = totalPages;
      } else {
        if ((totalPages - (currentPage - 2)) === 5) {
            startPage = currentPage - 1;
          endPage = currentPage + 3;
        } else {
            startPage = currentPage - 2;
        endPage = currentPage + 2;
        }
      }
    }

    const startIndex = (currentPage - 1) * pageSize;
    const endIndex = Math.min(startIndex + pageSize - 1, totalItems - 1);

    // let pages = _.range(startPage, endPage + 1);
    const pages = [];
    for (let i = startPage; i <= endPage; i++) {
      pages.push(i);
    }

    this.tableState = new TablePaginationState();
    this.tableState.totalItems = totalItems;
    this.tableState.currentPage = currentPage;
    this.tableState.pageSize = pageSize;
    this.tableState.totalPages = totalPages;
    this.tableState.startPage = startPage;
    this.tableState.endPage = endPage;
    this.tableState.startIndex = startIndex;
    this.tableState.endIndex = endIndex;
    this.tableState.pages = pages;

    return this.tableState;
  }
}
