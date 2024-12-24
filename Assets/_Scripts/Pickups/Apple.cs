using UnityEngine;

public class Apple : Pickup
{
    public override void OnPickup()
    {
        Debug.Log("Power up!");
        Destroy(gameObject);
    }
}
