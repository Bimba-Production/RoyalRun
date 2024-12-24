using UnityEngine;

public class Coin : Pickup
{
    public override void OnPickup()
    {
        //Debug.Log("Add coin");
        Destroy(gameObject);
    }
}
