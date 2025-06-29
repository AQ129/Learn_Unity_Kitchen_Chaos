using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VolumeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI volumeText;
    [SerializeField] private Slider volumeSlider;

    private void Start()
    {
        volumeSlider.value = SoudManager.instance.GetVolume();
        UpdateVisual();
    }

    public void OnVolumeChanged(float volume)
    {
        SoudManager.instance.ChangeVolume(volumeSlider.value);
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        volumeText.text = "Sound Effect: " + (SoudManager.instance.GetVolume()).ToString() + "%";
    }
}
