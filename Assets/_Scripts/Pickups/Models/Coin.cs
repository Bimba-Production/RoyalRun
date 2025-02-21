using _Scripts.UI;

namespace _Scripts.Pickups.Models
{
    public sealed class Coin : Pickup
    {
        public override void OnPickup()
        {
            CoinDisplay.Instance.IncreaseScore(1);
            Destroy(gameObject);
        }
    }
}