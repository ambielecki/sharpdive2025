namespace DiveApi.DTO.DiveCalculator;

public class NewPressureGroupResponseDto(string? newPressureGroup, List<string> warnings)
{
    public string? NewPressureGroup { get; set; } = newPressureGroup;
    public List<string> Warnings { get; set; } = warnings;
}