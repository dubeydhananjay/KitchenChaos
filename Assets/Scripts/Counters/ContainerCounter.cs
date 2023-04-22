using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ContainerCounter : BaseCounter, IKitchenObjectParent {
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public event EventHandler OnPlayerGrabbedObject;
    public override void Interact(PlayerController playerController) {
        if(playerController.HasKitchenObject()) return;
        KitchenObject.SpawnKitchenObject(kitchenObjectSO,playerController);
        InteractLogicServerRpc();

    }
    [ServerRpc]
    private void InteractLogicServerRpc() {
        InteractLogicClientRpc();
    }
    [ClientRpc]
    private void InteractLogicClientRpc() {
        OnPlayerGrabbedObject?.Invoke(this,EventArgs.Empty);
    }
}
