using System;

namespace CourseCompass.Models
{
    public class Collect
    {
        public string studentID { get; set; }

        public string courseID { get; set; }

        public DateTime collectTime { get; set; }

        public string note { get; set; }
    }
}