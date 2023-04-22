using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter, IKitchenObjectParent {
    
    public override void Interact(PlayerController playerController) {
       if(!HasKitchenObject()) {
        if(playerController.HasKitchenObject()) {
            KitchenObject kitchenObject = playerController.GetKitchenObject();
            kitchenObject.SetKitchenObjectParent(this);
        }
       }
       else {
        if(!playerController.HasKitchenObject()) {
            GetKitchenObject().SetKitchenObjectParent(playerController);
        }
        else {
            if(playerController.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
                plateKitchenObject.AddIngredients(GetKitchenObject());
            }
            else if(GetKitchenObject().TryGetPlate(out plateKitchenObject)) {
                plateKitchenObject.AddIngredients(playerController.GetKitchenObject());
            }
        }
       }
    }
}
