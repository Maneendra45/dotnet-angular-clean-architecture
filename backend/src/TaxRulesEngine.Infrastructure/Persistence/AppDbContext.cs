using Microsoft.EntityFrameworkCore;
using TaxRulesEngine.Domain.Entities;

namespace TaxRulesEngine.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<TaxRuleConfig> TaxRules => Set<TaxRuleConfig>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaxRuleConfig>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.JurisdictionCode).IsUnique();
            entity.Property(e => e.JurisdictionCode).HasMaxLength(20).IsRequired();
            entity.Property(e => e.JurisdictionName).HasMaxLength(120).IsRequired();
            entity.Property(e => e.BaseRate).HasPrecision(5, 4);
            entity.Property(e => e.SurchargeRate).HasPrecision(5, 4);
            entity.Property(e => e.ThresholdAmount).HasPrecision(18, 2);

            entity.HasData(
                new TaxRuleConfig
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    JurisdictionCode = "US-FED",
                    JurisdictionName = "United States Federal Corporate",
                    StrategyType = "Flat",
                    BaseRate = 0.21m,
                    ThresholdAmount = 0m,
                    SurchargeRate = 0m,
                    IsActive = true
                },
                new TaxRuleConfig
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    JurisdictionCode = "US-CA",
                    JurisdictionName = "California Progressive Tier",
                    StrategyType = "Progressive",
                    BaseRate = 0.0884m,
                    ThresholdAmount = 100000m,
                    SurchargeRate = 0.025m,
                    IsActive = true
                },
                new TaxRuleConfig
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    JurisdictionCode = "US-NY",
                    JurisdictionName = "New York State Tiered Rule",
                    StrategyType = "Progressive",
                    BaseRate = 0.065m,
                    ThresholdAmount = 250000m,
                    SurchargeRate = 0.015m,
                    IsActive = true
                }
            );
        });
    }
}
