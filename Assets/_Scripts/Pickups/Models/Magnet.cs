namespace _Scripts.Pickups.Models
{
    public class Magnet : Pickup
    {
        public sealed override void OnPickup()
        {
            EffectController.Instance.ApplyMagnetEffect();
            PowerUpIconController.Instance.ShowMagnet();
            MagnetHandler.Instance.EnableMagneting();
            
            Destroy(gameObject);
        }
    }
}
