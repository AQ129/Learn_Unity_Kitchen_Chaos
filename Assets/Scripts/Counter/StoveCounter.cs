using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StoveCounter : BaseCounter, IHasProgress
{
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;
    private float fryingTimerMax;
    private float fryingTimer = 0;
    private float burningTimerMax;
    private float burningTimer = 0;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }


    public enum State
    {
        Off,
        Idle,
        Frying,
        Fried,
        Burned,
    }

    private State state;
    public override void Interact(NewBehaviourScript player)
    {
        if (player.HasKitchenObject() && !HasKitchenObject())
        {
            if (GetOutputForInputFrying(player.KitchenObject.GetKitchenObjectSO()) != null)
            {
                player.KitchenObject.SetKitchentObjectParent(this);
                fryingTimerMax = GetFryingProgressMax(KitchenObject.GetKitchenObjectSO());
                if(state == State.Idle)
                {
                    state = State.Frying;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                    fryingTimer = 0;
                }
            }
            else if(GetOutputForInputBurning(player.KitchenObject.GetKitchenObjectSO()) != null)
            {
                player.KitchenObject.SetKitchentObjectParent(this);
                burningTimerMax = GetBurningProgressMax(KitchenObject.GetKitchenObjectSO());
                if (state == State.Idle)
                {
                    state = State.Fried;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                    burningTimer = 0;
                }
            }
        }
        else if(!player.HasKitchenObject() && HasKitchenObject())
        {
            KitchenObject.SetKitchentObjectParent(player);
            if (state != State.Off && state != State.Burned)
            {
                state = State.Idle;
            }
            else
            {
                state = State.Off;
            }
            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
            burningTimer = 0;
            fryingTimer = 0;
        }
        else if (player.HasKitchenObject() && player.KitchenObject.TryGetPlate(out PlateKitchenObject plateKitchenObject))
        {
            if (plateKitchenObject.tryAddIngredient(KitchenObject.GetKitchenObjectSO()))
            {
                KitchenObject.DestroySelf();
                if(state != State.Burned)
                {
                    state = State.Idle;
                }
                else
                {
                    state = State.Off;
                }
                burningTimer = 0;
                fryingTimer = 0;
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
            }
        }
    }

    public override void InteractAlternate(NewBehaviourScript player)
    {
        if (state != State.Off)
        {
            fryingTimer = 0;
            burningTimer = 0;
            state = State.Off;
            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
        }
        else if (!HasKitchenObject())
        {
            fryingTimer = 0;
            burningTimer = 0;
            state = State.Idle;
            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
        }
        else if (GetOutputForInputBurning(KitchenObject.GetKitchenObjectSO()) != null)
        {
            state = State.Fried;
            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
            burningTimer = 0;
        }
        else if (GetOutputForInputFrying(KitchenObject.GetKitchenObjectSO()) != null)
        {
            state = State.Frying;
            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
            fryingTimer = 0;
        }
        else
        {
            state = State.Off;
            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
        }
    }


    private KitchenObjectSO GetOutputForInputFrying(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO temp in fryingRecipeSOArray)
        {
            if (temp.input == inputKitchenObjectSO)
            {
                return temp.output;
            }
        }
        return null;
    }

    private KitchenObjectSO GetOutputForInputBurning(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (BurningRecipeSO temp in burningRecipeSOArray)
        {
            if (temp.input == inputKitchenObjectSO)
            {
                return temp.output;
            }
        }
        return null;
    }

    private float GetFryingProgressMax(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO temp in fryingRecipeSOArray)
        {
            if (temp.input == inputKitchenObjectSO)
            {
                return temp.fryingTimerMax;
            }
        }
        return -1;
    }

    private float GetBurningProgressMax(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (BurningRecipeSO temp in burningRecipeSOArray)
        {
            if (temp.input == inputKitchenObjectSO)
            {
                return temp.burningTimerMax;
            }
        }
        return -1;
    }

    private void Update()
    {
        switch (state)
        {
            case State.Off:
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0
                });
                break;
            case State.Idle:
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0
                });
                break;
            case State.Frying:
                fryingTimer += Time.deltaTime;
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                    progressNormalized = fryingTimer / fryingTimerMax
                });
                if (fryingTimer >= fryingTimerMax)
                {
                    fryingTimer = 0;
                    KitchenObjectSO tempSO = GetOutputForInputFrying(KitchenObject.GetKitchenObjectSO());
                    KitchenObject.DestroySelf();
                    KitchenObject.SpawnKitchenObject(tempSO, this);
                    burningTimerMax = GetBurningProgressMax(KitchenObject.GetKitchenObjectSO());
                    state = State.Fried;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                }
                break;
            case State.Fried:
                burningTimer += Time.deltaTime;
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = burningTimer / burningTimerMax
                });
                if (burningTimer >= burningTimerMax)
                {
                    burningTimer = 0;
                    KitchenObjectSO tempSO = GetOutputForInputBurning(KitchenObject.GetKitchenObjectSO());
                    KitchenObject.DestroySelf();
                    KitchenObject.SpawnKitchenObject(tempSO, this);
                    state = State.Burned;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                }
                break;
            case State.Burned:
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0
                });
                break;
        }
    }

    public bool IsFried()
    {
        return state == State.Fried;
    }

    private void Start()
    {
        state = State.Off;
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
    }
}
