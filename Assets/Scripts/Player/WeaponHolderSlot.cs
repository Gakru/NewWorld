using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KHC
{
    public class WeaponHolderSlot : MonoBehaviour
    {
        public Transform parentOverride;
        public bool isLeftHandSlot; // 왼손 슬롯 여부
        public bool isRightHandSlot; // 오른손 슬롯 여부
        public bool isBackSlot;

        // GameObject(현재무기모델) 참조
        public GameObject currrentWeaponModel;

        // 무기 해제
        public void UnloadWeapon()
        {
            // 현재 장착된 무기가 없으면 비활성화
            if (currrentWeaponModel != null)
            {
                currrentWeaponModel.SetActive(false);
            }
        }

        // 무기 해제 및 삭제
        public void UnloadWeaponAndDestroy()
        {
            // 현재 장착된 무기가 없으면 삭제
            if (currrentWeaponModel == null)
            {
                Destroy(currrentWeaponModel);
            }
        }

        // 무기 모델 로드
        public void LoadWeaponModel(WeaponItem weaponItem)
        {
            UnloadWeaponAndDestroy(); // 먼저 무기 해제 및 삭제

            // 무기 아이템이 없다면 무기 해제 및 종료
            if (weaponItem == null)
            {
                UnloadWeapon();
                return;
            }

            // 무기 아이템의 모델 프리팹을 인스턴스화
            GameObject model = Instantiate(weaponItem.modelPrefab) as GameObject;
            if (model != null)
            {
                // 부모 오버라이드가 있다면 해당 부모를 설정, 없다면 현재 오브젝트의 부모를 설정
                if (parentOverride != null)
                {
                    model.transform.parent = parentOverride;
                }
                else
                {
                    model.transform.parent = transform;
                }

                // 모델의 위치, 회전, 스케일을 기본 값으로 설정
                model.transform.localPosition = Vector3.zero;
                model.transform.localRotation = Quaternion.identity;
                model.transform.localScale = Vector3.one;
            }

            // 현재 장착된 무기 모델을 새로 로드한 모델로 설정
            currrentWeaponModel = model;
        }
    }
}