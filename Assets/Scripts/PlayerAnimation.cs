using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerAnimation : NetworkBehaviour {
    
    private const string IS_WALKING = "IsWalking";
    private Animator animator;
    [SerializeField] private PlayerController playerController;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Update() {
        animator.SetBool(IS_WALKING,playerController.IsWalking);
    }
}
