namespace _Scripts.Models
{
    public class BestScore: Singleton<BestScore> , IScore
    {
        public int Coin { get; set; }
        public float Distance { get; set; }
        public int Time { get; set; }
        
        public void SetScore(int coin, float distance, int time)
        {
            Coin = coin;
            Distance = distance;
            Time = time;
        }
    }
}