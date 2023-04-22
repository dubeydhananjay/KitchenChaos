using System;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour {

    [SerializeField] private Transform container;
    [SerializeField] private DeliveryManagerSingleUI recipeTemplate;

    private void Awake() {
        recipeTemplate.gameObject.SetActive(false);
    }

    private void Start() {
        DeliveryManager.Instance.OnRecipeSpawned += DeliveryManager_OnRecipeSpawned;
        UpdateVisual();
    }

    private void DeliveryManager_OnRecipeSpawned(object sender, EventArgs e) {
        UpdateVisual();
    }

    private void UpdateVisual() {
        for(int i = 1; i < container.childCount; i++) {
            Destroy(container.GetChild(i).gameObject);
        }

        foreach(RecipeSO recipeSO in DeliveryManager.Instance.WaitingRecipeListSO) {
            DeliveryManagerSingleUI recipeTransform = Instantiate(recipeTemplate,container);
            recipeTransform.gameObject.SetActive(true);
            recipeTransform.SetRecipeSO(recipeSO);
        }
    }
    
}
