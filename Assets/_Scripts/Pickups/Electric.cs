namespace Assets._Scripts
{
    public class Electric : Pickup
    {
        public override void OnPickup()
        {
            Destroy(gameObject);
        }
    }
}