public class Crown : Pickup
{
    public override void OnPickup()
    {
        Destroy(gameObject);
    }
}
