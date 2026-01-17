namespace AdobeCrapKiller {
    public class AdobeMemoryWastingCrap {
        public string ProcessPath { get; set; }
        public int ProcessId { get; set; }

        public AdobeMemoryWastingCrap(string processPath) {
            ProcessPath = processPath;
            ProcessId = 0;
        }

        public AdobeMemoryWastingCrap(string processPath, int processId) {
            ProcessPath = processPath;
            ProcessId = processId;
        }
    }
}
