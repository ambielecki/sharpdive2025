using DiveApi.DTO.DiveCalculator;

namespace DiveApi.Services.DiveCalculator;

public class ImperialDiveCalculator : IDiveCalculator
{
    private const int MaxDepth = 140;
    public string ExceedsNdl { get; } = "Dive Time Exceeds NDL For This Depth";
    public string ExceedsMaxDepth { get; } = "Depth Exceeds Recreational Limits";

    private List<string> _warnings = [];
    
    private readonly List<string> _tableGroups = [
        "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V",
        "W", "X", "Y", "Z"
    ];

    private readonly List<int> _tableDepths = [35, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140];

    private readonly Dictionary<string, List<int?>> _tableOne = new Dictionary<string, List<int?>>() {
        { "A", [10, 9, 7, 6, 5, 4, 4, 3, 3, 3, 3, null] },
        { "B", [19, 16, 13, 11, 9, 8, 7, 6, 6, 5, 5, 4] },
        { "C", [25, 22, 17, 14, 12, 10, 9, 8, 7, 6, 6, 5] },
        { "D", [29, 25, 19, 16, 13, 11, 10, 9, 8, 7, 7, 6] },
        { "E", [32, 27, 21, 17, 15, 13, 11, 10, 9, 8, null, 7] },
        { "F", [36, 31, 24, 19, 16, 14, 12, 11, 10, 9, 8, 8] },
        { "G", [40, 34, 26, 21, 18, 15, 13, 12, 11, 10, 9] },
        { "H", [44, 37, 28, 23, 19, 17, 15, 13, 12, 11, 10] },
        { "I", [48, 40, 31, 25, 21, 18, 16, 14, 13, null] },
        { "K", [57, 48, 36, 29, 24, 21, 18, 16, 14, 13] },
        { "L", [62, 51, 39, 31, 26, 22, 19, 17, 15] },
        { "M", [67, 55, 41, 33, 27, 23, 21, 18, 16] },
        { "N", [73, 60, 44, 35, 29, 25, 22, 19] },
        { "O", [79, 64, 47, 37, 31, 26, 23, 20] },
        { "P", [85, 69, 50, 39, 33, 28, 24] },
        { "Q", [92, 74, 53, 42, 35, 29, 25] },
        { "R", [100, 79, 57, 44, 36, 30] },
        { "S", [108, 85, 60, 47, 38] },
        { "T", [117, 91, 63, 49, 40] },
        { "U", [127, 97, 67, 52] },
        { "V", [139, 104, 71, 54] },
        { "W", [152, 111, 75, 55] },
        { "X", [168, 120, 80] },
        { "Y", [188, 129] },
        { "Z", [205, 140] }
    };

