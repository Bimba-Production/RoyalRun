namespace _Scripts.Pickups.Models
{
    public class Electric : Pickup
    {
        public sealed override void OnPickup()
        {
            PowerUpIconController.Instance.ShowEvasion();
            EffectController.Instance.ApplyElectricalEffect();
            Destroy(gameObject);
        }
    }
}