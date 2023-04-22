using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour {
    
    [SerializeField] private StoveCounter stoveCounter;
    [SerializeField] private GameObject[] stoveCounterVisuals;

    private void Start() {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e) {
        bool visualState = e.state == StoveCounter.State.FRYING || e.state == StoveCounter.State.FRIED;
        for(int i = 0; i < stoveCounterVisuals.Length; i++) {
            stoveCounterVisuals[i].SetActive(visualState);
        }
    }
}
