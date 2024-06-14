using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace KHC
{
    public class InputHandler : MonoBehaviour
    {
        public float horizontal; // 수평 입력값
        public float vertical; // 수직 입력값
        public float moveAmount; // 이동거리
        public float mouseX; // 마우스 X 입력값
        public float mouseY; // 마우스 Y 입력값

        // [inputActions]
        public bool boolInput;
        public bool leftAttackInput;
        public bool rightAttackInput;
        public bool criticalAttackInput;
        public bool jumpInput;
        public bool lockOnInput;
        public bool inventoryInput;
        public bool lockOnLeftInput;
        public bool lockOnRightInput;
        public bool d_Pad_Up;
        public bool d_Pad_Down;
        public bool d_Pad_Left;
        public bool d_Pad_Right;

        // Player Animation 상태
        public bool sprintFlag;
        public bool rollFlag;
        public bool comboFlag;
        public bool lockOnFlag;
        public bool inventoryFlag;
        public float rollInputTimer;

        public Transform criticalAttackRayCastStartPoint;

        // PlayerControl 참조
        PlayerControl inputActions;
        // PlayerAttack 참조
        PlayerAttack playerAttack;
        // PlayerInventory 참조
        PlayerInventory playerInventory;
        // PlayerManager 참조
        PlayerManager playerManager;
        // PlayerStatas 참조
        PlayerStats playerStats;
        // UIManager 참조
        UIManager uiManager;
        // CameraHandler 참조
        CameraHandler cameraHandler;
        // AnimatorHandler 참조
        PlayerAnimatorManager animatorHandler;

        //CameraHandler cameraHandler -> PlayerManager.cs
        Vector2 movementInput; // 이동 입력
        Vector2 cameraInput; // 카메라 입력

        private void Awake()
        {
            // Player 액션 컴포넌트 초기화
            playerAttack = GetComponent<PlayerAttack>();
            playerInventory = GetComponent<PlayerInventory>();
            playerManager = GetComponent<PlayerManager>();
            playerStats = GetComponent<PlayerStats>();
            // UI 및 카메라 컴포넌트 참조
            uiManager = FindObjectOfType<UIManager>();
            cameraHandler = FindObjectOfType<CameraHandler>();
            animatorHandler = GetComponentInChildren<PlayerAnimatorManager>();
        }

        // 입력 액션 활성화
        public void OnEnable()
        {
            // PlayerControl 컴포넌트가 존재하지 않는다면
            if (inputActions == null)
            {
                // [★★MEMO★★]
                // started: On Key Down(누르는 순간)
                // performed: On Key(누르는 동안)
                // canceled: On Key Up(눌렀다 떼는 순간)

                // 새로운 inputAction 생성
                inputActions = new PlayerControl();
                // 이동 입력 이벤트 핸들러 추가
                inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
                // 카메라 입력 이벤트 핸들러 추가
                inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
                // 좌클릭 또는 E키 입력 시 rbInput을 true로 설정
                inputActions.PlayerActions.LeftAttack.performed += i => leftAttackInput = true;
                // 우클릭 또는 R키 입력 시 rtInput을 true로 설정
                inputActions.PlayerActions.RightAttack.performed += i => rightAttackInput = true;
                // LeftShift 키 입력 시 boolInput을 true로 설정
                inputActions.PlayerActions.Roll.performed += i => boolInput = true;
                inputActions.PlayerActions.Roll.canceled += i => boolInput = false;
                // ESC 키 입력 시 inventoryInput을 true로 설정
                inputActions.PlayerActions.Inventory.performed += i => inventoryInput = true;
                // T 키 입력 시 lockOnInput을 true로 설정
                inputActions.PlayerActions.LockOn.performed += i => lockOnInput = true;
                // 숫자 키(1) 입력 시 lockOnLeftInput을 true로 설정
                inputActions.PlayerMovement.LockOnLeftTarget.performed += i => lockOnLeftInput = true;
                // 숫자 키(2) 입력 시 lockOnRightInput을 true로 설정
                inputActions.PlayerMovement.LockOnRightTarget.performed += i => lockOnRightInput = true;
                // E 키 입력 시 criticalAttackInput을 true로 설정
                //inputActions.PlayerActions.CriticalAttack.performed += i => criticalAttackInput = true;
            }

            inputActions.Enable(); // inputAction 활성화
        }

        // 입력 액션 비활성화
        private void OnDisable()
        {
            inputActions.Disable();
        }

        // 입력 처리
        public void ScriptInput(float delta)
        {
            HandleMoveInput(delta);
            HandleRollInput(delta);
            HandleAttackInput(delta);
            //HandleQuickSlotsInput(); //EP.14
            HandleJumpInput();
            HandleInventoryInput();
            HandleLockOnInput();
            HandleCriticalAttackInput();
        }

        // 이동 처리
        private void HandleMoveInput(float delta)
        {
            horizontal = movementInput.x; // 수평 입력값 설정
            vertical = movementInput.y; // 수직 입력값 설정
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical)); // 이동 양 계산
            mouseX = cameraInput.x; // 마우스 X 입력값 설정
            mouseY = cameraInput.y; // 마우스 Y 입력값 설정
        }

        // Roll 처리
        private void HandleRollInput(float delta)
        {
            // Roll이 실행되었을 때, rollInputTimer에 현재 시간을 더하고, Sprint를 참으로 설정 
            if (boolInput)
            {
                rollInputTimer += delta;

                // 스태미나가 없을 경우
                if (playerStats.currentStamina <= 0)
                {
                    boolInput = false;
                    sprintFlag = false;
                }

                if (moveAmount > 0.5f && playerStats.currentStamina > 0)
                {
                    sprintFlag = true;
                }
            }
            else
            {
                sprintFlag = false;

                // Roll이 실행되지 않았을 때, rollInputTimer가 0보다 크고 0.5보다 작은 경우, 
                // Sprint를 거짓, Roll을 참으로 설정ㅛㅔ
                if (rollInputTimer > 0 && rollInputTimer < 0.5f)
                {
                    sprintFlag = false;
                    rollFlag = true;
                }

                rollInputTimer = 0; // 초기화
            }
        }

        // Attack 처리
        private void HandleAttackInput(float delta)
        {
            if (leftAttackInput)
            {
                // 콤보가 가능할 경우
                if (playerManager.canDoCombo)
                {
                    comboFlag = true;
                    // 오른쪽 무기로 콤보 처리
                    playerAttack.HandleWeaponCombo(playerInventory.rightWeapon);
                    comboFlag = false;
                }
                // 상호작용 중이거나 콤보가 가능한 상태가 아닐 경우
                else
                {
                    if (playerManager.isInteracting) return;
                    if (playerManager.canDoCombo) return;

                    // 오른쪽 무기로 약공격 처리
                    playerAttack.HandleLightAttack(playerInventory.rightWeapon);
                }
            }

            if (rightAttackInput)
            {
                // 오른쪽 무기로 강공격 처리
                playerAttack.HandleHeavyAttack(playerInventory.rightWeapon);
            }
        }

        // Jump 처리
        private void HandleJumpInput()
        {
            // 스페이스바 키 입력 시 jumpInput을 true로 설정
            inputActions.PlayerActions.Jump.performed += i => jumpInput = true;
        }

        // Inventory 처리
        private void HandleInventoryInput()
        {
            if (inventoryInput)
            {
                inventoryFlag = !inventoryFlag; // 인벤토리 이벤트(anim) 활성화
                
                if (inventoryFlag)
                {
                    // UI 열기
                    uiManager.OpenSelectWindow();
                    uiManager.UpdateUI();
                    //uiManager.hudUI.SetActive(false);
                }
                else
                {
                    // UI 닫기
                    uiManager.CloseSelectWindow();
                    uiManager.CloseAllInventoryUI();
                    //uiManager.hudUI.SetActive(true);
                }
            }
        }

        // 시점 고정 처리
        private void HandleLockOnInput()
        {
            // 시점 고정 입력(lockOnInput)이 활성화 됬고, 현재 시점이 고정(lockOnFlag)상태가 아닐 때
            if (lockOnInput && lockOnFlag == false)
            {
                lockOnInput = false;
                //lockOnFlag = true;
                cameraHandler.HandleLockOn(); // 시점 고정 처리 활성화
                
                // 가장 가까운 타겟이 없을 경우
                if (cameraHandler.nearestLockOnTarget != null)
                {
                    // 현재 타겟을 가장 가까운 타겟으로 설정
                    cameraHandler.currentLockOnTarget = cameraHandler.nearestLockOnTarget;
                    lockOnFlag = true;
                }
            }
            // 시점 고정 입력(lockOnInput)이 활성화 됬고, 현재 시점이 고정(lockOnFlag)상태일 때
            else if (lockOnInput && lockOnFlag)
            {
                lockOnInput = false;
                lockOnFlag = false;
                cameraHandler.ClearLockOnTarget(); // 시점 고정 처리 비활성화
            }

            // 시점 고정 상태(lockOnFlag)에서 왼쪽 시점 고정 입력(lockOnLeftInput)이 활성화되었을 때
            if (lockOnFlag && lockOnLeftInput)
            {
                lockOnLeftInput = false;
                cameraHandler.HandleLockOn(); // 시점 고정 처리 활성화
                if (cameraHandler.leftLockTarget != null)
                {
                    // 현재 타겟을 왼쪽 시점 고정 타겟으로 설정
                    cameraHandler.currentLockOnTarget = cameraHandler.leftLockTarget;
                }
            }

            // 시점 고정 상태(lockOnFlag)에서 오른쪽 시점 고정 입력(lockOnRightInput)이 활성화되었을 때
            if (lockOnFlag && lockOnRightInput)
            {
                lockOnRightInput = false;
                cameraHandler.HandleLockOn(); // 시점 고정 처리 활성화
                if(cameraHandler.rightLockTarget != null)
                {
                    // 현재 타겟을 오른쪽 시점 고정 타겟으로 설정
                    cameraHandler.currentLockOnTarget = cameraHandler.rightLockTarget;
                }
            }
            
            // 카메라 높이 설정
            cameraHandler.SetCameraHeight();
        }

        // 치명타 공격 처리
        private void HandleCriticalAttackInput()
        {
            if (criticalAttackInput)
            {
                criticalAttackInput = false;
                //playerAttack.AttackBackStabOrCounterAttack();
                //playerAttack.AttemptBackStabOrRiposte();
            }
        }
    }
}