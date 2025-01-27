namespace _Scripts.Pickups
{
    public class Shield : Pickup
    {
        public sealed override void OnPickup()
        {
            EffectController.Instance.ApplyShieldEffect();
            Destroy(gameObject);
        }
    }
}
