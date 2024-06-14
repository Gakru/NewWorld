using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KHC
{
    public class WeaponHolderSlot : MonoBehaviour
    {
        public Transform parentOverride;
        public bool isLeftHandSlot; // �޼� ���� ����
        public bool isRightHandSlot; // ������ ���� ����
        public bool isBackSlot;

        // GameObject(���繫���) ����
        public GameObject currrentWeaponModel;

        // ���� ����
        public void UnloadWeapon()
        {
            // ���� ������ ���Ⱑ ������ ��Ȱ��ȭ
            if (currrentWeaponModel != null)
            {
                currrentWeaponModel.SetActive(false);
            }
        }

        // ���� ���� �� ����
        public void UnloadWeaponAndDestroy()
        {
            // ���� ������ ���Ⱑ ������ ����
            if (currrentWeaponModel == null)
            {
                Destroy(currrentWeaponModel);
            }
        }

        // ���� �� �ε�
        public void LoadWeaponModel(WeaponItem weaponItem)
        {
            UnloadWeaponAndDestroy(); // ���� ���� ���� �� ����

            // ���� �������� ���ٸ� ���� ���� �� ����
            if (weaponItem == null)
            {
                UnloadWeapon();
                return;
            }

            // ���� �������� �� �������� �ν��Ͻ�ȭ
            GameObject model = Instantiate(weaponItem.modelPrefab) as GameObject;
            if (model != null)
            {
                // �θ� �������̵尡 �ִٸ� �ش� �θ� ����, ���ٸ� ���� ������Ʈ�� �θ� ����
                if (parentOverride != null)
                {
                    model.transform.parent = parentOverride;
                }
                else
                {
                    model.transform.parent = transform;
                }

                // ���� ��ġ, ȸ��, �������� �⺻ ������ ����
                model.transform.localPosition = Vector3.zero;
                model.transform.localRotation = Quaternion.identity;
                model.transform.localScale = Vector3.one;
            }

            // ���� ������ ���� ���� ���� �ε��� �𵨷� ����
            currrentWeaponModel = model;
        }
    }
}