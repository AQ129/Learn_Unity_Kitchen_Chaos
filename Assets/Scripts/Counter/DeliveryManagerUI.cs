using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Transform recipeTemplate;

    private void Awake()
    {
        recipeTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        DeliveryManager.instance.OnRecipeCompleted += DeliveryManager_OnRecipeCompleted;
        DeliveryManager.instance.OnRecipeSpawned += DeliveryManager_OnRecipeSpawned;
    }

    private void DeliveryManager_OnRecipeSpawned(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void DeliveryManager_OnRecipeCompleted(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        foreach (Transform child in container)
        {
            if(child == recipeTemplate)
            {
                continue;
            }
            Destroy(child.gameObject);
        }

        foreach (RecipeSO recipeSO in DeliveryManager.instance.GetWaitingRecipeSOList())
        {
            Transform recipeTransform = Instantiate(recipeTemplate, container);
            recipeTransform.gameObject.SetActive (true);
            recipeTransform.GetComponent<DeliveryManagerSingleUI>().SetRecipeSOName(recipeSO);
        }
    }
}
