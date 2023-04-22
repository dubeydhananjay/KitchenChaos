using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AudioClipsSO : ScriptableObject {
    
    public AudioClip stoveSizzle;
    public AudioClip[] chops;
    public AudioClip[] footSteps;
    public AudioClip[] objectDrops;
    public AudioClip[] deliveryFailures;
    public AudioClip[] deliverySuccesses;
    public AudioClip[] trashes;
    public AudioClip[] pickups;
    public AudioClip[] warnings;
}
