namespace _Scripts.Pickups
{
    public class Electric : Pickup
    {
        public override void OnPickup()
        {
            Destroy(gameObject);
        }
    }
}