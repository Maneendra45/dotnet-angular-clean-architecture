using TaxRulesEngine.Application.DTOs;

namespace TaxRulesEngine.Application.Services;

public interface ITaxEvaluationService
{
    Task<IReadOnlyList<TaxRuleSummaryDto>> GetAvailableRulesAsync(CancellationToken cancellationToken = default);
    Task<TaxCalculationResponseDto> EvaluateAsync(TaxCalculationRequestDto request, CancellationToken cancellationToken = default);
}
