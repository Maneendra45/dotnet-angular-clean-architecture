using TaxRulesEngine.Domain.Entities;

namespace TaxRulesEngine.Application.Strategies;

public class FlatCorporateTaxStrategy : ITaxCalculationStrategy
{
    public string StrategyKey => "Flat";

    public decimal ComputeTax(decimal taxableIncome, TaxRuleConfig rule)
    {
        if (taxableIncome <= 0) return 0m;
        return Math.Round(taxableIncome * rule.BaseRate, 2);
    }
}
