using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainBoostVR_API.Models
{
    public class Score
    {
        public int ScoreID { get; set; }              // NOT NULL
        public int UserID { get; set; }               // NOT NULL, FK -> User
        public int ExerciseID { get; set; }           // NOT NULL, FK -> Exercise
        
		[Column("score")]
		public int? Value { get; set; }               // nullable
        public DateTime? Timestamp { get; set; }      // nullable, default CURRENT_TIMESTAMP
    }
}
