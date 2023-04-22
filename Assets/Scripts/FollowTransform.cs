using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour {
   
   private Transform targetTransform;

   private void LateUpdate() {
      if(!targetTransform) return;
      transform.position = targetTransform.position;
      transform.rotation = targetTransform.rotation;
   }

   public void SetFollowTargetTransform(Transform targetTransform) {
      this.targetTransform = targetTransform;
      Debug.Log(transform.name + " -- " + targetTransform.name);
   }
}
