using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class FryingRecipeSO : ScriptableObject {
    public KitchenObjectSO inputKitchenObject;
    public KitchenObjectSO outputKitchenObject;
    public float fryingTimerMax;
}
