namespace BrainBoostVR_API.Models
{
    public class User
    {
        public int UserID { get; set; }               // NOT NULL
		public required string FirebaseUID { get; set; } = string.Empty;     // NOT NULL
		public required string Name { get; set; } = string.Empty;              // NOT NULL
		public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
