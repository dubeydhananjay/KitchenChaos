using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyBindingUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI keyButtonText;
    [SerializeField] private Button keyButton;
    [SerializeField] private GameInput.Binding binding;

    private void Start() {
        GetKeyBindingText();
        keyButton.onClick.AddListener(() => RebindBinding(binding));
    }

    private void GetKeyBindingText() {
        keyButtonText.text = GameInput.Instance.GetBindingText(binding);
    }

    private void RebindBinding(GameInput.Binding binding) {
        OptionsUI.Instance.ShowPressToKeyBindTransform();
        GameInput.Instance.RebindBinding(binding, () => {
            OptionsUI.Instance.HidePressToKeyBindTransform();
            GetKeyBindingText();
        });
    }
}
