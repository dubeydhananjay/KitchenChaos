using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : AudioManager {
    
    private const string PLAYER_PREFS_SOUND_EFFECT_VOLUME = "SoundEffectVolume";
    [SerializeField] private AudioClipsSO audioClipsSO;
    public static SoundManager Instance { get; private set; }

    private void Awake() {
        Instance = this;
        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECT_VOLUME,1f);
    }
    private void Start() {
        DeliveryManager.Instance.OnRecipeFailure += DeliveryManager_OnRecipeFailure;
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        BaseCounter.OnAnyObjectPlacedHere += BaseCounter_OnAnyObjectPlacedHere;
        PlayerController.OnPlayerPickedObject += PlayerController_OnObjectPickup;
        TrashCounter.OnObjectTrashed += TrashCounter_OnObjectTrashed;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
    }

     private void PlayerController_OnObjectPickup(object sender, System.EventArgs e) {
        PlayerController player = sender as PlayerController;
        PlaySound(audioClipsSO.pickups, player.transform.position);
    }
    private void CuttingCounter_OnAnyCut(object sender, System.EventArgs e) {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(audioClipsSO.chops,cuttingCounter.transform.position);
    }
    private void TrashCounter_OnObjectTrashed(object sender, System.EventArgs e) {
        TrashCounter trashCounter = sender as TrashCounter;
        PlaySound(audioClipsSO.trashes,trashCounter.transform.position);
    }
    private void BaseCounter_OnAnyObjectPlacedHere(object sender, System.EventArgs e) {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(audioClipsSO.objectDrops,baseCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeFailure(object sender, System.EventArgs e) {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipsSO.deliveryFailures,deliveryCounter.transform.position);
    }
     private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e) {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipsSO.deliverySuccesses,deliveryCounter.transform.position);
    }
     private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f) {
        AudioSource.PlayClipAtPoint(audioClipArray[Random.Range(0,audioClipArray.Length)],position,volume);
    }
    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f) {
        AudioSource.PlayClipAtPoint(audioClip,position,volumeMultiplier * volume);
    }

    public void PlayFootStep(Vector3 position, float volume) {
        PlaySound(audioClipsSO.footSteps,position,volume);
    }

    public void PlayCountDownSound() {
        PlaySound(audioClipsSO.warnings, Vector3.zero);
    }

    public void PlayWarningSound(Vector3 position) {
        PlaySound(audioClipsSO.warnings,position);
    }

    public override void ChangeVolume(float volume) {
        base.ChangeVolume(volume);
        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECT_VOLUME,volume);
    }
}
