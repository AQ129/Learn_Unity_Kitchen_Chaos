using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    private PlayerInputAction action;
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternate;
    public event EventHandler OnPause;
    public event EventHandler OnBindingRebind;
    private const string PLAYER_PREFS_BINDINGS = "InputBindings";
    public enum Binding
    {
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Interact,
        InteractAlternate,
        Pause,
    }

    public static GameInput instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        action = new PlayerInputAction();
        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))
        {
            action.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        }
        action.Player.Enable();
        action.Player.Interact.performed += Interact_performed;
        action.Player.InteractAlternate.performed += InteractAlternate_performed;
        action.Player.Pause.performed += Pause_performed;
    }

    private void OnDestroy()
    {
        //action.Dispose();
        action.Player.Interact.performed -= Interact_performed;
        action.Player.InteractAlternate.performed -= InteractAlternate_performed;
        action.Player.Pause.performed -= Pause_performed;
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPause?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAlternate?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector3 GetMovermentVectorNomalized()
    {
        Vector2 tempVector = action.Player.Move.ReadValue<Vector2>();
        Vector3 inputVector = new Vector3(tempVector.x, 0, tempVector.y);
        inputVector = inputVector.normalized;
        return inputVector;
    }

    public string GetBidingText(Binding biding)
    {
        switch (biding) {
            default:
            case Binding.Interact:
                return action.Player.Interact.bindings[0].ToDisplayString();
            case Binding.InteractAlternate:
                return action.Player.InteractAlternate.bindings[0].ToDisplayString();
            case Binding.Pause:
                return action.Player.Pause.bindings[0].ToDisplayString();
            case Binding.Move_Up:
                return action.Player.Move.bindings[1].ToDisplayString();
            case Binding.Move_Down:
                return action.Player.Move.bindings[2].ToDisplayString();
            case Binding.Move_Left:
                return action.Player.Move.bindings[3].ToDisplayString();
            case Binding.Move_Right:
                return action.Player.Move.bindings[4].ToDisplayString();
        }
    }

    private void SaveBinding()
    {
        PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, action.SaveBindingOverridesAsJson());
        PlayerPrefs.Save();
    }

    public void RebindBinding(Binding binding, Action actionOnRebound)
    {
        int indexBinding;
        action.Player.Disable();
        switch (binding)
        {
            case Binding.Move_Up:
                indexBinding = 1;
                break;
            case Binding.Move_Down:
                indexBinding = 2;
                break;
            case Binding.Move_Left:
                indexBinding = 3;
                break;
            case Binding.Move_Right:
                indexBinding = 4;
                break;
            default:
                indexBinding = 0;
                break;
        }
        if(indexBinding != 0)
        {
            action.Player.Move.PerformInteractiveRebinding(indexBinding)
            .OnComplete(callback =>
            {
                action.Player.Enable();
                callback.Dispose();
                actionOnRebound();
                SaveBinding();
                
            })
            .Start();
        }
        else if (binding == Binding.Interact)
        {
            action.Player.Interact.PerformInteractiveRebinding(indexBinding)
            .OnComplete(callback =>
            {
                action.Player.Enable();
                callback.Dispose();
                actionOnRebound();
                SaveBinding();
            })
            .Start();
        }
        else if(binding == Binding.InteractAlternate)
        {
            action.Player.InteractAlternate.PerformInteractiveRebinding(indexBinding)
            .OnComplete(callback =>
            {
                action.Player.Enable();
                callback.Dispose();
                actionOnRebound();
                SaveBinding();
            })
            .Start();
        }
        else
        {
            action.Player.Pause.PerformInteractiveRebinding(indexBinding)
            .OnComplete(callback =>
            {
                action.Player.Enable();
                callback.Dispose();
                actionOnRebound();
                SaveBinding();
            })
            .Start();
        }
        OnBindingRebind?.Invoke(this, EventArgs.Empty);
    }
}
