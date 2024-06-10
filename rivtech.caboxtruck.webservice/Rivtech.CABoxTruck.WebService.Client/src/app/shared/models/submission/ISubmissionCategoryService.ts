import { Observable } from 'rxjs';

export interface ISubmissionCategoryService {
    get<T>({ }): Observable<T>;
    post<T>({ }): Observable<T>;
    put<T>({ }): Observable<T>;

}