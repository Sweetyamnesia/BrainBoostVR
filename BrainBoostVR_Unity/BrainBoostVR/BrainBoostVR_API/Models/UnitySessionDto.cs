using System;

namespace BrainBoostVR_API.Models
{
	public class UnitySessionDto
	{
		public string FirebaseUID { get; set; } = string.Empty; // valeur par défaut
		public string SessionUid { get; set; } = string.Empty;   // valeur par défaut
		public float DurationMinutes { get; set; } = 0f;
		public string StartTime { get; set; } = string.Empty;   // valeur par défaut
		public string EndTime { get; set; } = string.Empty;     // valeur par défaut
		public int Score { get; set; } = 0;
		public int Errors { get; set; } = 0;
	}
	
	// DTO pour compléter une session (fin de session)
	public class UnitySessionCompleteDto
	{
		public string FirebaseUID { get; set; } = string.Empty; // valeur par défaut
		public string SessionUid { get; set; } = string.Empty;   // valeur par défaut
	}
}
