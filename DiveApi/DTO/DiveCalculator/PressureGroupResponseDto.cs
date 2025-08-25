namespace DiveApi.DTO.DiveCalculator;

public class PressureGroupResponseDto(string? pressureGroup, List<string> warnings)
{
    public string? PressureGroup { get; set; } = pressureGroup;
    public List<string> Warnings { get; set; } = warnings;
}