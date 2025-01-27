namespace _Scripts.Pickups
{
    public class Explosion : Pickup
    {
        public sealed override void OnPickup()
        {
            Destroy(gameObject);
        }
    }
}
