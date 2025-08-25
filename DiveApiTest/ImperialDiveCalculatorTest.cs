using DiveApi.Services.DiveCalculator;
using DiveApi.DTO.DiveCalculator;

namespace DiveApiTest;

public class ImperialDiveCalculatorTest
{
    private readonly ImperialDiveCalculator _imperialDiveCalculator = new ImperialDiveCalculator();

    [Theory]
    [InlineData(60, 30, "L")]
    [InlineData(140, 30, null)] // depth exists, over NDL
    [InlineData(30, 60, "L")]
    [InlineData(70, 13, "D")] // exact match
    [InlineData(150, 1, null)] // out of rec diving limit
    [InlineData(90, 25, "Q")] // NDL
    public void GetPressureGroupTest(int depth, int time, string? expected) {
        var dto = new PressureGroupRequestDto {
            Depth = depth,
            Time = time,
        };
        var response = _imperialDiveCalculator.GetPressureGroup(dto);
        
        Assert.Equal(expected, response.PressureGroup);
    }
    
    [Fact]
    public void GetPressureGroupExceedsNdlTest() {
        var dto = new PressureGroupRequestDto {
            Depth = 60,
            Time = 56,
        };
        var response = _imperialDiveCalculator.GetPressureGroup(dto);
        
        Assert.Null(response.PressureGroup);
        Assert.Single(response.Warnings);
        Assert.Equal(_imperialDiveCalculator.ExceedsNdl, response.Warnings[0]);
    }
    
    [Fact]
    public void GetPressureGroupExceedsMaxDepthTest() {
        var dto = new PressureGroupRequestDto {
            Depth = 160,
            Time = 5,
        };
        var response = _imperialDiveCalculator.GetPressureGroup(dto);
        
        Assert.Null(response.PressureGroup);
        Assert.Single(response.Warnings);
        Assert.Equal(_imperialDiveCalculator.ExceedsMaxDepth, response.Warnings[0]);
    }
}