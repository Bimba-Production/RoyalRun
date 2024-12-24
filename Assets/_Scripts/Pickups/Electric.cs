public class Electric : Pickup
{
    public override void OnPickup()
    {
        Destroy(gameObject);
    }
}
