using System;

namespace BrainBoostVR_API.Models
{
    public class Exercise
    {
        public int ExerciseID { get; set; }           // NOT NULL
        public int UserID { get; set; }               // NOT NULL, FK -> User
        public int? Score { get; set; }               // nullable
        public float? DurationMinutes { get; set; }   // nullable
        public int? Successes { get; set; }           // nullable
        public int? Failures { get; set; }            // nullable
        public DateTime? Date { get; set; }           // nullable
    }
}
