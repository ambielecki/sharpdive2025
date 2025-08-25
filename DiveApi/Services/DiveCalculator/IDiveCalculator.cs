using DiveApi.DTO.DiveCalculator;

namespace DiveApi.Services.DiveCalculator;

public interface IDiveCalculator
{
    public PressureGroupResponseDto GetPressureGroup(PressureGroupRequestDto pressureGroupRequest);
    public MaxBottomTimeResponseDto GetMaxBottomTime(MaxBottomTimeRequestDto maxBottomTimeRequest);
    public NewPressureGroupResponseDto GetNewPressureGroup(NewPressureGroupRequestDto newPressureGroupRequest);
}