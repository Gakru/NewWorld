using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KHC
{
    public class AnimatorManager : MonoBehaviour
    {
        // Animator 참조
        public Animator animator;
        public bool canRotate; // 회전 가능 여부

        public void PlayAnimation(string targetAnimation, bool isInteracting)
        {
            animator.applyRootMotion = isInteracting; // 루트 모션 적용 여부 설정
            animator.SetBool("canRotate", false);
            animator.SetBool("isInteracting", isInteracting); // 상호 작용 여부 설정
            animator.CrossFade(targetAnimation, 0.2f); // 타겟 애니메이션 CrossFade 실행
            //animator.Play(targetAnimation);
        }

        public virtual void TakeCriticalDamageAnimationEvent()
        {
            
        }
    }
}
