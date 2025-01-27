namespace _Scripts.Pickups
{
    public sealed class Star : Pickup
    {
        public override void OnPickup()
        {
            Destroy(gameObject);
        }
    }
}