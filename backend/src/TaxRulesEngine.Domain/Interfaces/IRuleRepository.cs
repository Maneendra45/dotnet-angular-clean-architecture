using TaxRulesEngine.Domain.Entities;

namespace TaxRulesEngine.Domain.Interfaces;

public interface IRuleRepository
{
    Task<IReadOnlyList<TaxRuleConfig>> GetActiveRulesAsync(CancellationToken cancellationToken = default);
    Task<TaxRuleConfig?> GetByJurisdictionAsync(string jurisdictionCode, CancellationToken cancellationToken = default);
}
