namespace SmartChef.Models
{
    public class GroqResponse
    {
        public string Id { get; set; }
        public string Object { get; set; }
        public int Created { get; set; }
        public string Model { get; set; }
        public Choice[] Choices { get; set; }
        public UsageData Usage { get; set; }
        public SystemFingerprintData SystemFingerprint { get; set; }
        public XGroqData XGroq { get; set; }

        public class Choice
        {
            public int Index { get; set; }
            public Message Message { get; set; }
            public string FinishReason { get; set; }
        }

        public class Message
        {
            public string Role { get; set; }
            public string Content { get; set; }
        }

        public class UsageData
        {
            public int PromptTokens { get; set; }
            public double PromptTime { get; set; }
            public int CompletionTokens { get; set; }
            public double CompletionTime { get; set; }
            public int TotalTokens { get; set; }
            public double TotalTime { get; set; }
        }

        public class SystemFingerprintData
        {
            public string Fingerprint { get; set; }
        }

        public class XGroqData
        {
            public string Id { get; set; }
        }
    }
}





