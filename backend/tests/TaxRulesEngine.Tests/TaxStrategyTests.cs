using TaxRulesEngine.Application.Strategies;
using TaxRulesEngine.Domain.Entities;
using Xunit;

namespace TaxRulesEngine.Tests;

public class TaxStrategyTests
{
    [Fact]
    public void FlatCorporateTaxStrategy_ComputesExpectedFlatPercentage()
    {
        var strategy = new FlatCorporateTaxStrategy();
        var rule = new TaxRuleConfig { BaseRate = 0.21m };

        var tax = strategy.ComputeTax(100_000m, rule);

        Assert.Equal(21_000m, tax);
    }

    [Fact]
    public void ProgressiveSlabStrategy_AppliesSurchargeOnlyAboveThreshold()
    {
        var strategy = new ProgressiveSlabStrategy();
        var rule = new TaxRuleConfig
        {
            BaseRate = 0.10m,
            ThresholdAmount = 100_000m,
            SurchargeRate = 0.05m
        };

        // 100,000 * 10% = 10,000; remaining 50,000 * 15% = 7,500 => 17,500
        var tax = strategy.ComputeTax(150_000m, rule);

        Assert.Equal(17_500m, tax);
    }
}
