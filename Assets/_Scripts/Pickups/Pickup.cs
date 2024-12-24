using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;

    void Update()
    {
        float rotationAngle = _rotationSpeed * Time.deltaTime;
        transform.Rotate(0, rotationAngle, 0);     
    }

    public abstract void OnPickup();
}
