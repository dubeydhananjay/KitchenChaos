using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour {
    
    private enum Mode {
        LOOKAT,
        LOOKATINVERTED,
        CAMERAFORWARD,
        CAMERAFORWARDINVERTED
    }

    [SerializeField] private Mode mode;

    private void LateUpdate() {
        switch(mode) {
            case Mode.LOOKAT:
                transform.LookAt(Camera.main.transform);
                break;
            case Mode.LOOKATINVERTED:
                Vector3 dirFromCamera = transform.position - Camera.main.transform.position;
                transform.LookAt(transform.position + dirFromCamera);
                break;
            case Mode.CAMERAFORWARD:
                transform.forward = Camera.main.transform.forward;
                break;
            case Mode.CAMERAFORWARDINVERTED:
                transform.forward = -Camera.main.transform.forward;
                break;
        }
    }
}
