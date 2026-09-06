using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainBoostVR_API.Models
{
    public class Exercise
    {
        public int ExerciseID { get; set; }

        [Column("name")]
        public string Name { get; set; } = string.Empty;
    }
}