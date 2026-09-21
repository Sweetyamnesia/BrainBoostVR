using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainBoostVR_API.Models
{
    public class Score
    {
        public int ScoreID { get; set; }              // NOT NULL
        public int UserID { get; set; }               // NOT NULL, FK -> User
        public int? ExerciseID { get; set; }           // NOT NULL, FK -> Exercise
        
		[Column("score")]
		public int? Value { get; set; }               // nullable
        public int? Errors { get; set; }			  // nullable
		public float? TimeSpent { get; set; } 		  // nullable (en secondes)
		public string? SessionUid { get; set; }  	  // nullable, GUID session
		public DateTime? Timestamp { get; set; }      // nullable, default CURRENT_TIMESTAMP
    }
}
