using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject {

    public event EventHandler<OnIngredientsAddedEventArgs> OnIngredientsAdded;
    public class OnIngredientsAddedEventArgs : EventArgs {
        public KitchenObject kitchenObject;
    }
    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSOs;
    private List<KitchenObject> kitchenObjects;
    public List<KitchenObject> KitchenObjects => kitchenObjects;
    protected override void Awake() {
        base.Awake();
        kitchenObjects = new List<KitchenObject>();
    }

    public void AddIngredients(KitchenObject kitchenObject) {
        if(!validKitchenObjectSOs.Contains(kitchenObject.GetKitchenObjectSO)) return;

        kitchenObjects.Add(kitchenObject);
        OnIngredientsAdded?.Invoke(this, new OnIngredientsAddedEventArgs {
            kitchenObject = kitchenObject
        });
    }
}
