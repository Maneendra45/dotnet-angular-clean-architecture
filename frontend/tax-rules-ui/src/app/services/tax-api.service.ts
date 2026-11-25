import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import {
  TaxCalculationRequest,
  TaxCalculationResponse,
  TaxRuleSummary
} from '../models/tax.models';

@Injectable({
  providedIn: 'root'
})
export class TaxApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = 'http://localhost:5000/api/v1/taxcalculations';

  getAvailableRules(): Observable<TaxRuleSummary[]> {
    return this.http
      .get<TaxRuleSummary[]>(`${this.baseUrl}/rules`)
      .pipe(catchError(this.handleError));
  }

  evaluateTax(payload: TaxCalculationRequest): Observable<TaxCalculationResponse> {
    return this.http
      .post<TaxCalculationResponse>(`${this.baseUrl}/evaluate`, payload)
      .pipe(catchError(this.handleError));
  }

  private handleError(error: HttpErrorResponse): Observable<never> {
    const message =
      error.error?.detail || error.message || 'An unexpected API error occurred.';
    return throwError(() => new Error(message));
  }
}
