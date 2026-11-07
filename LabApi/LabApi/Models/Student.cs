namespace LabApi.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public int Age { get; set; }
        public string GroupName { get; set; } = string.Empty;
        public double AverageGrade { get; set; }
    }
}