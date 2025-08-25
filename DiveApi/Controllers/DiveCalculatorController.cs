using DiveApi.DTO.DiveCalculator;
using DiveApi.Services.DiveCalculator;
using Microsoft.AspNetCore.Mvc;

namespace DiveApi.Controllers;

[ApiController]
[Route("[controller]")]
public class DiveCalculatorController(ILogger<DiveCalculatorController> logger, IDiveCalculator diveCalculator) : ControllerBase
{
    private readonly ILogger<DiveCalculatorController> _logger = logger;
    
    [HttpPost("pressure-group")]
    public ActionResult<PressureGroupResponseDto> GetPressureGroup(PressureGroupRequestDto pressureGroupRequest) {
        return Ok(diveCalculator.GetPressureGroup(pressureGroupRequest));
    }

    [HttpPost("max-bottom-time")]
    public ActionResult<MaxBottomTimeResponseDto> GetMaxBottomTime(MaxBottomTimeRequestDto maxBottomTimeRequest) {
        return Ok(diveCalculator.GetMaxBottomTime(maxBottomTimeRequest));
    }
    
    [HttpPost("new-pressure-group")]
    public ActionResult<NewPressureGroupResponseDto> GetNewPressureGroup(NewPressureGroupRequestDto newPressureGroupRequest) {
        return Ok(diveCalculator.GetNewPressureGroup(newPressureGroupRequest));
    }
}