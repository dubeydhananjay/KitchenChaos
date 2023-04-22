using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlatesCounter : BaseCounter {
    
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;
    [SerializeField] private float spawnTimerMax = 4;
    [SerializeField] private int plateSpawnAmountMax = 4;
    [SerializeField] private KitchenObjectSO plateObjectSO;
    private float spawnTimer;
    private int plateSpawnAmount;

    private void Update() {
        if(!IsServer) return;

        spawnTimer += Time.deltaTime;
        if(spawnTimer >= spawnTimerMax) {
            spawnTimer = 0;
            if(GameManager.Instance.IsGamePlaying && plateSpawnAmount < plateSpawnAmountMax) {
                SpawnPlateServerRpc();
            }
            
        }
    }
    [ServerRpc]
    private void SpawnPlateServerRpc() {
        SpawnPlateClientRpc();
    }
    [ClientRpc]
    private void SpawnPlateClientRpc() {
        plateSpawnAmount++;
        OnPlateSpawned?.Invoke(this,EventArgs.Empty);
    }

    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicServerRpc() {
        InteractLogicClientRpc();
    }
    [ClientRpc]
    private void InteractLogicClientRpc() {
        plateSpawnAmount--;
        OnPlateRemoved?.Invoke(this,EventArgs.Empty);
    }

    public override void Interact(PlayerController playerController) {
        if(playerController.HasKitchenObject()) return;
        if(plateSpawnAmount > 0) {
            InteractLogicServerRpc();
            KitchenObject.SpawnKitchenObject(plateObjectSO,playerController);
        }
    }
}
