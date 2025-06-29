using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] private Transform counterTopPoint;
    private KitchenObject kitchenObject;
    public static event EventHandler OnDroppedSomething;

    public static void ResetStaticData()
    {
        OnDroppedSomething = null;
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return counterTopPoint;
    }

    public KitchenObject KitchenObject 
    { 
        get
        {
            return kitchenObject;
        }
        set 
        {
            kitchenObject = value;
            if (counterTopPoint != null)
            {
                OnDroppedSomething?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void ClearKitchenObject()
    {
        KitchenObject = null;
    }

    public bool HasKitchenObject() { return KitchenObject != null; }

    public virtual void Interact(NewBehaviourScript Player)
    {
        Debug.LogError("BaseCounter.Interact()");
    }

    public virtual void InteractAlternate(NewBehaviourScript player)
    {
        Debug.LogError("BaseCounter.InteractAlternate()");
    }
}
