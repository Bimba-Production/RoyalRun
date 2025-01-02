namespace _Scripts.Pickups
{
    public class Shield : Pickup
    {
        public override void OnPickup()
        {
            Destroy(gameObject);
        }
    }
}
