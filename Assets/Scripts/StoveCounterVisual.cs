using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    [SerializeField] private GameObject stoveOnVisual;
    [SerializeField] private GameObject sizzlingParticles;
    // Start is called before the first frame update
    void Start()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        if(e.state == StoveCounter.State.Off || e.state == StoveCounter.State.Burned)
        {
            stoveOnVisual.SetActive(false);
            sizzlingParticles.SetActive(false);
        }
        else { 
            stoveOnVisual.SetActive(true);
            if(e.state != StoveCounter.State.Idle)
            {
                sizzlingParticles.SetActive(true);
            }
            else
            {
                sizzlingParticles.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
