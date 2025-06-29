using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{    public static OptionsUI instance {  get; private set; }
    [SerializeField] private Button CloseBtn;
    [SerializeField] private Button MoveUpBtn;
    [SerializeField] private Button MoveDownBtn;
    [SerializeField] private Button MoveLeftBtn;
    [SerializeField] private Button MoveRightBtn;
    [SerializeField] private Button InteractBtn;
    [SerializeField] private Button InteractAlternateBtn;
    [SerializeField] private Button PauseBtn;
    private TextMeshProUGUI MoveUpText;
    private TextMeshProUGUI MoveDownText;
    private TextMeshProUGUI MoveLeftText;
    private TextMeshProUGUI MoveRightText;
    private TextMeshProUGUI InteractText;
    private TextMeshProUGUI InteractAlternateText;
    private TextMeshProUGUI PauseText;
    [SerializeField] private Transform PressToRebindKeyTransform;
    private Action onCloseButtonAction;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        MoveUpBtn.onClick.AddListener(() => {
            RebindBinding(GameInput.Binding.Move_Up);
        });
        MoveDownBtn.onClick.AddListener(() => {
            RebindBinding(GameInput.Binding.Move_Down);
        });
        MoveLeftBtn.onClick.AddListener(() => {
            RebindBinding(GameInput.Binding.Move_Left);
        });
        MoveRightBtn.onClick.AddListener(() => {
            RebindBinding(GameInput.Binding.Move_Right);
        });
        InteractBtn.onClick.AddListener(() => {
            RebindBinding(GameInput.Binding.Interact);
        });
        InteractAlternateBtn.onClick.AddListener(() => {
            RebindBinding(GameInput.Binding.InteractAlternate);
        });
        PauseBtn.onClick.AddListener(() => {
            RebindBinding(GameInput.Binding.Pause);
        });
    }
    // Start is called before the first frame update
    void Start()
    {
        MoveUpText = MoveUpBtn.GetComponentInChildren<TextMeshProUGUI>();
        MoveDownText = MoveDownBtn.GetComponentInChildren<TextMeshProUGUI>();
        MoveLeftText = MoveLeftBtn.GetComponentInChildren<TextMeshProUGUI>();
        MoveRightText = MoveRightBtn.GetComponentInChildren<TextMeshProUGUI>();
        InteractText = InteractBtn.GetComponentInChildren<TextMeshProUGUI>();
        InteractAlternateText = InteractAlternateBtn.GetComponentInChildren<TextMeshProUGUI>();
        PauseText = PauseBtn.GetComponentInChildren<TextMeshProUGUI>();
        Hide();
        CloseBtn.onClick.AddListener(() => {
            Hide();
            onCloseButtonAction();
        });
        GameManager.instance.OnGameUnPaused += GameManager_OnGameUnPaused;
        UpdateVisual();
        HidePressToRebendKey();
    }

    private void GameManager_OnGameUnPaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show(Action onCloseButtonAction)
    {
        this.onCloseButtonAction = onCloseButtonAction;
        gameObject.SetActive(true);
        MoveUpBtn.Select();
    }

    public void Hide()
    {
        gameObject.SetActive(false);  
    }

    private void UpdateVisual()
    {
        MoveUpText.text = GameInput.instance.GetBidingText(GameInput.Binding.Move_Up);
        MoveDownText.text = GameInput.instance.GetBidingText(GameInput.Binding.Move_Down);
        MoveLeftText.text = GameInput.instance.GetBidingText(GameInput.Binding.Move_Left);
        MoveRightText.text = GameInput.instance.GetBidingText(GameInput.Binding.Move_Right);
        InteractText.text = GameInput.instance.GetBidingText(GameInput.Binding.Interact);
        InteractAlternateText.text = GameInput.instance.GetBidingText(GameInput.Binding.InteractAlternate);
        PauseText.text = GameInput.instance.GetBidingText(GameInput.Binding.Pause);

    }

    private void ShowPressToRebindKey()
    {
        PressToRebindKeyTransform.gameObject.SetActive(true);
    }

    private void HidePressToRebendKey()
    {
        PressToRebindKeyTransform.gameObject.SetActive(false);
    }

    private void RebindBinding(GameInput.Binding binding)
    {
        ShowPressToRebindKey();
        GameInput.instance.RebindBinding(binding, () =>
        {
            HidePressToRebendKey();
            UpdateVisual();
        });
    }

}
