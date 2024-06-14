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
        public float horizontal; // ���� �Է°�
        public float vertical; // ���� �Է°�
        public float moveAmount; // �̵��Ÿ�
        public float mouseX; // ���콺 X �Է°�
        public float mouseY; // ���콺 Y �Է°�

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

        // Player Animation ����
        public bool sprintFlag;
        public bool rollFlag;
        public bool comboFlag;
        public bool lockOnFlag;
        public bool inventoryFlag;
        public float rollInputTimer;

        public Transform criticalAttackRayCastStartPoint;

        // PlayerControl ����
        PlayerControl inputActions;
        // PlayerAttack ����
        PlayerAttack playerAttack;
        // PlayerInventory ����
        PlayerInventory playerInventory;
        // PlayerManager ����
        PlayerManager playerManager;
        // PlayerStatas ����
        PlayerStats playerStats;
        // UIManager ����
        UIManager uiManager;
        // CameraHandler ����
        CameraHandler cameraHandler;
        // AnimatorHandler ����
        PlayerAnimatorManager animatorHandler;

        //CameraHandler cameraHandler -> PlayerManager.cs
        Vector2 movementInput; // �̵� �Է�
        Vector2 cameraInput; // ī�޶� �Է�

        private void Awake()
        {
            // Player �׼� ������Ʈ �ʱ�ȭ
            playerAttack = GetComponent<PlayerAttack>();
            playerInventory = GetComponent<PlayerInventory>();
            playerManager = GetComponent<PlayerManager>();
            playerStats = GetComponent<PlayerStats>();
            // UI �� ī�޶� ������Ʈ ����
            uiManager = FindObjectOfType<UIManager>();
            cameraHandler = FindObjectOfType<CameraHandler>();
            animatorHandler = GetComponentInChildren<PlayerAnimatorManager>();
        }

        // �Է� �׼� Ȱ��ȭ
        public void OnEnable()
        {
            // PlayerControl ������Ʈ�� �������� �ʴ´ٸ�
            if (inputActions == null)
            {
                // [�ڡ�MEMO�ڡ�]
                // started: On Key Down(������ ����)
                // performed: On Key(������ ����)
                // canceled: On Key Up(������ ���� ����)

                // ���ο� inputAction ����
                inputActions = new PlayerControl();
                // �̵� �Է� �̺�Ʈ �ڵ鷯 �߰�
                inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
                // ī�޶� �Է� �̺�Ʈ �ڵ鷯 �߰�
                inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
                // ��Ŭ�� �Ǵ� EŰ �Է� �� rbInput�� true�� ����
                inputActions.PlayerActions.LeftAttack.performed += i => leftAttackInput = true;
                // ��Ŭ�� �Ǵ� RŰ �Է� �� rtInput�� true�� ����
                inputActions.PlayerActions.RightAttack.performed += i => rightAttackInput = true;
                // LeftShift Ű �Է� �� boolInput�� true�� ����
                inputActions.PlayerActions.Roll.performed += i => boolInput = true;
                inputActions.PlayerActions.Roll.canceled += i => boolInput = false;
                // ESC Ű �Է� �� inventoryInput�� true�� ����
                inputActions.PlayerActions.Inventory.performed += i => inventoryInput = true;
                // T Ű �Է� �� lockOnInput�� true�� ����
                inputActions.PlayerActions.LockOn.performed += i => lockOnInput = true;
                // ���� Ű(1) �Է� �� lockOnLeftInput�� true�� ����
                inputActions.PlayerMovement.LockOnLeftTarget.performed += i => lockOnLeftInput = true;
                // ���� Ű(2) �Է� �� lockOnRightInput�� true�� ����
                inputActions.PlayerMovement.LockOnRightTarget.performed += i => lockOnRightInput = true;
                // E Ű �Է� �� criticalAttackInput�� true�� ����
                //inputActions.PlayerActions.CriticalAttack.performed += i => criticalAttackInput = true;
            }

            inputActions.Enable(); // inputAction Ȱ��ȭ
        }

        // �Է� �׼� ��Ȱ��ȭ
        private void OnDisable()
        {
            inputActions.Disable();
        }

        // �Է� ó��
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

        // �̵� ó��
        private void HandleMoveInput(float delta)
        {
            horizontal = movementInput.x; // ���� �Է°� ����
            vertical = movementInput.y; // ���� �Է°� ����
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical)); // �̵� �� ���
            mouseX = cameraInput.x; // ���콺 X �Է°� ����
            mouseY = cameraInput.y; // ���콺 Y �Է°� ����
        }

        // Roll ó��
        private void HandleRollInput(float delta)
        {
            // Roll�� ����Ǿ��� ��, rollInputTimer�� ���� �ð��� ���ϰ�, Sprint�� ������ ���� 
            if (boolInput)
            {
                rollInputTimer += delta;

                // ���¹̳��� ���� ���
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

                // Roll�� ������� �ʾ��� ��, rollInputTimer�� 0���� ũ�� 0.5���� ���� ���, 
                // Sprint�� ����, Roll�� ������ �����ˤ�
                if (rollInputTimer > 0 && rollInputTimer < 0.5f)
                {
                    sprintFlag = false;
                    rollFlag = true;
                }

                rollInputTimer = 0; // �ʱ�ȭ
            }
        }

        // Attack ó��
        private void HandleAttackInput(float delta)
        {
            if (leftAttackInput)
            {
                // �޺��� ������ ���
                if (playerManager.canDoCombo)
                {
                    comboFlag = true;
                    // ������ ����� �޺� ó��
                    playerAttack.HandleWeaponCombo(playerInventory.rightWeapon);
                    comboFlag = false;
                }
                // ��ȣ�ۿ� ���̰ų� �޺��� ������ ���°� �ƴ� ���
                else
                {
                    if (playerManager.isInteracting) return;
                    if (playerManager.canDoCombo) return;

                    // ������ ����� ����� ó��
                    playerAttack.HandleLightAttack(playerInventory.rightWeapon);
                }
            }

            if (rightAttackInput)
            {
                // ������ ����� ������ ó��
                playerAttack.HandleHeavyAttack(playerInventory.rightWeapon);
            }
        }

        // Jump ó��
        private void HandleJumpInput()
        {
            // �����̽��� Ű �Է� �� jumpInput�� true�� ����
            inputActions.PlayerActions.Jump.performed += i => jumpInput = true;
        }

        // Inventory ó��
        private void HandleInventoryInput()
        {
            if (inventoryInput)
            {
                inventoryFlag = !inventoryFlag; // �κ��丮 �̺�Ʈ(anim) Ȱ��ȭ
                
                if (inventoryFlag)
                {
                    // UI ����
                    uiManager.OpenSelectWindow();
                    uiManager.UpdateUI();
                    //uiManager.hudUI.SetActive(false);
                }
                else
                {
                    // UI �ݱ�
                    uiManager.CloseSelectWindow();
                    uiManager.CloseAllInventoryUI();
                    //uiManager.hudUI.SetActive(true);
                }
            }
        }

        // ���� ���� ó��
        private void HandleLockOnInput()
        {
            // ���� ���� �Է�(lockOnInput)�� Ȱ��ȭ ���, ���� ������ ����(lockOnFlag)���°� �ƴ� ��
            if (lockOnInput && lockOnFlag == false)
            {
                lockOnInput = false;
                //lockOnFlag = true;
                cameraHandler.HandleLockOn(); // ���� ���� ó�� Ȱ��ȭ
                
                // ���� ����� Ÿ���� ���� ���
                if (cameraHandler.nearestLockOnTarget != null)
                {
                    // ���� Ÿ���� ���� ����� Ÿ������ ����
                    cameraHandler.currentLockOnTarget = cameraHandler.nearestLockOnTarget;
                    lockOnFlag = true;
                }
            }
            // ���� ���� �Է�(lockOnInput)�� Ȱ��ȭ ���, ���� ������ ����(lockOnFlag)������ ��
            else if (lockOnInput && lockOnFlag)
            {
                lockOnInput = false;
                lockOnFlag = false;
                cameraHandler.ClearLockOnTarget(); // ���� ���� ó�� ��Ȱ��ȭ
            }

            // ���� ���� ����(lockOnFlag)���� ���� ���� ���� �Է�(lockOnLeftInput)�� Ȱ��ȭ�Ǿ��� ��
            if (lockOnFlag && lockOnLeftInput)
            {
                lockOnLeftInput = false;
                cameraHandler.HandleLockOn(); // ���� ���� ó�� Ȱ��ȭ
                if (cameraHandler.leftLockTarget != null)
                {
                    // ���� Ÿ���� ���� ���� ���� Ÿ������ ����
                    cameraHandler.currentLockOnTarget = cameraHandler.leftLockTarget;
                }
            }

            // ���� ���� ����(lockOnFlag)���� ������ ���� ���� �Է�(lockOnRightInput)�� Ȱ��ȭ�Ǿ��� ��
            if (lockOnFlag && lockOnRightInput)
            {
                lockOnRightInput = false;
                cameraHandler.HandleLockOn(); // ���� ���� ó�� Ȱ��ȭ
                if(cameraHandler.rightLockTarget != null)
                {
                    // ���� Ÿ���� ������ ���� ���� Ÿ������ ����
                    cameraHandler.currentLockOnTarget = cameraHandler.rightLockTarget;
                }
            }
            
            // ī�޶� ���� ����
            cameraHandler.SetCameraHeight();
        }

        // ġ��Ÿ ���� ó��
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