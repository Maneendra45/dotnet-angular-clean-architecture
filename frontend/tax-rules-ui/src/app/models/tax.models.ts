export interface TaxRuleSummary {
  jurisdictionCode: string;
  jurisdictionName: string;
  strategyType: string;
  baseRatePercent: number;
}

export interface TaxCalculationRequest {
  jurisdictionCode: string;
  grossIncome: number;
  deductions: number;
}

export interface TaxCalculationResponse {
  jurisdictionCode: string;
  jurisdictionName: string;
  appliedStrategy: string;
  grossIncome: number;
  taxableIncome: number;
  calculatedTax: number;
  effectiveTaxRatePercent: number;
  evaluatedAtUtc: string;
}
