namespace Assets._Scripts
{
    public class Coin : Pickup
    {
        public override void OnPickup()
        {
            Destroy(gameObject);
        }
    }
}
