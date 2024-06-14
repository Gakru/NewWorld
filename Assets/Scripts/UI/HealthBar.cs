using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace KHC
{
    public class HealthBar : MonoBehaviour
    {
        // Slider 참조
        public Slider slider;
        // Text 참조
        public TextMeshProUGUI text;
        
        private void Start()
        {
            slider = GetComponent<Slider>(); // Slider 컴포넌트 초기화
            //text = FindObjectOfType<TextMeshProUGUI>(); // TextMeshProUGUI 컴포넌트 참조
        }

        // 최대 체력 설정
        public void MaxHealth(int maxHealth)
        {
            slider.maxValue = maxHealth; // 최대 값 설정
            slider.value = maxHealth; // slider의 현재 값을 최대 체력으로 설정
            text.text = slider.value.ToString();
        }

        // 현재 체력 설정
        public void CurrentHealth(int currentHealth)
        {
            slider.value = currentHealth; // slider의 현재 값을 현재 체력으로 설정
            text.text = slider.value.ToString(); // 실시간 체력 수치 설정
        }
    }
}
