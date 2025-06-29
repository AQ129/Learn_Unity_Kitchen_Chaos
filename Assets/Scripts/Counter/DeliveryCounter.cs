using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public static DeliveryCounter Instance {  get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public override void Interact(NewBehaviourScript player)
    {
        if (player.HasKitchenObject())
        {
            if (player.KitchenObject.TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {
                DeliveryManager.instance.DeliveryRecipe(plateKitchenObject);
                player.KitchenObject.DestroySelf();
            }
        }
    }
}
