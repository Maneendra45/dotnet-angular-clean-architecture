import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { TaxApiService } from '../services/tax-api.service';
import { TaxCalculationResponse, TaxRuleSummary } from '../models/tax.models';

@Component({
  selector: 'app-tax-calculator',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <section class="calculator-card">
      <h2>Enterprise Tax Rule Evaluator</h2>

      <form [formGroup]="form" (ngSubmit)="onSubmit()">
        <label>
          Jurisdiction
          <select formControlName="jurisdictionCode">
            @for (rule of rules(); track rule.jurisdictionCode) {
              <option [value]="rule.jurisdictionCode">
                {{ rule.jurisdictionCode }} - {{ rule.jurisdictionName }} ({{ rule.strategyType }})
              </option>
            }
          </select>
        </label>

        <label>
          Gross Income (USD)
          <input type="number" formControlName="grossIncome" min="0" />
        </label>

        <label>
          Deductions (USD)
          <input type="number" formControlName="deductions" min="0" />
        </label>

        <button type="submit" [disabled]="form.invalid || isSubmitting()">
          {{ isSubmitting() ? 'Evaluating...' : 'Calculate Liability' }}
        </button>
      </form>

      @if (errorMessage()) {
        <p class="error">{{ errorMessage() }}</p>
      }

      @if (result(); as res) {
        <div class="result-panel">
          <h3>Evaluation Summary ({{ res.jurisdictionCode }})</h3>
          <p><strong>Strategy Applied:</strong> {{ res.appliedStrategy }}</p>
          <p><strong>Taxable Income:</strong> {{ res.taxableIncome | currency }}</p>
          <p><strong>Calculated Tax:</strong> {{ res.calculatedTax | currency }}</p>
          <p><strong>Effective Rate:</strong> {{ res.effectiveTaxRatePercent }}%</p>
        </div>
      }
    </section>
  `
})
export class TaxCalculatorComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly taxApi = inject(TaxApiService);

  readonly rules = signal<TaxRuleSummary[]>([]);
  readonly result = signal<TaxCalculationResponse | null>(null);
  readonly errorMessage = signal<string | null>(null);
  readonly isSubmitting = signal(false);

  readonly form = this.fb.nonNullable.group({
    jurisdictionCode: ['US-FED', Validators.required],
    grossIncome: [150000, [Validators.required, Validators.min(0)]],
    deductions: [15000, [Validators.required, Validators.min(0)]]
  });

  ngOnInit(): void {
    this.taxApi.getAvailableRules().subscribe({
      next: (data) => this.rules.set(data),
      error: (err: Error) => this.errorMessage.set(err.message)
    });
  }

  onSubmit(): void {
    if (this.form.invalid) return;

    this.isSubmitting.set(true);
    this.errorMessage.set(null);

    this.taxApi.evaluateTax(this.form.getRawValue()).subscribe({
      next: (res) => {
        this.result.set(res);
        this.isSubmitting.set(false);
      },
      error: (err: Error) => {
        this.errorMessage.set(err.message);
        this.isSubmitting.set(false);
      }
    });
  }
}
