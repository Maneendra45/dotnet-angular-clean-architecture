namespace TaxRulesEngine.Domain.Entities;

public class TaxRuleConfig
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string JurisdictionCode { get; set; } = string.Empty;
    public string JurisdictionName { get; set; } = string.Empty;
    public string StrategyType { get; set; } = string.Empty; // "Progressive" | "Flat"
    public decimal BaseRate { get; set; }
    public decimal ThresholdAmount { get; set; }
    public decimal SurchargeRate { get; set; }
    public bool IsActive { get; set; } = true;
}
