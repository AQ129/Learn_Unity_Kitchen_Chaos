using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CuttingCounter : BaseCounter, IHasProgress
{
    public static event EventHandler OnAnyCutting;
    public event EventHandler OnCutting;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    private int cuttingProgress;
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSO;

    new public static void ResetStaticData()
    {
        OnAnyCutting = null;
    }

    public override void Interact(NewBehaviourScript player)
    {
        if (player.HasKitchenObject() && !HasKitchenObject())
        {
            if (GetOutputForInput(player.KitchenObject.GetKitchenObjectSO()) != null)
            {
                player.KitchenObject.SetKitchentObjectParent(this);
                cuttingProgress = 0;
                int tempCuttingProgress = GetCuttingProgressMax(KitchenObject.GetKitchenObjectSO());
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = (float)cuttingProgress / tempCuttingProgress
                });
            }
        }
        else if(!player.HasKitchenObject() && HasKitchenObject())
        {
            KitchenObject.SetKitchentObjectParent(player);
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = 0
            }); 
        }
        else if (player.HasKitchenObject() && player.KitchenObject.TryGetPlate(out PlateKitchenObject plateKitchenObject))
        {
            if (plateKitchenObject.tryAddIngredient(KitchenObject.GetKitchenObjectSO()))
            {
                KitchenObject.DestroySelf();
            }
        }
    }
        
    public override void InteractAlternate(NewBehaviourScript player)
    {

        if (HasKitchenObject())
        {
            KitchenObjectSO temp = GetOutputForInput(KitchenObject.GetKitchenObjectSO());
            int tempCuttingProgress = GetCuttingProgressMax(KitchenObject.GetKitchenObjectSO());
            if (temp != null)
            {
                cuttingProgress++;
                OnAnyCutting?.Invoke(this, EventArgs.Empty);
                OnCutting?.Invoke(this, EventArgs.Empty);
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = (float)cuttingProgress / tempCuttingProgress
                });
            }
            if (cuttingProgress >= tempCuttingProgress && tempCuttingProgress > -1)
            {
                KitchenObject.DestroySelf();
                KitchenObject.SpawnKitchenObject(temp, this);
            }
        }
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO temp in cuttingRecipeSO)
        {
            if(temp.input == inputKitchenObjectSO)
            {
                return temp.output;
            }
        }
        return null;
    }

    private int GetCuttingProgressMax(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach(CuttingRecipeSO temp in cuttingRecipeSO)
        {
            if(temp.input == inputKitchenObjectSO)
            {
                return temp.cuttingProgressMax;
            }
        }
        return -1;
    }
}
