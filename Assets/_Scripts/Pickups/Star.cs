namespace _Scripts.Pickups
{
    public class Star : Pickup
    {
        public override void OnPickup()
        {
            Destroy(gameObject);
        }
    }
}