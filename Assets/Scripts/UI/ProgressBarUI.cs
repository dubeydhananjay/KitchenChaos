using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour {

    [SerializeField] private GameObject hasProgressGO;
    [SerializeField] private Image barImage;
    private IHasProgress hasProgress;

    private void Start() {
        hasProgress = hasProgressGO.GetComponent<IHasProgress>();
        if(hasProgress == null) return;
        hasProgress.OnProgressChanged += IHasProgress_OnProgressChanged;
        barImage.fillAmount = 0;
        Hide();
    }

    private void IHasProgress_OnProgressChanged(object sender, IHasProgress.OnProgresChangedEventArgs e) {
        barImage.fillAmount = e.progressNormalized;
        Debug.Log("barImage.fillAmount: " + barImage.fillAmount);
        if(e.progressNormalized == 0 || e.progressNormalized == 1f) {
            Hide();
        }
        else {
            Show();
        }
    }

    private void Show() {
        gameObject.SetActive(true);
    }

     private void Hide() {
        gameObject.SetActive(false);
    }
  
}
