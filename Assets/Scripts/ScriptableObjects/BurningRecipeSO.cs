using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class BurningRecipeSO : ScriptableObject {
    public KitchenObjectSO inputKitchenObject;
    public KitchenObjectSO outputKitchenObject;
    public float burningTimerMax;
}
