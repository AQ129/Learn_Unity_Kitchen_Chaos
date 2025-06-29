using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    public static event EventHandler OnDestroy;

    new public static void ResetStaticData()
    {
        OnDestroy = null;
    }
    public override void Interact(NewBehaviourScript player)
    {
        if (player.HasKitchenObject())
        {
            player.KitchenObject.DestroySelf();
            OnDestroy?.Invoke(this, EventArgs.Empty);
        }
    }
}
