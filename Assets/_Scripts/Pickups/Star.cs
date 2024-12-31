namespace Assets._Scripts
{
    public class Star : Pickup
    {
        public override void OnPickup()
        {
            Destroy(gameObject);
        }
    }
}