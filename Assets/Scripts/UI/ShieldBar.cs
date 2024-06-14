using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace KHC
{
    public class ShieldBar : MonoBehaviour
    {
        // Slider 참조
        public Slider slider;
        // Text 참조
        public TextMeshProUGUI text;

        private void Start()
        {
            slider = GetComponent<Slider>(); // Slider 컴포넌트 초기화
            text = FindObjectOfType<TextMeshProUGUI>(); // TextMeshProUGUI 컴포넌트 참조
        }

        // 최대 쉴드 설정
        public void MaxShield(float maxShield)
        {
            slider.maxValue = maxShield; // 최대 값 설정
            slider.value = maxShield; // slider의 현재 값을 최대 쉴드로 설정
            text.text = slider.value.ToString();
        }

        // 현재 쉴드 생성
        public void CurrentShield(float currentShield)
        {
            slider.value = currentShield; // slider의 현재 값을 현재 쉴드로 설정
            text.text = slider.value.ToString(); // 실시간 쉴드 수치 설정
        }
    }
}
