using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateDestroyed;
    private int platesSpawnedAmount;
    private int platesSpawnedAmountMax = 4;
    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.IsGamePlaying())
        {
            if (platesSpawnedAmount < platesSpawnedAmountMax)
            {
                spawnPlateTimer += Time.deltaTime;
                if (spawnPlateTimer > spawnPlateTimerMax)
                {
                    spawnPlateTimer = 0;
                    platesSpawnedAmount += 1;
                    OnPlateSpawned?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }

    public override void Interact(NewBehaviourScript player)
    {
        if (!player.HasKitchenObject() && platesSpawnedAmount > 0)
        {
            platesSpawnedAmount--;
            KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
            OnPlateDestroyed?.Invoke(this, EventArgs.Empty);
        }
    }
}