    private readonly Dictionary<string, List<int>> _tableTwo = new Dictionary<string, List<int>>() {
        { "A", [180] },
        { "B", [228, 47] },
        { "C", [250, 69, 21] },
        { "D", [259, 78, 30, 8] },
        { "E", [268, 87, 38, 16, 7] },
        { "F", [275, 94, 46, 24, 15, 7] },
        { "G", [282, 101, 53, 32, 22, 13, 6] },
        { "H", [288, 107, 59, 37, 28, 20, 12, 5] },
        { "I", [294, 113, 65, 43, 34, 26, 18, 11, 5] },
        { "J", [300, 119, 71, 49, 40, 31, 24, 17, 11, 5] },
        { "K", [305, 124, 76, 54, 45, 37, 29, 22, 16, 10, 4] },
        { "L", [310, 129, 81, 59, 50, 42, 34, 27, 21, 15, 9, 4] },
        { "M", [315, 134, 85, 64, 55, 46, 39, 32, 25, 19, 14, 9, 4] },
        { "N", [319, 138, 90, 68, 59, 51, 43, 36, 30, 24, 18, 13, 8, 3] },
        { "O", [324, 143, 94, 72, 63, 55, 47, 41, 34, 28, 23, 17, 12, 8, 3] },
        { "P", [328, 147, 98, 76, 67, 59, 51, 45, 38, 32, 27, 21, 16, 12, 7, 3] },
        { "Q", [331, 150, 102, 80, 71, 63, 55, 48, 42, 36, 30, 25, 20, 16, 11, 7, 3] },
        { "R", [335, 154, 106, 84, 75, 67, 59, 52, 46, 40, 34, 29, 24, 19, 15, 11, 7, 3] },
        { "S", [339, 158, 109, 87, 78, 70, 60, 56, 49, 43, 38, 32, 27, 23, 16, 14, 10, 6, 3] },
        { "T", [342, 161, 113, 91, 82, 73, 66, 59, 53, 47, 41, 36, 31, 26, 22, 17, 13, 10, 6, 2] },
        { "U", [345, 164, 116, 94, 87, 77, 69, 62, 56, 50, 44, 39, 34, 29, 25, 21, 17, 13, 9, 6, 2] },
        { "V", [348, 167, 119, 97, 88, 80, 72, 65, 59, 53, 47, 42, 37, 33, 28, 24, 20, 16, 12, 9, 5, 2] },
        { "W", [351, 170, 122, 100, 91, 83, 75, 68, 62, 56, 50, 45, 40, 36, 31, 27, 23, 19, 15, 12, 8, 5, 2] },
        { "X", [354, 173, 125, 103, 94, 86, 78, 71, 65, 59, 53, 48, 43, 39, 34, 30, 26, 22, 18, 15, 11, 8, 5, 2] },
        { "Y", [357, 176, 128, 106, 97, 89, 81, 74, 68, 62, 56, 51, 46, 41, 37, 33, 29, 25, 21, 18, 14, 11, 8, 5, 2] },
        { "Z", [360, 179, 131, 109, 100, 91, 84, 77, 71, 65, 59, 54, 49, 44, 40, 35, 31, 28, 24, 20, 17, 14, 11, 8, 5, 2] },
    };

    private readonly Dictionary<int, List<int>> _tableThree = new Dictionary<int, List<int>>() {
        { 35, [10, 19, 25, 29, 32, 36, 40, 44, 48, 52, 57, 62, 67, 73, 79, 85, 92, 100, 108, 117, 127, 139, 152, 168, 188, 205] },
        { 40, [9, 16, 22, 25, 27, 31, 34, 37, 40, 44, 48, 51, 55, 60, 64, 69, 74, 79, 85, 91, 97, 104, 111, 120, 129, 140] },
        { 50, [7, 13, 17, 19, 21, 24, 26, 28, 31, 33, 36, 38, 41, 44, 47, 50, 53, 57, 60, 63, 67, 71, 75, 80] },
        { 60, [6, 11, 14, 16, 17, 19, 21, 23, 25, 27, 29, 31, 33, 35, 37, 39, 42, 44, 47, 49, 52, 54, 55] },
        { 70, [5, 9, 12, 13, 15, 16, 18, 19, 21, 22, 24, 26, 27, 29, 31, 33, 34, 36, 38, 40] },
        { 80, [4, 8, 10, 11, 13, 14, 15, 17, 18, 19, 21, 22, 23, 25, 26, 28, 29, 30] },
        { 90, [4, 7, 9, 10, 11, 12, 13, 15, 16, 17, 18, 19, 21, 22, 23, 24, 25] },
        { 100, [3, 6, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20] },
        { 110, [3, 6, 7, 8, 9, 10, 11, 12, 13, 14, 14, 15, 16] },
        { 120, [3, 5, 6, 7, 8, 9, 10, 11, 12, 12, 13] },
        { 130, [3, 5, 6, 7, 8, 8, 9, 10] },
    };
    
    public PressureGroupResponseDto GetPressureGroup(int depth, int time, int? residualNitrogenTime = null) {
        if (depth > MaxDepth) {
            _warnings.Add(ExceedsMaxDepth);
            
            return new PressureGroupResponseDto(null, _warnings);
        }
        
        var depthKey = GetTableDepthKey(depth);
        var pressureGroup = GetPressureGroupFromTableOne(depthKey, time);

        if (pressureGroup == null) {
            _warnings.Add(ExceedsNdl);
            
            return new PressureGroupResponseDto(null, _warnings);
        }
        
        return new PressureGroupResponseDto(pressureGroup, _warnings);
    }
    
