namespace _Scripts.Pickups
{
    public class Shield : Pickup
    {
        public sealed override void OnPickup()
        {
            Destroy(gameObject);
        }
    }
}
