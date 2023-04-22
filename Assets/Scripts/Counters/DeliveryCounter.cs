using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter {

    public static DeliveryCounter Instance { get; private set; }
    private void Awake() {
        Instance = this;
    }
    public override void Interact(PlayerController playerController) {
        if(!playerController.HasKitchenObject()) return;
        if(playerController.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
            DeliveryManager.Instance.DeliverRecipe(plateKitchenObject);
            KitchenObject.DestroyKitchenObject(plateKitchenObject);
        }
    }
   
}
