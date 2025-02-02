namespace _Scripts.Pickups.Models
{
    public class Explosion : Pickup
    {
        public sealed override void OnPickup()
        {
            Destroy(gameObject);
        }
    }
}
