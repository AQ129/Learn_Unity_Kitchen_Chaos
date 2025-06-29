using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Interact(NewBehaviourScript player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                player.KitchenObject.SetKitchentObjectParent(this);
            }
        }
        else 
        {
            if (!player.HasKitchenObject())
            {
                KitchenObject.SetKitchentObjectParent(player);
            }
            else if (player.KitchenObject.TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {
                if (plateKitchenObject.tryAddIngredient(KitchenObject.GetKitchenObjectSO()))
                {
                    KitchenObject.DestroySelf();
                }
            }
            else if(player.HasKitchenObject() && KitchenObject.TryGetPlate(out plateKitchenObject))
            {
                if (plateKitchenObject.tryAddIngredient(player.KitchenObject.GetKitchenObjectSO()))
                {
                    player.KitchenObject.DestroySelf();
                }
            }
        }
    }
}
