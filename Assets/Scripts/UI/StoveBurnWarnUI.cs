using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnWarnUI : MonoBehaviour {

    [SerializeField] private StoveCounter stoveCounter;
    private void Start() {
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
        gameObject.SetActive(false);
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgresChangedEventArgs e) {
        float burnShowProgressAmount = 0.5f;
        bool show = stoveCounter.IsFried && e.progressNormalized >= burnShowProgressAmount;
        gameObject.SetActive(show); 
    }

    public void PlayWarningSound() {
        SoundManager.Instance.PlayWarningSound(transform.position);
    }
}
