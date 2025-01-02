using _Scripts.Level;

namespace _Scripts.Pickups
{
    public class Crown : Pickup
    {
        public override void OnPickup()
        {
            LevelGenerator.Instance.SlowDownTheLevel();
            Destroy(gameObject);
        }
    }
}