namespace _Scripts.Pickups
{
    public class Explosion : Pickup
    {
        public override void OnPickup()
        {
            Destroy(gameObject);
        }
    }
}
