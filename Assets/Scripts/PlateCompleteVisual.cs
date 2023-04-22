using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour {
    [Serializable]
    public struct KitchenObjectPlacement {
        public KitchenObjectSO kitchenObjectSO;
        public float posY;
    }

    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private List<KitchenObjectPlacement> kitchenObjectPlacements;


    private void Start() {
        plateKitchenObject.OnIngredientsAdded += PlateKitchenObject_OnIngredientsAdded;
    }

    private void PlateKitchenObject_OnIngredientsAdded(object sender, PlateKitchenObject.OnIngredientsAddedEventArgs e) {
        for(int i = 0; i < kitchenObjectPlacements.Count; i++) {
            if(kitchenObjectPlacements[i].kitchenObjectSO == e.kitchenObject.GetKitchenObjectSO) {
                e.kitchenObject.transform.parent = transform;
                Vector3 placingPos = Vector3.zero;
                placingPos.y = kitchenObjectPlacements[i].posY;
                e.kitchenObject.transform.localPosition = placingPos;
                e.kitchenObject.ClearParent();
            }
        }
    }

}
