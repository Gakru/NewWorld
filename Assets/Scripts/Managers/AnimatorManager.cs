using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KHC
{
    public class AnimatorManager : MonoBehaviour
    {
        // Animator ����
        public Animator animator;
        public bool canRotate; // ȸ�� ���� ����

        public void PlayAnimation(string targetAnimation, bool isInteracting)
        {
            animator.applyRootMotion = isInteracting; // ��Ʈ ��� ���� ���� ����
            animator.SetBool("canRotate", false);
            animator.SetBool("isInteracting", isInteracting); // ��ȣ �ۿ� ���� ����
            animator.CrossFade(targetAnimation, 0.2f); // Ÿ�� �ִϸ��̼� CrossFade ����
            //animator.Play(targetAnimation);
        }

        public virtual void TakeCriticalDamageAnimationEvent()
        {
            
        }
    }
}
