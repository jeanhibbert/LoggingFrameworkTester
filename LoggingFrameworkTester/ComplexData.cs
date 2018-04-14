using System;

namespace LoggingFrameworkTester
{
    public class ComplexData
    {
        public int Sequence { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ErrorType { get; set; }
        public int ErrorTypeId { get; internal set; }
    }
}
