using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace KHC
{
    public class PlayerAttack : MonoBehaviour
    {
        // PlayerManager 참조
        PlayerManager playerManager;
        // PlayerStats 참조
        PlayerStats playerStats;
        // PlayerInventory 참조
        PlayerInventory playerInventory;
        // AnimatorHandler 참조
        PlayerAnimatorManager animatorHandler;
        // InputHandler 참조
        InputHandler inputHandler;
        public string lastAttack; // 마지막 공격 애니메이션 1
        public string lastAttack2; // 마지막 공격 애니메이션 2

        // WeaponSlotManager 참조
        WeaponSlotManager weaponSlotManager;

        private void Awake()
        {
            playerManager = GetComponent<PlayerManager>(); // PlayerManager 컴포넌트 초기화
            playerStats = GetComponent<PlayerStats>(); // PlayerStats 컴포넌트 초기화
            animatorHandler = GetComponentInChildren<PlayerAnimatorManager>(); // AnimatorHandler 컴포넌트 초기화
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>(); // WeaponSlotManager 컴포넌트 초기화
            inputHandler = GetComponent<InputHandler>(); // InputHandler 컴포넌트 초기화
        }

        public void HandleWeaponCombo(WeaponItem weapon)
        {
            // 스태미나가 없으면 종료
            if (playerStats.currentStamina <= 0) return;

            // comboFlag가 실행되었을 시
            if (inputHandler.comboFlag)
            {
                animatorHandler.animator.SetBool("canDoCombo", false); // 콤보 비활설화

                // 마지막 공격이 첫 번째 공격인 경우, 두 번째 공격을 실행하고 마지막 공격을 두 번째 
                if (lastAttack == weapon.OneHandLightAttack1)
                {
                    animatorHandler.animator.SetBool("canDoCombo", false);
                    animatorHandler.PlayAnimation(weapon.OneHandLightAttack2, true);
                    lastAttack2 = weapon.OneHandLightAttack2;

                    StartCoroutine(HandleLightLastAttack()); // 마지막 공격을 두 번째 공격으로 설정
                }

                // 마지막 공격이 두 번째 공격인 경우, 첫 번째 공격을 실행
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
            lastAttack = lastAttack2; // 마지막 공격을 두 번째 공격으로 설정
        }

        // 약 공격
        public void HandleLightAttack(WeaponItem weapon)
        {
            // 스태미나가 없으면 종료
            if (playerStats.currentStamina <= 0) return;

            weaponSlotManager.attackingWeapon = weapon;

            animatorHandler.PlayAnimation(weapon.OneHandLightAttack1, true);
            lastAttack = weapon.OneHandLightAttack1;
        }
        // 강 공격
        public void HandleHeavyAttack(WeaponItem weapon)
        {
            //weaponSlotManager.attackingWeapon = weapon;
            //animatorHandler.PlayAnimation(weapon.OneHandHeavyAttack1, true);
            //lastAttack = weapon.OneHandHeavyAttack1;
        }

        // 뒷공격
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

        // 뒷공격
        //public void AttemptBackStabOrRiposte()
        //{
        //    // 스태미나가 없으면 종료
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

        //            // 에러 발생 Null
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
