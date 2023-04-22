using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class TestNetcodeUI : MonoBehaviour {

    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;

    private void Start() {
        hostButton.onClick.AddListener(() => {
            NetworkManager.Singleton.StartHost();
            Hide();
        });
        clientButton.onClick.AddListener(() => {
            NetworkManager.Singleton.StartClient();
            Hide();
        });
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
   
}
