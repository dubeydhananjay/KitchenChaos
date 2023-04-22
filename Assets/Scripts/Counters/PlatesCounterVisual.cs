using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour {
    
    [SerializeField] private PlatesCounter platesCounter;
    [SerializeField] private GameObject pfPlateVisual;
    [SerializeField] private Transform counterTopTransform;
    private List<Transform> plateTransforms;

    private void Awake() {
        plateTransforms = new List<Transform>();
    }

    private void Start() {
        platesCounter.OnPlateSpawned += PlatesCounter_OnPlateSpwaned;
        platesCounter.OnPlateRemoved += PlatesCounter_OnPlateRemoved;
    }

    private void PlatesCounter_OnPlateSpwaned(object sender, System.EventArgs e) {
        float plateOffsetY = 0.1f;
        Transform plateTransform = Instantiate(pfPlateVisual, counterTopTransform).transform;
        plateTransform.localPosition = new Vector3(0, plateOffsetY * plateTransforms.Count, 0);
        plateTransforms.Add(plateTransform);
    }

     private void PlatesCounter_OnPlateRemoved(object sender, System.EventArgs e) {
        Transform plateTransform = plateTransforms[plateTransforms.Count - 1];
        plateTransforms.Remove(plateTransform);
        Destroy(plateTransform.gameObject);
    }
}
