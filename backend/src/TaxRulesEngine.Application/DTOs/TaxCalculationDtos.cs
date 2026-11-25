using System.ComponentModel.DataAnnotations;

namespace TaxRulesEngine.Application.DTOs;

public record TaxCalculationRequestDto(
    [Required] string JurisdictionCode,
    [Range(0, 1_000_000_000)] decimal GrossIncome,
    [Range(0, 1_000_000_000)] decimal Deductions
);

public record TaxCalculationResponseDto(
    string JurisdictionCode,
    string JurisdictionName,
    string AppliedStrategy,
    decimal GrossIncome,
    decimal TaxableIncome,
    decimal CalculatedTax,
    decimal EffectiveTaxRatePercent,
    DateTime EvaluatedAtUtc
);

public record TaxRuleSummaryDto(
    string JurisdictionCode,
    string JurisdictionName,
    string StrategyType,
    decimal BaseRatePercent
);
