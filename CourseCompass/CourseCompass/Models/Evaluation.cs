namespace CourseCompass.Models
{
    public class Evaluation
    {
        public int evalID { get; set; }

        public int recommendations { get; set; }

        public int sweetness { get; set; }

        public int difficult { get; set; }

        public int workload { get; set; }

        public string comment { get; set; }
    }
}