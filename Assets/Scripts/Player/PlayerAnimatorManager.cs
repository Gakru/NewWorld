using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KHC
{
    public class PlayerAnimatorManager : AnimatorManager
    {
        // PlayerManager ����
        PlayerManager playerManager;
        // PlayerStats ����
        PlayerStats playerStats;
        // InputHandler ����
        InputHandler inputHandler;
        // PlayerLocalMotions ����
        PlayerLocomotions playerLocalMotions;
        int vertical; // ���� ���� �Ķ���� �ؽ�
        int horizontal; // ���� ���� �Ķ���� �ؽ�
        //public bool canRotate; // ȸ�� ���� ����

        // �ʱ�ȭ
        public void Initialized()
        {
            playerManager = GetComponentInParent<PlayerManager>(); // PlayerManager ������Ʈ �ʱ�ȭ
            playerStats = GetComponentInParent<PlayerStats>(); // PlayerStats �������� �ʱ�ȭ
            animator = GetComponent<Animator>(); // Animator ������Ʈ �ʱ�ȭ
            inputHandler = GetComponentInParent<InputHandler>(); // InputHanler ������Ʈ �ʱ�ȭ
            playerLocalMotions = GetComponentInParent<PlayerLocomotions>(); // PlayerLocalMotions ������Ʈ �ʱ�ȭ
            vertical = Animator.StringToHash("Vertical"); // ���� ���� �Ķ���� �ؽ� �� �ʱ�ȭ
            horizontal = Animator.StringToHash("Horizontal"); // ���� ���� �Ķ���� �ؽ� �� �ʱ�ȭ
        }

        public void UpdateAnimatorValue(float verticalMovemnet, float horizontalMovemnet, bool isSprinting)
        {
            #region Vertical
            float v = 0f;

            // ���� ���� �����ӿ� ���� �Ķ���� �� ����
            if (verticalMovemnet > 0f && verticalMovemnet < 0.55f) v = 0.5f;
            else if (verticalMovemnet > 0.55f) v = 1f;
            else if (verticalMovemnet < 0f && verticalMovemnet > -0.55f) v = -0.5f;
            else if (verticalMovemnet < -0.55f) v = -1f;
            else v = 0f;
            #endregion

            #region Horizontal;
            float h = 0f;

            // ���� ���� �����ӿ� ���� �Ķ���� �� ����
            if (horizontalMovemnet > 0f && horizontalMovemnet < 0.55f) h = 0.5f;
            else if (horizontalMovemnet > 0.55f) h = 1f;
            else if (horizontalMovemnet < 0f && horizontalMovemnet > -0.55f) h = -0.5f;
            else if (horizontalMovemnet < -0.55f) h = -1f;
            else h = 0f;
            #endregion

            // Sprint ���� �� �Ķ���� �� ����
            if (isSprinting)
            {
                v = 2f;
                h = horizontalMovemnet;
            }

            animator.SetFloat(vertical, v, 0.1f, Time.deltaTime); // ���� ���� �Ķ���� ����
            animator.SetFloat(horizontal, h, 0.1f, Time.deltaTime); // ���� ���� �Ķ���� ����
        }

        // ȸ�� ó��(����)
        public void CanRotate()
        {
            animator.SetBool("canRotate", true);
        }
        // ȸ�� ó��(�Ұ���)
        public void StopRotation()
        {
            animator.SetBool("canRotate", false);
        }

        // �޺� ó��(����)
        public void EnableCombo()
        {
            animator.SetBool("canDoCombo", true);
        }
        // �޺� ó��(�Ұ���)
        public void DisableCombo()
        {
            animator.SetBool("canDoCombo", false);
        }

        // ���� ó��(����)
        public void EnableIsInvulnerable()
        {
            animator.SetBool("isInvulnerable", true);
        }

        // ���� ó��(�Ұ���)
        public void DisableIsInvulnerable()
        {
            animator.SetBool("isInvulnerable", false);
        }

        private void OnAnimatorMove()
        {
            if (playerManager.isInteracting == false) return;

            float delta = Time.deltaTime;
            playerLocalMotions.rigidbody.drag = 0; // �巡�� �� �ʱ�ȭ
            Vector3 deltaPosition = animator.deltaPosition; // animator�� deltaPosition ��������
            deltaPosition.y = 0; // ���� ���� ����(���� �̵�X)
            Vector3 velocity = deltaPosition / delta; // deltaPosition�� �ð��� ���� �ӵ��� ��ȯ
            playerLocalMotions.rigidbody.velocity = velocity; // rigidbody �ӵ��� velocity�� ����
        }
    }
}