 using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryManagerSingleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipeNameText;
    [SerializeField] private Transform iconTemplate;
    [SerializeField] private Transform iconContainer;

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }

    public void SetRecipeSOName(RecipeSO recipeSO)
    {
        recipeNameText.text = recipeSO.recipeName;
        foreach (Transform temp in iconContainer)
        {
            if(temp == iconTemplate)
            {
                continue;
            }
            Destroy(temp);
        }
        foreach(KitchenObjectSO kitchenObjectSO in recipeSO.kitchenObjectSOList)
        {
            Transform tempIcon = Instantiate(iconTemplate, iconContainer);
            tempIcon.gameObject.SetActive(true);
            tempIcon.GetComponent<Image>().sprite = kitchenObjectSO.sprite;
        }
    }
}
