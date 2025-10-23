using System;

namespace BrainBoostVR_API.Models
{
    public class Session
    {
        public int SessionID { get; set; }            // NOT NULL
        public int UserID { get; set; }               // NOT NULL, FK -> User
        public DateTime? StartTime { get; set; }      // nullable
        public DateTime? EndTime { get; set; }        // nullable
        public float? DurationMinutes { get; set; }   // nullable
    }
}
