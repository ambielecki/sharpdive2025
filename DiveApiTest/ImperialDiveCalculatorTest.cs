using DiveApi.Services.DiveCalculator;

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
        var response = _imperialDiveCalculator.GetPressureGroup(depth, time, null);
        
        Assert.Equal(expected, response.PressureGroup);
    }
    
    [Fact]
    public void GetPressureGroupExceedsNdlTest()
    {
        const int depth = 60;
        const int time = 56;
        var response = _imperialDiveCalculator.GetPressureGroup(depth, time, null);
        
        Assert.Null(response.PressureGroup);
        Assert.Single(response.Warnings);
        Assert.Equal(_imperialDiveCalculator.ExceedsNdl, response.Warnings[0]);
    }
    
    [Fact]
    public void GetPressureGroupExceedsMaxDepthTest() {
        const int depth = 160;
        const int time = 5;
        var response = _imperialDiveCalculator.GetPressureGroup(depth, time, null);
        
        Assert.Null(response.PressureGroup);
        Assert.Single(response.Warnings);
        Assert.Equal(_imperialDiveCalculator.ExceedsMaxDepth, response.Warnings[0]);
    }

    [Theory]
    [InlineData(34, 205)]
    [InlineData(35, 205)]
    [InlineData(140, 8)]
    [InlineData(150, null)]
    public void GetMaxBottomTimeTest(int depth, int? expected) {
        var response = _imperialDiveCalculator.GetMaxBottomTime(depth);
        
        Assert.Equal(expected, response.MaxBottomTime);
    }

    [Fact]
    public void GetMaxBottomTimeExceedsMaxDepthTest() {
        const int depth = 160;
        var response = _imperialDiveCalculator.GetMaxBottomTime(depth);
        
        Assert.Null(response.MaxBottomTime);
        Assert.Single(response.Warnings);
        Assert.Equal(_imperialDiveCalculator.ExceedsMaxDepth, response.Warnings[0]);
    }

    [Theory]
    [InlineData("A", 60, "A")]
    [InlineData("A", 500, null)]
    [InlineData("R", 60, "F")]
    public void GetNewPressureGroupTest(string pressureGroup, int depth, string? expected) {
        var response = _imperialDiveCalculator.GetNewPressureGroup(pressureGroup, depth);
        
        Assert.Equal(expected, response.NewPressureGroup);
    }
    
    [Theory]
    [InlineData("A", 60, 6)]
    [InlineData("B", 35, 19)]
    [InlineData("B", 30, 19)]
    [InlineData("L", 120, null)]
    public void GetResidualNitrogenTimeTest(string pressureGroup, int depth, int? expected) {
        var response = _imperialDiveCalculator.GetResidualNitrogenTime(pressureGroup, depth);
        
        Assert.Equal(expected, response.ResidualNitrogenTime);
    }
}