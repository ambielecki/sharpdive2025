using DiveApi.DTO.DiveCalculator;

namespace DiveApi.Services.DiveCalculator;

public interface IDiveCalculator
{
    public PressureGroupResponseDto GetPressureGroup(int depth, int time, int? residualNitrogenTime);
    public MaxBottomTimeResponseDto GetMaxBottomTime(int depth);
    public NewPressureGroupResponseDto GetNewPressureGroup(string startingPressureGroup, int surfaceInterval);
}