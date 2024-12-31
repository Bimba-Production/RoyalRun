namespace Assets._Scripts
{
    public class Shield : Pickup
    {
        public override void OnPickup()
        {
            Destroy(gameObject);
        }
    }
}
