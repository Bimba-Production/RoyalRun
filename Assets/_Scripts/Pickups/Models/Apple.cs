namespace _Scripts.Pickups.Models
{
    public sealed class Apple : Pickup
    {
        public override void OnPickup()
        {
            Destroy(gameObject);
        }
    }
}
