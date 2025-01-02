namespace _Scripts.Pickups
{
    public class Apple : Pickup
    {
        public override void OnPickup()
        {
            Destroy(gameObject);
        }
    }
}
