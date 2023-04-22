using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TrashCounter : BaseCounter {

    public static event System.EventHandler OnObjectTrashed;

    new public static void ResetStaticData() {
        OnObjectTrashed = null;
    }
    public override void Interact(PlayerController playerController) {
        if (playerController.HasKitchenObject()) {
            KitchenObject.DestroyKitchenObject(playerController.GetKitchenObject());
            InteractLogicServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicServerRpc() {
        InteractLogicClientRpc();
    }
    [ClientRpc]
    private void InteractLogicClientRpc() {
        OnObjectTrashed?.Invoke(this,System.EventArgs.Empty);
    }
}
