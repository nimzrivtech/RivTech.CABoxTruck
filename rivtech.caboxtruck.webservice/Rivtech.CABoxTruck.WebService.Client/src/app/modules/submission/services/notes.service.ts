import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { CommonService } from '../../../core/services/common.service';
import { NoteCategoryOptionDTO, NoteDTO } from '../../../shared/models/submission/note/NoteDto';
import { catchError, map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class NoteService {
    private baseUrl: string;

    constructor(private http: HttpClient, private commonService: CommonService) {
        this.baseUrl = environment.ApiUrl.replace('/api', '');
    }

    getNoteCategoryOptions(): Observable<NoteCategoryOptionDTO[]> {
        const url_ = this.baseUrl + `/api/KeyValue/generic/NoteCategoryOption`;

        return this.http.get(url_)
                .pipe(
                    map(data => {
                        return data as NoteCategoryOptionDTO[];
                    }),
                    catchError(this.commonService.handleObservableHttpError)
                );
    }

    saveNote(body: NoteDTO | undefined): Observable<NoteDTO> {
        const url_ = this.baseUrl + `/api/RiskNotes`;

        return this.http.post(url_, body)
            .map(data => {
                return data as NoteDTO;
            })
            .catch(this.commonService.handleObservableHttpError);
    }

    deleteNote(id: string): Observable<any> {
        const url_ = this.baseUrl + `/api/RiskNotes/${id}`;

        return this.http.delete(url_).catch(this.commonService.handleObservableHttpError);
    }
}
