using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class StoveCounter : BaseCounter,IHasProgress {
    
    public event EventHandler<IHasProgress.OnProgresChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs {
        public State state;
    }
    public enum State {
        IDLE,
        FRYING,
        FRIED,
        BURNED
    }

    [SerializeField] private FryingRecipeSO[] fryingRecipeSOs;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOs;
    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;
    private NetworkVariable<float> fryingTimer = new NetworkVariable<float>(0f);
    private NetworkVariable<float> burningTimer = new NetworkVariable<float>(0f);
    private NetworkVariable<State> state = new NetworkVariable<State>(State.IDLE);
    public bool IsFried => state.Value == State.FRIED;

    public override void OnNetworkSpawn() {
        fryingTimer.OnValueChanged += FryingTimer_OnValueChanged;
        burningTimer.OnValueChanged += BurningTimer_OnValueChanged;
        state.OnValueChanged += State_OnValueChanged;
    }

    private void FryingTimer_OnValueChanged(float previousVal, float newVal) {
        float fryingTimerMax = fryingRecipeSO != null ? fryingRecipeSO.fryingTimerMax : 1f;
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgresChangedEventArgs {
                    progressNormalized = fryingTimer.Value/fryingRecipeSO.fryingTimerMax
                });
    }

    private void BurningTimer_OnValueChanged(float previousVal, float newVal) {
        float burningTimerMax = burningRecipeSO != null ? burningRecipeSO.burningTimerMax : 1f;
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgresChangedEventArgs {
                    progressNormalized = burningTimer.Value/burningTimerMax
                });
    }

    private void State_OnValueChanged(State previousState, State newState) {
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
                state = state.Value
            });

        if(state.Value == State.IDLE || state.Value == State.BURNED) {
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgresChangedEventArgs {
                    progressNormalized = 0
                });
        }
    }

    private void Update() {
        if(!IsServer) return;

        switch(state.Value) {
            case State.IDLE:
                break;
            case State.FRYING:
                fryingTimer.Value += Time.deltaTime;
                
                if(fryingTimer.Value >= fryingRecipeSO.fryingTimerMax) {
                    KitchenGameMultiplayer.Instance.DestroyKitchenObject(GetKitchenObject());
                    KitchenObject.SpawnKitchenObject(fryingRecipeSO.outputKitchenObject,this);
                    burningTimer.Value = 0;
                    state.Value = State.FRIED;
                    SetBurningRecipeSOClientRpc(
                        KitchenGameMultiplayer.Instance.GetKitchenObjectSOIndex(GetKitchenObject().GetKitchenObjectSO));
                }
                break;
            case State.FRIED:
                burningTimer.Value += Time.deltaTime;
                if(burningTimer.Value >= burningRecipeSO.burningTimerMax) {
                    KitchenGameMultiplayer.Instance.DestroyKitchenObject(GetKitchenObject());
                    KitchenObject.SpawnKitchenObject(burningRecipeSO.outputKitchenObject,this);
                    state.Value = State.BURNED;
                }
                break;
            case State.BURNED:
                break;
        }
    }
     public override void Interact(PlayerController playerController) {
       if(!HasKitchenObject()) {
        if(playerController.HasKitchenObject() && HasRecipeWithInput(playerController.GetKitchenObject().GetKitchenObjectSO)) {
            KitchenObject kitchenObject = playerController.GetKitchenObject();
            kitchenObject.SetKitchenObjectParent(this);
            InteractLogicPlacedObjectOnCounterServerRpc(
            KitchenGameMultiplayer.Instance.GetKitchenObjectSOIndex(kitchenObject.GetKitchenObjectSO));
        }
       }
       else {
        if(!playerController.HasKitchenObject()) {
            GetKitchenObject().SetKitchenObjectParent(playerController);
            state.Value = State.IDLE;
        }
        else {
            if(playerController.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
                plateKitchenObject.AddIngredients(GetKitchenObject());
                SetStateIdleServerRpc();
            }
        }
       }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetStateIdleServerRpc() {
        state.Value = State.IDLE;
    }

    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicPlacedObjectOnCounterServerRpc(int kitchenObjectSOIndex) {
        fryingTimer.Value = 0;
        state.Value = State.FRYING;
        SetFryingRecipeSOClientRpc(kitchenObjectSOIndex);
    }

    [ClientRpc]
    private void SetFryingRecipeSOClientRpc(int kitchenObjectSOIndex) {
        KitchenObjectSO kitchenObjectSO = KitchenGameMultiplayer.Instance.GetKitchenObjectSOFromIndex(kitchenObjectSOIndex);
        fryingRecipeSO = GetFryingRecipeSOWithInput(kitchenObjectSO);
    }

    [ClientRpc]
    private void SetBurningRecipeSOClientRpc(int kitchenObjectSOIndex) {
        KitchenObjectSO kitchenObjectSO = KitchenGameMultiplayer.Instance.GetKitchenObjectSOFromIndex(kitchenObjectSOIndex);
        burningRecipeSO = GetBurningRecipeSOWithInput(kitchenObjectSO);
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO) {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        
        return fryingRecipeSO != null;
    }

    private KitchenObjectSO GetOutputKitchenObjectSO(KitchenObjectSO inputKitchenObjectSO) {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        
        return fryingRecipeSO != null ? fryingRecipeSO.outputKitchenObject : null;
    }

    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
         foreach(FryingRecipeSO fryingRecipeSO in fryingRecipeSOs) {
            if(fryingRecipeSO.inputKitchenObject == inputKitchenObjectSO) {
                return fryingRecipeSO;
            }
        }
        return null;
    }

    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
         foreach(BurningRecipeSO burningRecipeSO in burningRecipeSOs) {
            if(burningRecipeSO.inputKitchenObject == inputKitchenObjectSO) {
                return burningRecipeSO;
            }
        }
        return null;
    }
}
