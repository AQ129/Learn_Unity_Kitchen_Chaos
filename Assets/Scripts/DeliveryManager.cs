using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeFailed;
    public event EventHandler OnRecipeSuccess;
    public static DeliveryManager instance { get; private set; }
    [SerializeField] private RecipeListSO recipeListSO;
    private List<RecipeSO> waitingRecipeSOList;
    private float spawnRecipeTimer = 0;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipeMax = 4;
    private int successfulRecipesAmount = 0;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        waitingRecipeSOList = new List<RecipeSO>();
    }

    private void Update()
    {
        if (GameManager.instance.IsGamePlaying())
        {
            spawnRecipeTimer -= Time.deltaTime;
            if (spawnRecipeTimer <= 0)
            {
                spawnRecipeTimer = spawnRecipeTimerMax;
                if (waitingRecipeSOList.Count < waitingRecipeMax)
                {
                    RecipeSO recipeSO = recipeListSO.RecipeSOList[UnityEngine.Random.Range(0, recipeListSO.RecipeSOList.Count)];
                    waitingRecipeSOList.Add(recipeSO);
                    OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        
    }

    public void DeliveryRecipe(PlateKitchenObject plateKitchenObject)
    {
        bool numberCheck = false;
        bool detailCheck = true;
        int numberOrder = 0;
        for (int i = 0; i < waitingRecipeSOList.Count; i++) 
        {
            if (plateKitchenObject.GetKitchenObjectList().Count == waitingRecipeSOList[i].kitchenObjectSOList.Count)
            {
                numberOrder = i;
                numberCheck = true;
                detailCheck = true;
                foreach (KitchenObjectSO kitchenObjectSO in waitingRecipeSOList[i].kitchenObjectSOList)
                {
                    bool detail = false;
                    foreach (KitchenObjectSO kitchenObject in plateKitchenObject.GetKitchenObjectList())
                    {
                        if (kitchenObject == kitchenObjectSO)
                        {
                            detail = true;
                            break;
                        }
                    }
                    if (!detail)
                    {
                        detailCheck = false;
                        numberCheck = false;
                        break;
                    }
                }
            }
        }
        if(numberCheck && detailCheck)
        {
            waitingRecipeSOList.RemoveAt(numberOrder);
            OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
            OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
            successfulRecipesAmount++;
        }
        else
        {
            OnRecipeFailed?.Invoke(this, EventArgs.Empty);
        }
    }

    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return waitingRecipeSOList;
    }

    public int GetSuccessfulRecipesAmount()
    {
        return successfulRecipesAmount;
    }
}
