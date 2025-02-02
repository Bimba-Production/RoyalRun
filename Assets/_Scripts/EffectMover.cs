using _Scripts.Level;
using UnityEngine;

namespace _Scripts
{
    public sealed class EffectMover: MonoBehaviour
    {
        private void Update() => Move();

        private void Move()
        {
            transform.position = new Vector3(transform.position.x, transform.position.y,
                transform.position.z - LevelGenerator.Instance.Speed * Time.deltaTime);
        }
    }
}