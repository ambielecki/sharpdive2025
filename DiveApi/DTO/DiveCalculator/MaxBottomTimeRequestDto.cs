using System.ComponentModel.DataAnnotations;

namespace DiveApi.DTO.DiveCalculator;

public class MaxBottomTimeRequestDto
{
    [Required]
    public int Depth { get; set; }
    public string? StartingPressureGroup { get; set; }
}