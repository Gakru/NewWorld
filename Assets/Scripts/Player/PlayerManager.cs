using KHC;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace KHC
{
    public class PlayerManager : CharacterManager
    {
        // InputHandler 참조
        InputHandler inputHandler;
        // Animator 참조
        Animator animator;
        // CameraHandler 참조
        CameraHandler cameraHandler;
        // PlayerStats 참조
        PlayerStats playerStats;
        // PlayerAnimatorManager 참조
        PlayerAnimatorManager playerAnimatorManager;
        // PlayerLocalMotions 참조
        PlayerLocomotions playerLocalMotions;

        public bool isInteracting; // 상호작용 여부

        [Header("Player Flags")]
        public bool isSprinting; // Sprint 상태 여부
        public bool isInAir; // 공중 상태 여부
        public bool isGrounded; // 지상 밀착 여부
        public bool canDoCombo; // 콤보 가능 여부
        public bool isInvulnerable; // 무적 가능 여부

        private void Awake()
        {
            cameraHandler = FindObjectOfType<CameraHandler>(); // CameraHandler 오브젝트 찾기
            cameraHandler = CameraHandler.singleton; // cameraHandler 초기화
            inputHandler = GetComponent<InputHandler>(); // inputHandler 컴포넌트 초기화
            animator = GetComponentInChildren<Animator>(); // animator 컴포넌트 초기화
            playerStats = GetComponent<PlayerStats>(); // PlayerStats 컴포넌트 초기화
            playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>(); // PlayerAnimatorManager 컴포넌트 초기화
            playerLocalMotions = GetComponent<PlayerLocomotions>(); // playerLocalMotions 컴포넌트 초기화
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            float delta = Time.deltaTime;
            isInteracting = animator.GetBool("isInteracting"); // 상호작용 확인
            canDoCombo = animator.GetBool("canDoCombo"); // 콤보작용 확인
            isInvulnerable = animator.GetBool("isInvulnerable"); // 무적시간 확인
            animator.SetBool("isInAir", isInAir); // 공중 여부
            animator.SetBool("isDead", playerStats.isDead);

            inputHandler.ScriptInput(delta); // 입력 처리
            playerLocalMotions.HandleRollingAndSprinting(delta); // Roll 및 Sprint 처리
            playerLocalMotions.HandleJumping(); // 점프 처리
            playerStats.RegenrateShield(); // 실드 재생 처리
            playerStats.RegenerateStamina(); // 스태미나 재생 처리
            playerAnimatorManager.canRotate = animator.GetBool("canRotate");
        }

        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;

            playerLocalMotions.HandleMovement(delta); // 이동 처리
            playerLocalMotions.HandleFalling(delta, playerLocalMotions.moveDirection); // 추락 처리
            playerLocalMotions.HandleRotation(delta); // 회전 처리
        }

        private void LateUpdate()
        {
            inputHandler.rollFlag = false; // Roll 초기화
            inputHandler.leftAttackInput = false;
            inputHandler.rightAttackInput = false;
            inputHandler.d_Pad_Up = false;
            inputHandler.d_Pad_Down = false;
            inputHandler.d_Pad_Left = false;
            inputHandler.d_Pad_Right = false;
            inputHandler.jumpInput = false;
            inputHandler.inventoryInput = false;

            float delta = Time.deltaTime;

            if (cameraHandler != null)
            {
                // 타겟 추격
                cameraHandler.FollowTarget(delta);
                // 카메라 회전 처리
                cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
            }

            if (isInAir)
            {
                // 공중에 떠 있는 시간 업데이트
                playerLocalMotions.inAirTimer = playerLocalMotions.inAirTimer + Time.deltaTime;
            }
        }
    }
}