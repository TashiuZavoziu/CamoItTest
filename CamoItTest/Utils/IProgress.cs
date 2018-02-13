using System;

namespace CamoItTest.Utils {
    public interface IProgress {
        event Action<int, string> ProgressUpdated;
    }
}