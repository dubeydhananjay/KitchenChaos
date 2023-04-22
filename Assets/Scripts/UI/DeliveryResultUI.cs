using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryResultUI : MonoBehaviour {

    private const string RESULT_POPUP = "ResultPopup";
    private const string SUCCESS = "Delivery\nSuccess";
    private const string FAILURE = "Delivery\nFailed";

    [SerializeField] private Image bgImage;
    [SerializeField] private Image iconImage;
    [SerializeField] private Sprite successImage;
    [SerializeField] private Sprite failureImage;
    [SerializeField] private Color successColor;
    [SerializeField] private Color failureColor;
    [SerializeField] private TMPro.TextMeshProUGUI messageText;
    [SerializeField] private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Start() {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailure += DeliveryManager_OnRecipeFailure;
        gameObject.SetActive(false);
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e) {
        SetAnimation();
        messageText.text = SUCCESS;
        iconImage.sprite = successImage;
        bgImage.color = successColor;
    }

    private void DeliveryManager_OnRecipeFailure(object sender, System.EventArgs e) {
        SetAnimation();
        messageText.text = FAILURE;
        iconImage.sprite = failureImage;
        bgImage.color = failureColor;
    }

    private void SetAnimation() {
        gameObject.SetActive(true);
        //animator.SetTrigger(RESULT_POPUP);
    }
    
    
}
