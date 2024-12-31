namespace Assets._Scripts
{
    public class Apple : Pickup
    {
        public override void OnPickup()
        {
            Destroy(gameObject);
        }
    }
}
