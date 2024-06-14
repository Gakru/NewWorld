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
        // Slider ����
        public Slider slider;
        // Text ����
        public TextMeshProUGUI text;
        
        private void Start()
        {
            slider = GetComponent<Slider>(); // Slider ������Ʈ �ʱ�ȭ
            //text = FindObjectOfType<TextMeshProUGUI>(); // TextMeshProUGUI ������Ʈ ����
        }

        // �ִ� ü�� ����
        public void MaxHealth(int maxHealth)
        {
            slider.maxValue = maxHealth; // �ִ� �� ����
            slider.value = maxHealth; // slider�� ���� ���� �ִ� ü������ ����
            text.text = slider.value.ToString();
        }

        // ���� ü�� ����
        public void CurrentHealth(int currentHealth)
        {
            slider.value = currentHealth; // slider�� ���� ���� ���� ü������ ����
            text.text = slider.value.ToString(); // �ǽð� ü�� ��ġ ����
        }
    }
}
