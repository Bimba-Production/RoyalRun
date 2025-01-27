namespace _Scripts.Models
{
    public class Score : Singleton<Score>
    {
        public int coin;
        public float distance;
        public int time;

        public Score(int coin, float distance, int time)
        {
            this.coin = coin;
            this.distance = distance;
            this.time = time;
        }

        public void SetScore(int coin, float distance, int time)
        {
            this.coin = coin;
            this.distance = distance;
            this.time = time;
        }
    }
}