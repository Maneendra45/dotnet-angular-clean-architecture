using Microsoft.EntityFrameworkCore;
using TaxRulesEngine.Domain.Entities;
using TaxRulesEngine.Domain.Interfaces;
using TaxRulesEngine.Infrastructure.Persistence;

namespace TaxRulesEngine.Infrastructure.Repositories;

public class RuleRepository : IRuleRepository
{
    private readonly AppDbContext _dbContext;

    public RuleRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<TaxRuleConfig>> GetActiveRulesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.TaxRules
            .AsNoTracking()
            .Where(r => r.IsActive)
            .OrderBy(r => r.JurisdictionCode)
            .ToListAsync(cancellationToken);
    }

    public async Task<TaxRuleConfig?> GetByJurisdictionAsync(string jurisdictionCode, CancellationToken cancellationToken = default)
    {
        var normalized = jurisdictionCode.Trim().ToUpperInvariant();
        return await _dbContext.TaxRules
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.IsActive && r.JurisdictionCode == normalized, cancellationToken);
    }
}
