using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    protected float volume;
    public float Volume => volume;
    public virtual void ChangeVolume(float volume) {
        this.volume = volume;
    }
}
