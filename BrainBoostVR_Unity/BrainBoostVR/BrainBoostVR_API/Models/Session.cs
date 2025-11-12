public class Session
{
    public int SessionID { get; set; }
    public int UserID { get; set; }
	public string? Pseudo { get; set; } = string.Empty;
    public string FirebaseUID { get; set; } = string.Empty;  // ajouté
    public string SessionUid { get; set; } = string.Empty;   // ajouté
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public float? DurationMinutes { get; set; }
    public int Score { get; set; } = 0;
    public int Errors { get; set; } = 0;
}
