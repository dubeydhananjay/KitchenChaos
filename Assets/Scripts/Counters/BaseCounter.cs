using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BaseCounter : NetworkBehaviour, IKitchenObjectParent {

    public static event System.EventHandler OnAnyObjectPlacedHere;
    [SerializeField] protected Transform counterTopPoint;
    private KitchenObject kitchenObject;
    public virtual void Interact(PlayerController playerController) {

    }

    public virtual void InteractAlternate(PlayerController playerController) {

    }

    
    public void ClearKitchenObject() {
        kitchenObject = null;
    }

    public KitchenObject GetKitchenObject() {
        return kitchenObject;
    }

    public Transform GetKitchenObjectFollowTransform() {
        return counterTopPoint;
    }

    public bool HasKitchenObject() {
        return kitchenObject != null;
    }

    public void SetKitchenObject(KitchenObject kitchenObject) {
        this.kitchenObject = kitchenObject;
        if(kitchenObject != null) {
            OnAnyObjectPlacedHere?.Invoke(this, System.EventArgs.Empty);
        }
    }

    public NetworkObject GetNetworkObject() {
        return NetworkObject;
    }

    public static void ResetStaticData() {
        OnAnyObjectPlacedHere = null;
    }
}
