using System.ComponentModel.DataAnnotations;

namespace DiveApi.DTO.DiveCalculator;

public class NewPressureGroupRequestDto
{
    [Required]
    public required string StartingPressureGroup { get; set; }
    [Required]
    public required int SurfaceInterval { get; set; }
}