    public MaxBottomTimeResponseDto GetMaxBottomTime(int depth, int residualNitrogenTime = 0) {
        if (depth > MaxDepth) {
            _warnings.Add(ExceedsMaxDepth);
            
            return new MaxBottomTimeResponseDto(null, _warnings);
        }

        var depthKey = GetTableDepthKey(depth);
        int? maxBottomTime = null;

        if (depthKey != null) {
            foreach (KeyValuePair<string, List<int?>> row in _tableOne.Reverse()) {
                if (row.Value.Count > depthKey.Value && row.Value[depthKey.Value] != null) {
                    maxBottomTime = row.Value[depthKey.Value];
                    break;
                }
            }
        }
        
        return new MaxBottomTimeResponseDto(maxBottomTime - residualNitrogenTime, _warnings);
    }
    
    public NewPressureGroupResponseDto GetNewPressureGroup(string startingPressureGroup, int surfaceInterval) {
        startingPressureGroup = startingPressureGroup.ToUpper();;
        if (startingPressureGroup.Length > 1) {
            _warnings.Add("Starting Pressure Group Must Be A Single Letter");
            
            return new NewPressureGroupResponseDto(null, _warnings);;
        }
        
        var surfaceIntervalTimes = _tableTwo[startingPressureGroup];
        string? newGroupKey = null;
        int? key = null;

        for (var i = 0; i < surfaceIntervalTimes.Count; i++) {
            if (surfaceIntervalTimes[i] > surfaceInterval) {
                key = i;
            }
        }

        if (key != null) {
            newGroupKey = _tableGroups[key.Value];
            
            return new NewPressureGroupResponseDto(newGroupKey, _warnings);;
        }

        return new NewPressureGroupResponseDto(null, _warnings);;
    }

    public ResidualNitrogenTimeResponseDto GetResidualNitrogenTime(string postIntervalPressureGroup, int depth) {
        postIntervalPressureGroup = postIntervalPressureGroup.ToUpper();;
        if (postIntervalPressureGroup.Length > 1) {
            _warnings.Add("Starting Pressure Group Must Be A Single Letter");
            
            return new ResidualNitrogenTimeResponseDto(null, _warnings);;
        }

        if (depth > MaxDepth) {
            _warnings.Add(ExceedsMaxDepth);
            
            return new ResidualNitrogenTimeResponseDto(null, _warnings);;
        }
        
        var depthKey = GetTableDepthKey(depth);
        var standardizedDepth = _tableDepths[depthKey.Value];
        var residualNitrogenTimes = _tableThree[standardizedDepth];
        var pressureGroupKey = GetPressureGroupKey(postIntervalPressureGroup);

        if (pressureGroupKey >= residualNitrogenTimes.Count) {
            _warnings.Add(ExceedsNdl);
            
            return new ResidualNitrogenTimeResponseDto(null, _warnings);;
        }
        
        return new ResidualNitrogenTimeResponseDto(residualNitrogenTimes[pressureGroupKey], _warnings);;
    }
    
    private int? GetTableDepthKey(int depth) {
        for (var i = 0; i < _tableDepths.Count; i++) {
            if (_tableDepths[i] >= depth) {
                return i;
            }
        }

        return null;
    }

    private int GetPressureGroupKey(string pressureGroup) {
        return _tableGroups.IndexOf(pressureGroup);   
    }

    private string? GetPressureGroupFromTableOne(int? tableDepthKey, int minutes) {
        if (tableDepthKey == null) {
            return null;
        }

        foreach (KeyValuePair<string, List<int?>> row in _tableOne) {
            var length = row.Value.Count;
            if (length <= tableDepthKey.Value) {
                continue;
            }
            
            var depthTime = row.Value[tableDepthKey.Value];
            
            if (depthTime != null && depthTime >= minutes) {
                return row.Key;
            }
        }
        
        return null;
    }
}