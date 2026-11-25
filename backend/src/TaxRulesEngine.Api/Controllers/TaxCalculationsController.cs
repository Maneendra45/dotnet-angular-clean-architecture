using Microsoft.AspNetCore.Mvc;
using TaxRulesEngine.Application.DTOs;
using TaxRulesEngine.Application.Services;

namespace TaxRulesEngine.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TaxCalculationsController : ControllerBase
{
    private readonly ITaxEvaluationService _evaluationService;

    public TaxCalculationsController(ITaxEvaluationService evaluationService)
    {
        _evaluationService = evaluationService;
    }

    [HttpGet("rules")]
    [ProducesResponseType(typeof(IReadOnlyList<TaxRuleSummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRules(CancellationToken cancellationToken)
    {
        var rules = await _evaluationService.GetAvailableRulesAsync(cancellationToken);
        return Ok(rules);
    }

    [HttpPost("evaluate")]
    [ProducesResponseType(typeof(TaxCalculationResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Evaluate(
        [FromBody] TaxCalculationRequestDto request,
        CancellationToken cancellationToken)
    {
        var result = await _evaluationService.EvaluateAsync(request, cancellationToken);
        return Ok(result);
    }
}
