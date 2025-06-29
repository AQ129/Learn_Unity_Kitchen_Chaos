using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCounterVisual : MonoBehaviour
{
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform plateVisualPrefab;
    [SerializeField] private PlateCounter PlateCounter;
    private List<GameObject> plates = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        PlateCounter.OnPlateSpawned += PlateCounter_OnPlateSpawned;
        PlateCounter.OnPlateDestroyed += PlateCounter_OnPlateDestroyed;
    }

    private void PlateCounter_OnPlateDestroyed(object sender, System.EventArgs e)
    {
        Destroy(plates[plates.Count - 1]);
        plates.Remove(plates[plates.Count - 1]);    
    }

    private void PlateCounter_OnPlateSpawned(object sender, System.EventArgs e)
    {
        Transform plateVisualTransform = Instantiate(plateVisualPrefab, counterTopPoint);
        float plateOffSetY = .1f;
        plateVisualTransform.localPosition = new Vector3(0, plateOffSetY * plates.Count, 0);
        plates.Add(plateVisualTransform.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
