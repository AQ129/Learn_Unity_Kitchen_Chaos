using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class SoudManager : MonoBehaviour
{
    private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";
    [SerializeField] private AudioClipRefsSO audioClipRefsSO;
    private float volume;

    public static SoudManager instance { get; private set; }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, 1f);
    }

    private void Start()
    {
        DeliveryManager.instance.OnRecipeSuccess += Delivery_OnRecipeSuccess;
        DeliveryManager.instance.OnRecipeFailed += Delivery_OnRecipeFailed;
        CuttingCounter.OnAnyCutting += CuttingCounter_OnAnyCutting;
        NewBehaviourScript.Instance.OnPickedSomething += Player_OnPickedSomething;
        BaseCounter.OnDroppedSomething += Player_OnDroppedSomething;
        TrashCounter.OnDestroy += TrashCounter_OnDestroy;
        PlayerSounds.OnPlayerMoving += PlayerSounds_OnPlayerMoving;
    }

    private void PlayerSounds_OnPlayerMoving(object sender, System.EventArgs e)
    {
        PlayerSounds playerSounds = (PlayerSounds)sender;
        NewBehaviourScript player = playerSounds.GetComponent<NewBehaviourScript>();
        PlaySound(audioClipRefsSO.footstep, player.transform.position);
    }

    private void TrashCounter_OnDestroy(object sender, System.EventArgs e)
    {
        TrashCounter trashCounter = (TrashCounter)sender;
        PlaySound(audioClipRefsSO.trash, trashCounter.transform.position);
    }

    private void Player_OnDroppedSomething(object sender, System.EventArgs e)
    {
        BaseCounter counter = (BaseCounter)sender;
        PlaySound(audioClipRefsSO.objectDrop, counter.transform.position);        
    }

    private void Player_OnPickedSomething(object sender, System.EventArgs e)
    {
        NewBehaviourScript player = sender as NewBehaviourScript;
        PlaySound(audioClipRefsSO.objectPickup, player.transform.position);
    }

    private void CuttingCounter_OnAnyCutting(object sender, System.EventArgs e)
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(audioClipRefsSO.chop, cuttingCounter.transform.position);
    }

    private void Delivery_OnRecipeFailed(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.deliveryFail, DeliveryCounter.Instance.transform.position);
    }

    private void Delivery_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.deliverySuccess, DeliveryCounter.Instance.transform.position);
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * volume);
    }

    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f)
    {
        PlaySound(audioClipArray[Random.Range(0,audioClipArray.Length)], position, volume);
    }

    public void PlayCountDownSound()
    {
        PlaySound(audioClipRefsSO.warning, Vector3.zero);
    }

    public void PlayWarningSound(Vector3 porsition)
    {
        PlaySound(audioClipRefsSO.warning, porsition);
    }

    public void ChangeVolume(float volumeValue)
    {
        volume = volumeValue/100;
        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return volume*100;
    }
    
    public void SetVolume(float volume)
    {
        this.volume = volume;
    }
}
