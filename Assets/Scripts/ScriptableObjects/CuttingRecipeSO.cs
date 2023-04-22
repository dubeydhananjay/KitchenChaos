using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class CuttingRecipeSO : ScriptableObject {
    
    public KitchenObjectSO inputKitchenObject;
    public KitchenObjectSO outputKitchenObject;
    public int cuttingProgressMax;
}
