using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MusicUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI volumeText;
    [SerializeField] private Slider volumeSlider;

    private void Start()
    {
        volumeSlider.value = MusicManager.instance.GetVolume();
        UpdateVisual();
    }

    public void OnVolumeChanged(float volume)
    {
        MusicManager.instance.ChangeVolume(volumeSlider.value);
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        volumeText.text = "Music: " + (MusicManager.instance.GetVolume()).ToString() + "%";
    }
}
