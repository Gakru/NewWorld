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
        // Slider 참조
        Slider slider;

        float hideHealthBar = 0; // 체력바 숨김 시간

        Transform cameraTransform;

        private void Awake()
        {
            slider = GetComponentInChildren<Slider>(); // Slider 컴포너트 초기화
        }

        private void Start()
        {
            cameraTransform = Camera.main.transform;
        }

        public void SetHealth(int health)
        {
            slider.value = health; // 체력 적용
            hideHealthBar = 3;
        }

        public void SetMaxHealth(int maxHealth)
        {
            // 최대 체력 적용
            slider.maxValue = maxHealth;
            slider.value = maxHealth;
        }

        private void Update()
        {
            // 할상 카메라를 정면으로 바라볼 수 있도록 로테이션 값 조정
            transform.LookAt(transform.position + cameraTransform.rotation * Vector3.forward, cameraTransform.rotation * Vector3.up);

            // 체력 숨김 시간에서 실시간 감소
            hideHealthBar = hideHealthBar - Time.deltaTime;

            // Null 에러 방지
            if (slider != null)
            {
                // 체력 숨김 시간이 0이 되면
                if (hideHealthBar <= 0)
                {
                    // 0초로 변경
                    hideHealthBar = 0;
                    slider.gameObject.SetActive(false); // Slider 비활성화
                }
                else
                {
                    // [★MEMO★]
                    // activeSelf: SetActive(bool)에 영향을 받음
                    // activeInHierarchy: SetActive(bool)에 따라 변하며, 부모가 있다면 부모의 SetActive(bool)에 영향을 받는다. (자신보다 부모상태를 먼저 따른다)

                    if (!slider.gameObject.activeInHierarchy)
                    {
                        slider.gameObject.SetActive(true); // Slider 활성화
                    }
                }

                // Slider가 0보다 크면
                if (slider.value <= 0)
                {
                    Destroy(slider.gameObject); // Slider 삭제
                }
            }
        }
    }
}
