using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerController : NetworkBehaviour, IKitchenObjectParent {

    public static event EventHandler OnPlayerSpawned;
    public static event EventHandler OnPlayerPickedObject;
    public static void ResetStaticData() {
        OnPlayerSpawned = null;
        OnPlayerPickedObject = null;
    }
    public static PlayerController LocalInstance { get; private set; }
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs {
        public BaseCounter selectedCounter;
    }
    public event EventHandler OnObjectPickup;
    [SerializeField] private float moveSpeed = 7;
    [SerializeField] private float rotateSpeed = 10;
    [SerializeField] private LayerMask counterLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;
    private KitchenObject kitchenObject;
    private Vector3 lastInteractDir;
    private BaseCounter selectedCounter;
    private bool isWalking;
    public bool IsWalking => isWalking;

    public override void OnNetworkSpawn() {
        if(IsOwner) {
            LocalInstance = this;
        }
        OnPlayerSpawned?.Invoke(this,EventArgs.Empty);
    }
    private void Start() {
        GameInput.Instance.OnInteract += GameInput_Interaction;
        GameInput.Instance.OnInteractAlternate += GameInput_InteractionAlternate;
    }
    private void Update() {
        if(!IsOwner) return;
        HandleMovement();
        HandleInteractions();
    }

    private void GameInput_Interaction(object sender, System.EventArgs e) {
        if (!GameManager.Instance.IsGamePlaying) return;
        if (selectedCounter) {
            selectedCounter.Interact(this);
        }
    }

    private void GameInput_InteractionAlternate(object sender, System.EventArgs e) {
        if (!GameManager.Instance.IsGamePlaying) return;
        if (selectedCounter) {
            selectedCounter.InteractAlternate(this);
        }
    }

    private bool CanMove(Vector3 moveDir, float playerHeight, float playerRadius, float moveDistance) {
        return Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius,
                        moveDir, moveDistance);
    }

    private void HandleMovement() {
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = 0.7f;
        float playerHeight = 2f;
        bool canMove = !CanMove(moveDir, playerHeight, playerRadius, moveDistance);
        if (!canMove) {
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = (moveDir.x < -0.5f || moveDir.x > 0.5f) && !CanMove(moveDirX, playerHeight, playerRadius, moveDistance);
            if (canMove) {
                moveDir = moveDirX;
            }
            else {
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = (moveDir.z < -0.5f || moveDir.z > 0.5f) && !CanMove(moveDirZ, playerHeight, playerRadius, moveDistance);
                if (canMove) {
                    moveDir = moveDirZ;
                }
            }
        }
        if (canMove) {
            transform.position += moveDir * moveDistance;
        }
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
        isWalking = moveDir != Vector3.zero;
    }

    private void HandleInteractions() {
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
        float interactDistance = 2f;
        if (moveDir != Vector3.zero) {
            lastInteractDir = moveDir;
        }
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, counterLayerMask)) {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter)) {
                if (baseCounter != selectedCounter) {
                    SetSelectedCounter(baseCounter);
                }
            }
            else {
                SetSelectedCounter(null);
            }
        }
        else {
            SetSelectedCounter(null);
        }
    }

    private void SetSelectedCounter(BaseCounter selectedCounter) {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs {
            selectedCounter = this.selectedCounter
        });
    }

    public Transform GetKitchenObjectFollowTransform() {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject) {
        this.kitchenObject = kitchenObject;
        if (kitchenObject != null) {
            OnObjectPickup?.Invoke(this, EventArgs.Empty);
            OnPlayerPickedObject?.Invoke(this,EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject() {
        return kitchenObject;
    }

    public void ClearKitchenObject() {
        kitchenObject = null;
    }

    public bool HasKitchenObject() {
        return kitchenObject != null;
    }

    public NetworkObject GetNetworkObject() {
        return NetworkObject;
    }

}
