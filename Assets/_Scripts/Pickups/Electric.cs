namespace _Scripts.Pickups
{
    public class Electric : Pickup
    {
        public sealed override void OnPickup()
        {
            Destroy(gameObject);
        }
    }
}