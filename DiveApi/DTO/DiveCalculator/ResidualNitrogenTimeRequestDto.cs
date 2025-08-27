using System.ComponentModel.DataAnnotations;

namespace DiveApi.DTO.DiveCalculator;

public class ResidualNitrogenTimeRequestDto
{
    [Required]
    public required string PostIntervalPressureGroup { get; set; }
    [Required]
    public int Depth { get; set; }
}