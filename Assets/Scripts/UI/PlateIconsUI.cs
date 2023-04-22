using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateIconsUI : MonoBehaviour {
    
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private PlateIconSingleUI templatePlateIconSingleUI;

    private void Awake() {
        templatePlateIconSingleUI.gameObject.SetActive(false);
    }
    private void Start() {
        plateKitchenObject.OnIngredientsAdded += PlateIconsUI_OnIngredientsAdded;
    }

    private void PlateIconsUI_OnIngredientsAdded(object sender, PlateKitchenObject.OnIngredientsAddedEventArgs e) {
        for(int i = 1; i <transform.childCount; i++) {
            Destroy(transform.GetChild(i).gameObject);
        }

        for(int i = 0; i < plateKitchenObject.KitchenObjects.Count; i++) {
            PlateIconSingleUI plateIconSingleUI = Instantiate(templatePlateIconSingleUI,transform);
            plateIconSingleUI.gameObject.SetActive(true);
            plateIconSingleUI.SetIcon(plateKitchenObject.KitchenObjects[i].GetKitchenObjectSO);
        }
    } 
}
