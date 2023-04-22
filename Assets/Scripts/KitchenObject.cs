using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class KitchenObject : NetworkBehaviour {
    
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    private IKitchenObjectParent kitchenObjectParent;
    public IKitchenObjectParent KitchenObjectParent => kitchenObjectParent;
    public KitchenObjectSO GetKitchenObjectSO => kitchenObjectSO;
    private FollowTransform followTransform;

    protected virtual void Awake() {
        followTransform = GetComponent<FollowTransform>();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetKitchenObjectParentServerRpc(NetworkObjectReference kitchenObjectParentNetworkObjectRef) {
        SetKitchenObjectParentClientRpc(kitchenObjectParentNetworkObjectRef);
    }

    [ClientRpc]
    private void SetKitchenObjectParentClientRpc(NetworkObjectReference kitchenObjectParentNetworkObjectRef) {

        ClearParent();
        kitchenObjectParentNetworkObjectRef.TryGet(out NetworkObject networkObject);
        IKitchenObjectParent kitchenObjectParent = networkObject.GetComponent<IKitchenObjectParent>();
        this.kitchenObjectParent = kitchenObjectParent;
        kitchenObjectParent.SetKitchenObject(this);
        followTransform.SetFollowTargetTransform(kitchenObjectParent.GetKitchenObjectFollowTransform());
    }    

    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent) {
       SetKitchenObjectParentServerRpc(kitchenObjectParent.GetNetworkObject());
    }

    public void DestroySelf() {
        Destroy(gameObject);
    }

    public void ClearKitchenObjectOnParent() {
        kitchenObjectParent.ClearKitchenObject();
    }

    public void ClearParent() {
        if(this.kitchenObjectParent != null) {
            this.kitchenObjectParent.ClearKitchenObject();
        }
    }

    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject) {
        if(this is PlateKitchenObject) {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }
        else {
            plateKitchenObject = null;
            return false;
        }
    }

    public static void SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent) {
        KitchenGameMultiplayer.Instance.SpawnKitchenObject(kitchenObjectSO,kitchenObjectParent);
    }

    public static void DestroyKitchenObject(KitchenObject kitchenObject) {
        KitchenGameMultiplayer.Instance.DestroyKitchenObject(kitchenObject);
    }
    
}
