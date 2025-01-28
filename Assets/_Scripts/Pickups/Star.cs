using _Scripts.Level;

namespace _Scripts.Pickups
{
    public sealed class Star : Pickup
    {
        public override void OnPickup()
        {
            PowerUpIconController.Instance.ShowDoubleCoin();
            EffectController.Instance.ApplySpeedUpEffect();
            LevelGenerator.Instance.ApplyAcceleration(4f);
            Destroy(gameObject);
        }
    }
}