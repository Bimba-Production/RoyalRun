namespace _Scripts.Models
{
    public class CurrentScore: Singleton<CurrentScore> , IScore
    {
        public int Coin { get; set; }
        public float Distance { get; set; }
        public int Time { get; set; }
    }
}