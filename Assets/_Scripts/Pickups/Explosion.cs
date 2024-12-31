namespace Assets._Scripts
{
    public class Explosion : Pickup
    {
        public override void OnPickup()
        {
            Destroy(gameObject);
        }
    }
}
