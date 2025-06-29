using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryResultUI : MonoBehaviour
{
    [SerializeField] Image BackgoundSuccess;
    [SerializeField] Image IconSuccess;
    [SerializeField] TextMeshProUGUI textSuccess;
    [SerializeField] Image BackgoundFailed;
    [SerializeField] Image IconFailed;
    [SerializeField] TextMeshProUGUI textFailed;
    private const string POP_UP = "PopUp";
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        DeliveryManager.instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        Hide();
    }

    private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e)
    {
        ShowFailed();
        Show();
        animator.SetTrigger(POP_UP);
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        ShowSuccess();
        Show();
        animator.SetTrigger(POP_UP);
    }
    
    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void ShowSuccess()
    {
        BackgoundSuccess.gameObject.SetActive(true);
        IconSuccess.gameObject.SetActive(true); 
        textSuccess.gameObject.SetActive(true);
        BackgoundFailed.gameObject.SetActive(false);
        textFailed.gameObject.SetActive(false);
        IconFailed.gameObject.SetActive(false);
    }

    private void ShowFailed()
    {
        BackgoundSuccess.gameObject.SetActive(false);
        IconSuccess.gameObject.SetActive(false);
        textSuccess.gameObject.SetActive(false);
        BackgoundFailed.gameObject.SetActive(true);
        textFailed.gameObject.SetActive(true);
        IconFailed.gameObject.SetActive(true);
    }
}
