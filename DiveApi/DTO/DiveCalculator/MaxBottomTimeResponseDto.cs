namespace DiveApi.DTO.DiveCalculator;

public class MaxBottomTimeResponseDto(int? maxBottomTime, List<string> warnings)
{
    public int? MaxBottomTime { get; set; } = maxBottomTime;
    public List<string> Warnings { get; set; } = warnings;
}