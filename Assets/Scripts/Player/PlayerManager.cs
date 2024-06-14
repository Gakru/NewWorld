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
        // InputHandler ����
        InputHandler inputHandler;
        // Animator ����
        Animator animator;
        // CameraHandler ����
        CameraHandler cameraHandler;
        // PlayerStats ����
        PlayerStats playerStats;
        // PlayerAnimatorManager ����
        PlayerAnimatorManager playerAnimatorManager;
        // PlayerLocalMotions ����
        PlayerLocomotions playerLocalMotions;

        public bool isInteracting; // ��ȣ�ۿ� ����

        [Header("Player Flags")]
        public bool isSprinting; // Sprint ���� ����
        public bool isInAir; // ���� ���� ����
        public bool isGrounded; // ���� ���� ����
        public bool canDoCombo; // �޺� ���� ����
        public bool isInvulnerable; // ���� ���� ����

        private void Awake()
        {
            cameraHandler = FindObjectOfType<CameraHandler>(); // CameraHandler ������Ʈ ã��
            cameraHandler = CameraHandler.singleton; // cameraHandler �ʱ�ȭ
            inputHandler = GetComponent<InputHandler>(); // inputHandler ������Ʈ �ʱ�ȭ
            animator = GetComponentInChildren<Animator>(); // animator ������Ʈ �ʱ�ȭ
            playerStats = GetComponent<PlayerStats>(); // PlayerStats ������Ʈ �ʱ�ȭ
            playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>(); // PlayerAnimatorManager ������Ʈ �ʱ�ȭ
            playerLocalMotions = GetComponent<PlayerLocomotions>(); // playerLocalMotions ������Ʈ �ʱ�ȭ
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            float delta = Time.deltaTime;
            isInteracting = animator.GetBool("isInteracting"); // ��ȣ�ۿ� Ȯ��
            canDoCombo = animator.GetBool("canDoCombo"); // �޺��ۿ� Ȯ��
            isInvulnerable = animator.GetBool("isInvulnerable"); // �����ð� Ȯ��
            animator.SetBool("isInAir", isInAir); // ���� ����
            animator.SetBool("isDead", playerStats.isDead);

            inputHandler.ScriptInput(delta); // �Է� ó��
            playerLocalMotions.HandleRollingAndSprinting(delta); // Roll �� Sprint ó��
            playerLocalMotions.HandleJumping(); // ���� ó��
            playerStats.RegenrateShield(); // �ǵ� ��� ó��
            playerStats.RegenerateStamina(); // ���¹̳� ��� ó��
            playerAnimatorManager.canRotate = animator.GetBool("canRotate");
        }

        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;

            playerLocalMotions.HandleMovement(delta); // �̵� ó��
            playerLocalMotions.HandleFalling(delta, playerLocalMotions.moveDirection); // �߶� ó��
            playerLocalMotions.HandleRotation(delta); // ȸ�� ó��
        }

        private void LateUpdate()
        {
            inputHandler.rollFlag = false; // Roll �ʱ�ȭ
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
                // Ÿ�� �߰�
                cameraHandler.FollowTarget(delta);
                // ī�޶� ȸ�� ó��
                cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
            }

            if (isInAir)
            {
                // ���߿� �� �ִ� �ð� ������Ʈ
                playerLocalMotions.inAirTimer = playerLocalMotions.inAirTimer + Time.deltaTime;
            }
        }
    }
}