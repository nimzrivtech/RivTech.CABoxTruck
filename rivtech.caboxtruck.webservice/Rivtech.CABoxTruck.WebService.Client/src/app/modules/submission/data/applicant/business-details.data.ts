import { Injectable } from '@angular/core';
import { PagerService } from '../../../../core/services/pager.service';
import { TableData } from '../tables.data';
import { ApplicantData } from './applicant.data';

@Injectable()
export class BusinessDetailsData {

    companyListPager: any = {};
    companyListItems: any[];
    companyListCurrentPage: number = 0;

    subsidiaryListPager: any = {};
    subsidiaryListItems: any[];
    subsidiaryListCurrentPage: number = 0;

    constructor(private pagerService: PagerService,
                private tableData: TableData,
                private applicantData: ApplicantData
    ) {}

    resetFields() {
        this.companyListPager = {};
        this.companyListItems = [];
        this.companyListCurrentPage = 0;
        this.subsidiaryListPager = {};
        this.subsidiaryListItems = [];
        this.subsidiaryListCurrentPage = 0;
    }

    setPreviousCompanyPage(page: number, data: any[], hasAdded: boolean = false) {
        if (page < 1) {
          return;
        }

        this.companyListPager = this.pagerService.getPager(data.length, page);

        if (hasAdded) {
          this.companyListCurrentPage = this.companyListPager.currentPage = this.companyListPager.totalPages;
          this.companyListPager.endIndex = this.companyListPager.totalItems;
          this.companyListPager.startIndex = ((this.companyListPager.currentPage * this.companyListPager.pageSize) - this.companyListPager.pageSize);
        } else {
          while (this.companyListPager.totalItems <= ((this.companyListPager.currentPage * this.companyListPager.pageSize) - this.companyListPager.pageSize)) {
            this.companyListPager.currentPage--;
            this.companyListPager.startIndex = ((this.companyListPager.currentPage * this.companyListPager.pageSize) - this.companyListPager.pageSize);
          }
          this.companyListCurrentPage = page;
        }

        this.companyListItems = data.slice(this.companyListPager.startIndex, this.companyListPager.endIndex + 1);
    }

    setSubsidiaryPage(page: number, data: any[], hasAdded: boolean = false) {
        if (page < 1) {
          return;
        }

        this.subsidiaryListPager = this.pagerService.getPager(data.length, page);

        if (hasAdded) {
          this.subsidiaryListCurrentPage = this.subsidiaryListPager.currentPage = this.subsidiaryListPager.totalPages;
          this.subsidiaryListPager.endIndex = this.subsidiaryListPager.totalItems;
          this.subsidiaryListPager.startIndex = ((this.subsidiaryListPager.currentPage * this.subsidiaryListPager.pageSize) - this.subsidiaryListPager.pageSize);
        } else {
          while (this.subsidiaryListPager.totalItems <= ((this.subsidiaryListPager.currentPage * this.subsidiaryListPager.pageSize) - this.subsidiaryListPager.pageSize)) {
            this.subsidiaryListPager.currentPage--;
            this.subsidiaryListPager.startIndex = ((this.subsidiaryListPager.currentPage * this.subsidiaryListPager.pageSize) - this.subsidiaryListPager.pageSize);
          }
          this.subsidiaryListCurrentPage = page;
        }

        this.subsidiaryListItems = data.slice(this.subsidiaryListPager.startIndex, this.subsidiaryListPager.endIndex + 1);
    }

    sortSubsidiaryList(targetElement) {
        let sortData = this.tableData.sortPagedData(targetElement, this.applicantData.subsidiaryList, this.subsidiaryListCurrentPage);
        this.applicantData.subsidiaryList = sortData.sortedData;
        this.subsidiaryListItems = sortData.pagedItems;
      }

      sortCompanyList(targetElement) {
        let sortData = this.tableData.sortPagedData(targetElement, this.applicantData.companyList, this.companyListCurrentPage);
        this.applicantData.companyList = sortData.sortedData;
        this.companyListItems = sortData.pagedItems;
      }
}
