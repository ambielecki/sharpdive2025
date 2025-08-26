using System.ComponentModel.DataAnnotations;

namespace DiveApi.DTO.DiveCalculator;

public class PressureGroupRequestDto
{
    [Required]
    public required int Depth { get; set; }
    [Required]
    public required int Time { get; set; }
    public int? ResidualNitrogenTime { get; set; }
}