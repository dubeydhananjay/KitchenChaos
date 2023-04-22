using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCountDownUI : MonoBehaviour {
    
    private const string NUMBER_POPUP = "NumberPopUp";
    [SerializeField] private TMPro.TextMeshProUGUI countDownText;
    [SerializeField] private Animator animator;
    private int previousCountDownNumber;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Start() {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        Hide();
    }
    
    private void Update() {
        int countDownNumber =  Mathf.CeilToInt(GameManager.Instance.CountDownTimer);
        countDownText.text = countDownNumber.ToString();
        if(previousCountDownNumber != countDownNumber) {
            previousCountDownNumber = countDownNumber;
            animator.SetTrigger(NUMBER_POPUP);
            SoundManager.Instance.PlayCountDownSound();
        }
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e) {
        if(GameManager.Instance.IsCountDownActive) {
            Show();
        }
        else {
            Hide();
        }
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
