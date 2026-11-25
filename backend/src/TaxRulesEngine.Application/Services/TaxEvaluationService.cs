using TaxRulesEngine.Application.DTOs;
using TaxRulesEngine.Application.Strategies;
using TaxRulesEngine.Domain.Exceptions;
using TaxRulesEngine.Domain.Interfaces;

namespace TaxRulesEngine.Application.Services;

public class TaxEvaluationService : ITaxEvaluationService
{
    private readonly IRuleRepository _ruleRepository;
    private readonly IReadOnlyDictionary<string, ITaxCalculationStrategy> _strategies;

    public TaxEvaluationService(
        IRuleRepository ruleRepository,
        IEnumerable<ITaxCalculationStrategy> strategies)
    {
        _ruleRepository = ruleRepository;
        _strategies = strategies.ToDictionary(s => s.StrategyKey, StringComparer.OrdinalIgnoreCase);
    }

    public async Task<IReadOnlyList<TaxRuleSummaryDto>> GetAvailableRulesAsync(CancellationToken cancellationToken = default)
    {
        var rules = await _ruleRepository.GetActiveRulesAsync(cancellationToken);
        return rules
            .Select(r => new TaxRuleSummaryDto(
                r.JurisdictionCode,
                r.JurisdictionName,
                r.StrategyType,
                Math.Round(r.BaseRate * 100, 2)))
            .ToList();
    }

    public async Task<TaxCalculationResponseDto> EvaluateAsync(
        TaxCalculationRequestDto request,
        CancellationToken cancellationToken = default)
    {
        if (request.Deductions > request.GrossIncome)
        {
            throw new DomainValidationException("Deductions cannot exceed gross income.");
        }

        var rule = await _ruleRepository.GetByJurisdictionAsync(request.JurisdictionCode, cancellationToken)
            ?? throw new KeyNotFoundException($"No active tax rule configured for jurisdiction '{request.JurisdictionCode}'.");

        if (!_strategies.TryGetValue(rule.StrategyType, out var strategy))
        {
            throw new InvalidOperationException($"Unsupported tax strategy '{rule.StrategyType}'.");
        }

        var taxableIncome = Math.Max(0m, request.GrossIncome - request.Deductions);
        var tax = strategy.ComputeTax(taxableIncome, rule);
        var effectiveRate = request.GrossIncome > 0
            ? Math.Round((tax / request.GrossIncome) * 100, 2)
            : 0m;

        return new TaxCalculationResponseDto(
            rule.JurisdictionCode,
            rule.JurisdictionName,
            strategy.StrategyKey,
            request.GrossIncome,
            taxableIncome,
            tax,
            effectiveRate,
            DateTime.UtcNow
        );
    }
}
