using TaxRulesEngine.Domain.Entities;

namespace TaxRulesEngine.Application.Strategies;

public class ProgressiveSlabStrategy : ITaxCalculationStrategy
{
    public string StrategyKey => "Progressive";

    public decimal ComputeTax(decimal taxableIncome, TaxRuleConfig rule)
    {
        if (taxableIncome <= 0) return 0m;

        if (taxableIncome <= rule.ThresholdAmount)
        {
            return Math.Round(taxableIncome * rule.BaseRate, 2);
        }

        var baseTax = rule.ThresholdAmount * rule.BaseRate;
        var excessAmount = taxableIncome - rule.ThresholdAmount;
        var excessTax = excessAmount * (rule.BaseRate + rule.SurchargeRate);

        return Math.Round(baseTax + excessTax, 2);
    }
}
