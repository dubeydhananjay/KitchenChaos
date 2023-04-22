using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour {

    [SerializeField] private float footStepTimerMax = 0.1f;
    private float footStepTimer;
    private void Awake() {
        footStepTimer = footStepTimerMax;
    }

    private void Update() {
        footStepTimer -= Time.deltaTime;
        if (footStepTimer <= 0) {
            footStepTimer = footStepTimerMax;
            // if (PlayerController.Instance.IsWalking) {
            //     float volume = 1f;
            //     SoundManager.Instance.PlayFootStep(transform.position, volume);
            // }
        }
    }
}
