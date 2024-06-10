import { Injectable } from "@angular/core";
import { DocumentApiService } from "./document-api.service";
import { DocumentFile } from "./document-file";

@Injectable()
export class DocumentService { 

    constructor(
        private documentApiService: DocumentApiService
    ) {

    }

    upload(doc: DocumentFile) {

    }

}