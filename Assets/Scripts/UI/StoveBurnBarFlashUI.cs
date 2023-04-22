using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnBarFlashUI : MonoBehaviour {

    private const string IS_FLASHING = "IsFlashing";
     [SerializeField] private StoveCounter stoveCounter;
     [SerializeField] private Animator animator;

     private void Awake() {
        animator = GetComponent<Animator>();
     }
    private void Start() {
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgresChangedEventArgs e) {
        float burnShowProgressAmount = 0.5f;
        bool show = stoveCounter.IsFried && e.progressNormalized >= burnShowProgressAmount;
        animator.SetBool(IS_FLASHING,show);
    }
}
