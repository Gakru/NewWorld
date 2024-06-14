using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KHC
{
    public class PlayerAnimatorManager : AnimatorManager
    {
        // PlayerManager 참조
        PlayerManager playerManager;
        // PlayerStats 참조
        PlayerStats playerStats;
        // InputHandler 참조
        InputHandler inputHandler;
        // PlayerLocalMotions 참조
        PlayerLocomotions playerLocalMotions;
        int vertical; // 수직 방향 파라미터 해시
        int horizontal; // 수평 방향 파라미터 해시
        //public bool canRotate; // 회전 가능 여부

        // 초기화
        public void Initialized()
        {
            playerManager = GetComponentInParent<PlayerManager>(); // PlayerManager 컴포넌트 초기화
            playerStats = GetComponentInParent<PlayerStats>(); // PlayerStats 컴포넌츠 초기화
            animator = GetComponent<Animator>(); // Animator 컴포넌트 초기화
            inputHandler = GetComponentInParent<InputHandler>(); // InputHanler 컴포넌트 초기화
            playerLocalMotions = GetComponentInParent<PlayerLocomotions>(); // PlayerLocalMotions 컴포넌트 초기화
            vertical = Animator.StringToHash("Vertical"); // 수직 방향 파라미터 해시 값 초기화
            horizontal = Animator.StringToHash("Horizontal"); // 수평 방향 파라미터 해시 값 초기화
        }

        public void UpdateAnimatorValue(float verticalMovemnet, float horizontalMovemnet, bool isSprinting)
        {
            #region Vertical
            float v = 0f;

            // 수직 방향 움직임에 따른 파라미터 값 설정
            if (verticalMovemnet > 0f && verticalMovemnet < 0.55f) v = 0.5f;
            else if (verticalMovemnet > 0.55f) v = 1f;
            else if (verticalMovemnet < 0f && verticalMovemnet > -0.55f) v = -0.5f;
            else if (verticalMovemnet < -0.55f) v = -1f;
            else v = 0f;
            #endregion

            #region Horizontal;
            float h = 0f;

            // 수평 방향 움직임에 따른 파라미터 값 설정
            if (horizontalMovemnet > 0f && horizontalMovemnet < 0.55f) h = 0.5f;
            else if (horizontalMovemnet > 0.55f) h = 1f;
            else if (horizontalMovemnet < 0f && horizontalMovemnet > -0.55f) h = -0.5f;
            else if (horizontalMovemnet < -0.55f) h = -1f;
            else h = 0f;
            #endregion

            // Sprint 상태 시 파라미터 값 조정
            if (isSprinting)
            {
                v = 2f;
                h = horizontalMovemnet;
            }

            animator.SetFloat(vertical, v, 0.1f, Time.deltaTime); // 수직 방향 파라미터 설정
            animator.SetFloat(horizontal, h, 0.1f, Time.deltaTime); // 수평 바향 파라미터 설정
        }

        // 회전 처리(가능)
        public void CanRotate()
        {
            animator.SetBool("canRotate", true);
        }
        // 회전 처리(불가능)
        public void StopRotation()
        {
            animator.SetBool("canRotate", false);
        }

        // 콤보 처리(가능)
        public void EnableCombo()
        {
            animator.SetBool("canDoCombo", true);
        }
        // 콤보 처리(불가능)
        public void DisableCombo()
        {
            animator.SetBool("canDoCombo", false);
        }

        // 무적 처리(가능)
        public void EnableIsInvulnerable()
        {
            animator.SetBool("isInvulnerable", true);
        }

        // 무적 처리(불가능)
        public void DisableIsInvulnerable()
        {
            animator.SetBool("isInvulnerable", false);
        }

        private void OnAnimatorMove()
        {
            if (playerManager.isInteracting == false) return;

            float delta = Time.deltaTime;
            playerLocalMotions.rigidbody.drag = 0; // 드래그 값 초기화
            Vector3 deltaPosition = animator.deltaPosition; // animator의 deltaPosition 가져오기
            deltaPosition.y = 0; // 수직 방향 제거(수직 이동X)
            Vector3 velocity = deltaPosition / delta; // deltaPosition을 시간에 따른 속도로 변환
            playerLocalMotions.rigidbody.velocity = velocity; // rigidbody 속도를 velocity로 설정
        }
    }
}