using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private NewBehaviourScript player;
    private float footstepTimer;
    private float footstepTimerMax = .1f;
    public static event EventHandler OnPlayerMoving;

    public static void ResetStaticData()
    {
        OnPlayerMoving = null;
    }

    private void Awake()
    {
        player = GetComponent<NewBehaviourScript>();
    }

    private void Update()
    {
        footstepTimer -= Time.deltaTime;
        if (footstepTimer < 0)
        {
            footstepTimer = footstepTimerMax;
            if (player.IsWalking())
            {
                OnPlayerMoving?.Invoke(this, new EventArgs());
            }
        }
    }
}
