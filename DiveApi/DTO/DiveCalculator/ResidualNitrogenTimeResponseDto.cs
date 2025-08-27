namespace DiveApi.DTO.DiveCalculator;

public class ResidualNitrogenTimeResponseDto(int? residualNitrogenTime, List<string> warnings)
{
    public int? ResidualNitrogenTime { get; set; } = residualNitrogenTime;
    public List<string> Warnings { get; set; } = warnings;
}