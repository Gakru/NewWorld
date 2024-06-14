using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace KHC
{
    public class PlayerAttack : MonoBehaviour
    {
        // PlayerManager ����
        PlayerManager playerManager;
        // PlayerStats ����
        PlayerStats playerStats;
        // PlayerInventory ����
        PlayerInventory playerInventory;
        // AnimatorHandler ����
        PlayerAnimatorManager animatorHandler;
        // InputHandler ����
        InputHandler inputHandler;
        public string lastAttack; // ������ ���� �ִϸ��̼� 1
        public string lastAttack2; // ������ ���� �ִϸ��̼� 2

        // WeaponSlotManager ����
        WeaponSlotManager weaponSlotManager;

        private void Awake()
        {
            playerManager = GetComponent<PlayerManager>(); // PlayerManager ������Ʈ �ʱ�ȭ
            playerStats = GetComponent<PlayerStats>(); // PlayerStats ������Ʈ �ʱ�ȭ
            animatorHandler = GetComponentInChildren<PlayerAnimatorManager>(); // AnimatorHandler ������Ʈ �ʱ�ȭ
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>(); // WeaponSlotManager ������Ʈ �ʱ�ȭ
            inputHandler = GetComponent<InputHandler>(); // InputHandler ������Ʈ �ʱ�ȭ
        }

        public void HandleWeaponCombo(WeaponItem weapon)
        {
            // ���¹̳��� ������ ����
            if (playerStats.currentStamina <= 0) return;

            // comboFlag�� ����Ǿ��� ��
            if (inputHandler.comboFlag)
            {
                animatorHandler.animator.SetBool("canDoCombo", false); // �޺� ��Ȱ��ȭ

                // ������ ������ ù ��° ������ ���, �� ��° ������ �����ϰ� ������ ������ �� ��° 
                if (lastAttack == weapon.OneHandLightAttack1)
                {
                    animatorHandler.animator.SetBool("canDoCombo", false);
                    animatorHandler.PlayAnimation(weapon.OneHandLightAttack2, true);
                    lastAttack2 = weapon.OneHandLightAttack2;

                    StartCoroutine(HandleLightLastAttack()); // ������ ������ �� ��° �������� ����
                }

                // ������ ������ �� ��° ������ ���, ù ��° ������ ����
                if (lastAttack == weapon.OneHandLightAttack2)
                {
                    animatorHandler.animator.SetBool("canDoCombo", false);
                    animatorHandler.PlayAnimation(weapon.OneHandLightAttack1, true);
                }
            }

        }
        private IEnumerator HandleLightLastAttack()
        {
            yield return null;
            lastAttack = lastAttack2; // ������ ������ �� ��° �������� ����
        }

        // �� ����
        public void HandleLightAttack(WeaponItem weapon)
        {
            // ���¹̳��� ������ ����
            if (playerStats.currentStamina <= 0) return;

            weaponSlotManager.attackingWeapon = weapon;

            animatorHandler.PlayAnimation(weapon.OneHandLightAttack1, true);
            lastAttack = weapon.OneHandLightAttack1;
        }
        // �� ����
        public void HandleHeavyAttack(WeaponItem weapon)
        {
            //weaponSlotManager.attackingWeapon = weapon;
            //animatorHandler.PlayAnimation(weapon.OneHandHeavyAttack1, true);
            //lastAttack = weapon.OneHandHeavyAttack1;
        }

        // �ް���
        //public void AttackBackStabOrCounterAttack()
        //{
        //    RaycastHit hit;

        //    if (Physics.Raycast(inputHandler.criticalAttackRayCastStartPoint.position,
        //        transform.TransformDirection(Vector3.forward), out hit, 0.5f, backStabLayer))
        //    {
        //        CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
        //        DamageCollider rightWeapon = weaponSlotManager.rightHandDamageCollider;

        //        if (enemyCharacterManager != null)
        //        {
        //            playerManager.transform.position = enemyCharacterManager.backStabCollider.backStabStandPoint.position;
        //            Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
        //            //rotationDirection = hit.transform.position - playerManager.transform.position;
        //            rotationDirection.y = 0f;
        //            rotationDirection.Normalize();
        //            Quaternion tr = Quaternion.LookRotation(rotationDirection);
        //            Quaternion targetDirection = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
        //            playerManager.transform.rotation = targetDirection;

        //            //int criticalDamage = playerInventory.rightWeapon.criticalDamage * rightWeapon.currentWeaponDamage;
        //            //enemyCharacterManager.pendingCriticalDamage = criticalDamage;

        //            animatorHandler.PlayTargetAnimation("Back Stab", true);
        //            enemyCharacterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Back Stabbed", true);
        //        }
        //    }
        //}

        // �ް���
        //public void AttemptBackStabOrRiposte()
        //{
        //    // ���¹̳��� ������ ����
        //    if (playerStats.currentStamina <= 0) return;

        //    RaycastHit hit;

        //    if (Physics.Raycast(inputHandler.criticalAttackRayCastStartPoint.position,
        //        transform.TransformDirection(Vector3.forward), out hit, 5f, backStabLayer))
        //    {
        //        CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
        //        DamageCollider rightWeapon = weaponSlotManager.rightHandDamageCollider;

        //        if (enemyCharacterManager != null)
        //        {
        //            //cheack team id
        //            Vector3 targetPosition = enemyCharacterManager.backStabCollider.backStabStandPoint.position;
        //            Vector3 startPosition = playerManager.transform.position;

        //            float dist = Vector3.Distance(targetPosition, startPosition);
        //            float moveTime = 5 / dist; // 5 is movement Speed from PlayerLocomotion, could set to public and grab it.

        //            Vector3 targetDir = targetPosition - startPosition;
        //            float angle = Vector3.Angle(targetDir, playerManager.transform.forward);

        //            Quaternion targetRotation = hit.transform.rotation;
        //            Quaternion startRotation = playerManager.transform.rotation;

        //            // ���� �߻� Null
        //            //int criticalDamage = playerInventory.rightWeapon.criticalDamage * rightWeapon.currentWeaponDamage;
        //            //enemyCharacterManager.pendingCriticalDamage = criticalDamage;

        //            StartCoroutine(HandleMoveToStab(moveTime, startPosition, targetPosition, targetRotation,
        //                startRotation, enemyCharacterManager, animatorHandler, angle));
        //        }
        //    }
        //}

        //public IEnumerator HandleMoveToStab(float moveTime, Vector3 startPosition, Vector3 targetPosition, Quaternion targetRotation,
        //Quaternion startRotation, CharacterManager enemyCharacterManager, PlayerAnimatorManager animatorHandler, float angle)
        //{
        //    float t = 0f;
        //    float cos = Mathf.Cos((angle * Mathf.PI) / 180);
        //    float sin = Mathf.Sin((angle * Mathf.PI) / 180);

        //    while (t <= 1f)
        //    {
        //        t += Time.deltaTime * moveTime;
        //        playerManager.transform.position = Vector3.Lerp(startPosition, targetPosition, t);

        //        playerManager.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

        //        animatorHandler.animator.SetFloat("Vertical", cos, 0.01f, Time.deltaTime);
        //        animatorHandler.animator.SetFloat("Horizontal", sin, 0.01f, Time.deltaTime);
        //        yield return null;
        //    }

        //    animatorHandler.animator.SetFloat("Vertical", 0, 0.01f, Time.deltaTime);
        //    animatorHandler.animator.SetFloat("Horizontal", 0, 0.01f, Time.deltaTime);
        //    animatorHandler.PlayAnimation("Back Stab", true);
        //    enemyCharacterManager.GetComponentInChildren<AnimatorManager>().PlayAnimation("Back Stabbed", true);
        //}
    }
}
