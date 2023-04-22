using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryManagerSingleUI : MonoBehaviour {

    [SerializeField] private Transform iconContainer;
    [SerializeField] private TMPro.TextMeshProUGUI recipeName;
    [SerializeField] private Image iconTemplate;

    private void Awake() {
        iconTemplate.gameObject.SetActive(false);
    }
    public void SetRecipeSO(RecipeSO recipeSO) {
        for(int i = 1; i < iconContainer.childCount; i++) {
            Destroy(iconContainer.GetChild(i).gameObject);
        }
        recipeName.text = recipeSO.recipeName;
        foreach(KitchenObjectSO kitchenObjectSO in recipeSO.ingredients) {
            Image icon = Instantiate(iconTemplate,iconContainer);
            icon.gameObject.SetActive(true);
            icon.sprite = kitchenObjectSO.sprite;
        }
    }


    
}
