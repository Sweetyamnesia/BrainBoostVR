namespace BrainBoostVR_API.Models
{
    public class UnityScoreDto
    {
        public string FirebaseUID { get; set; } = string.Empty; // identifie l'utilisateur
        public int Score { get; set; }
        public int Errors { get; set; }
        public float TimeSpent { get; set; }
    }
}
