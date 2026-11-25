using TaxRulesEngine.Domain.Entities;

namespace TaxRulesEngine.Application.Strategies;

public interface ITaxCalculationStrategy
{
    string StrategyKey { get; }
    decimal ComputeTax(decimal taxableIncome, TaxRuleConfig rule);
}
