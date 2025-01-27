using _Scripts.Level;

namespace _Scripts.Pickups
{
    public sealed class Crown : Pickup
    {
        public override void OnPickup()
        {
            LevelGenerator.Instance.SlowDownTheLevel();
            Destroy(gameObject);
        }
    }
}