public class Explosion : Pickup
{
    public override void OnPickup()
    {
        Destroy(gameObject);
    }
}
