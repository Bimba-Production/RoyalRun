namespace _Scripts.Pickups
{
    public sealed class Apple : Pickup
    {
        public override void OnPickup()
        {
            Destroy(gameObject);
        }
    }
}
