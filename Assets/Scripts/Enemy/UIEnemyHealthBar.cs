using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace KHC
{
    public class UIEnemyHealthBar : MonoBehaviour
    {
        // Slider ����
        Slider slider;

        float hideHealthBar = 0; // ü�¹� ���� �ð�

        Transform cameraTransform;

        private void Awake()
        {
            slider = GetComponentInChildren<Slider>(); // Slider ������Ʈ �ʱ�ȭ
        }

        private void Start()
        {
            cameraTransform = Camera.main.transform;
        }

        public void SetHealth(int health)
        {
            slider.value = health; // ü�� ����
            hideHealthBar = 3;
        }

        public void SetMaxHealth(int maxHealth)
        {
            // �ִ� ü�� ����
            slider.maxValue = maxHealth;
            slider.value = maxHealth;
        }

        private void Update()
        {
            // �һ� ī�޶� �������� �ٶ� �� �ֵ��� �����̼� �� ����
            transform.LookAt(transform.position + cameraTransform.rotation * Vector3.forward, cameraTransform.rotation * Vector3.up);

            // ü�� ���� �ð����� �ǽð� ����
            hideHealthBar = hideHealthBar - Time.deltaTime;

            // Null ���� ����
            if (slider != null)
            {
                // ü�� ���� �ð��� 0�� �Ǹ�
                if (hideHealthBar <= 0)
                {
                    // 0�ʷ� ����
                    hideHealthBar = 0;
                    slider.gameObject.SetActive(false); // Slider ��Ȱ��ȭ
                }
                else
                {
                    // [��MEMO��]
                    // activeSelf: SetActive(bool)�� ������ ����
                    // activeInHierarchy: SetActive(bool)�� ���� ���ϸ�, �θ� �ִٸ� �θ��� SetActive(bool)�� ������ �޴´�. (�ڽź��� �θ���¸� ���� ������)

                    if (!slider.gameObject.activeInHierarchy)
                    {
                        slider.gameObject.SetActive(true); // Slider Ȱ��ȭ
                    }
                }

                // Slider�� 0���� ũ��
                if (slider.value <= 0)
                {
                    Destroy(slider.gameObject); // Slider ����
                }
            }
        }
    }
}
