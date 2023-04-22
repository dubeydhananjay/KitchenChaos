using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class DeliveryManager : NetworkBehaviour {
    
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailure;
    public static DeliveryManager Instance { get; private set; }
    [SerializeField] private RecipeListSO recipeListSO;
    [SerializeField] private List<RecipeSO> waitingRecipeListSO;
    private float spawnTimer;
    [SerializeField] private float spawnTimerMax = 4f;
    [SerializeField] private int maxRecipeListAmount = 4;
    private int recipeListAmount;
    private int recipeDeliveredSuccessAmount;
    public int RecipeDeliveredSuccessAmount => recipeDeliveredSuccessAmount;
    public List<RecipeSO> WaitingRecipeListSO => waitingRecipeListSO; 

    private void Awake() {
        Instance = this;
        waitingRecipeListSO = new List<RecipeSO>();
    }

    private void Update() {
        spawnTimer += Time.deltaTime;
        if(spawnTimer >= spawnTimerMax) {
            spawnTimer = 0;
            if(recipeListAmount >= maxRecipeListAmount || !GameManager.Instance.IsGamePlaying) return;
            int waitingRecipeIndex = UnityEngine.Random.Range( 0,recipeListSO.recipeList.Count );
            SpawnNewWaitingRecipeClientRpc(waitingRecipeIndex);

        }
    }

    [ClientRpc]
    private void SpawnNewWaitingRecipeClientRpc(int waitingRecipeIndex) {
        RecipeSO waitingRecipe = recipeListSO.recipeList[waitingRecipeIndex];
        waitingRecipeListSO.Add(waitingRecipe);
        OnRecipeSpawned?.Invoke(this,EventArgs.Empty);
        recipeListAmount++;
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject) {
        for(int i = 0; i < waitingRecipeListSO.Count; i++) {
            RecipeSO waitingRecipeSO = waitingRecipeListSO[i];
            if(waitingRecipeSO.ingredients.Count != plateKitchenObject.KitchenObjects.Count) continue;
            bool plateContentMatchesRecipe = true;
            foreach(KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.ingredients) {
                bool ingredientFound = false;
                foreach(KitchenObject plateKO in plateKitchenObject.KitchenObjects) {
                    if(plateKO.GetKitchenObjectSO == recipeKitchenObjectSO) {
                        ingredientFound = true;
                        break;
                    }
                }
                plateContentMatchesRecipe = ingredientFound;
            }
            if(plateContentMatchesRecipe) {
               DeliverCorrectRecipeServerRpc(i);
               return;
            }
        }
        DeliverInCorrectRecipeServerRpc();
    }
    [ServerRpc(RequireOwnership = false)]
    private void DeliverCorrectRecipeServerRpc(int waitingRecipeListSOIndex) {
        DeliverCorrectRecipeClientRpc(waitingRecipeListSOIndex);
    }
     
    [ClientRpc]
    private void DeliverCorrectRecipeClientRpc(int waitingRecipeListSOIndex) {
         waitingRecipeListSO.RemoveAt(waitingRecipeListSOIndex);
        recipeListAmount--;
        recipeDeliveredSuccessAmount++;
        OnRecipeSpawned?.Invoke(this,EventArgs.Empty);
        OnRecipeSuccess?.Invoke(this,EventArgs.Empty);
    }
    [ServerRpc(RequireOwnership = false)]
    private void DeliverInCorrectRecipeServerRpc() {
        DeliverInCorrectRecipeClientRpc();
    }
    [ClientRpc]
     private void DeliverInCorrectRecipeClientRpc() {
        OnRecipeFailure?.Invoke(this,EventArgs.Empty);
    }
}
