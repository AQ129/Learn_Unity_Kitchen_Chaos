using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI keyMoveUpText;
    [SerializeField] private TextMeshProUGUI keyMoveDownText;
    [SerializeField] private TextMeshProUGUI keyMoveLeftText;
    [SerializeField] private TextMeshProUGUI keyMoveRightText;
    [SerializeField] private TextMeshProUGUI keyInteractText;
    [SerializeField] private TextMeshProUGUI keyInteractAlternateText;
    [SerializeField] private TextMeshProUGUI keyPauseText;

    // Start is called before the first frame update
    void Start()
    {
        UpdateVisual();
        GameInput.instance.OnBindingRebind += GameInput_OnBindingRebind;
        Show();
        GameManager.instance.OnStateChanged += GameManager_OnStateChanged;
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.instance.IsCountdownToStartActive())
        {
            Hide();
        }
    }

    private void GameInput_OnBindingRebind(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        keyMoveUpText.text = GameInput.instance.GetBidingText(GameInput.Binding.Move_Up);
        keyMoveDownText.text = GameInput.instance.GetBidingText(GameInput.Binding.Move_Down);
        keyMoveLeftText.text = GameInput.instance.GetBidingText(GameInput.Binding.Move_Left);
        keyMoveRightText.text = GameInput.instance.GetBidingText(GameInput.Binding.Move_Right);
        keyInteractText.text = GameInput.instance.GetBidingText(GameInput.Binding.Interact);
        keyInteractAlternateText.text = GameInput.instance.GetBidingText(GameInput.Binding.InteractAlternate);
        keyPauseText.text = GameInput.instance.GetBidingText(GameInput.Binding.Pause);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
