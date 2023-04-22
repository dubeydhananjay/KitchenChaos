using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CuttingCounter : BaseCounter, IHasProgress {
    
    public event EventHandler<IHasProgress.OnProgresChangedEventArgs> OnProgressChanged;
   
    public event EventHandler OnCut;
    public static event EventHandler OnAnyCut;
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOs;
    private int cuttingProgress;

    new public static void ResetStaticData() {
        OnAnyCut = null;
    }
    public override void Interact(PlayerController playerController) {
       if(!HasKitchenObject()) {
        if(playerController.HasKitchenObject() && HasRecipeWithInput(playerController.GetKitchenObject().GetKitchenObjectSO)) {
            KitchenObject kitchenObject = playerController.GetKitchenObject();
            kitchenObject.SetKitchenObjectParent(this);
            InteractLogicServerRpc();
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
        }
       }
    }

    public override void InteractAlternate(PlayerController playerController) {
        if(HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO)) {
           CutKitchenObjectServerRpc();
           CutProgressServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void CutKitchenObjectServerRpc() {
        CutKitchenObjectClientRpc();
    }
    [ClientRpc]
    private void CutKitchenObjectClientRpc() {
         cuttingProgress++;
            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO);
             OnProgressChanged?.Invoke(this, new IHasProgress.OnProgresChangedEventArgs {
                progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
            });
            OnCut?.Invoke(this,EventArgs.Empty);
            OnAnyCut?.Invoke(this,EventArgs.Empty);
           
    }

    [ServerRpc(RequireOwnership = false)]
    private void CutProgressServerRpc() {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO);
         if(cuttingProgress < cuttingRecipeSO.cuttingProgressMax) return;
            KitchenObject kitchenObject = GetKitchenObject();
            KitchenObjectSO outputKSO = GetOutputKitchenObjectSO(kitchenObject.GetKitchenObjectSO);
            kitchenObject.DestroySelf();
            KitchenObject.SpawnKitchenObject(outputKSO,this);
    }

    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicServerRpc() {
        InteractLogicClientRpc();
    }
    [ClientRpc]
    private void InteractLogicClientRpc() {
        cuttingProgress = 0;
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgresChangedEventArgs {
                progressNormalized = 0
        });
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO) {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        
        return cuttingRecipeSO != null;
    }

    private KitchenObjectSO GetOutputKitchenObjectSO(KitchenObjectSO inputKitchenObjectSO) {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        
        return cuttingRecipeSO != null ? cuttingRecipeSO.outputKitchenObject : null;
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
         foreach(CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOs) {
            if(cuttingRecipeSO.inputKitchenObject == inputKitchenObjectSO) {
                return cuttingRecipeSO;
            }
        }
        return null;
    }
}
