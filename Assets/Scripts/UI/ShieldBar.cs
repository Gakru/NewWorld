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
        // Slider ����
        public Slider slider;
        // Text ����
        public TextMeshProUGUI text;

        private void Start()
        {
            slider = GetComponent<Slider>(); // Slider ������Ʈ �ʱ�ȭ
            text = FindObjectOfType<TextMeshProUGUI>(); // TextMeshProUGUI ������Ʈ ����
        }

        // �ִ� ���� ����
        public void MaxShield(float maxShield)
        {
            slider.maxValue = maxShield; // �ִ� �� ����
            slider.value = maxShield; // slider�� ���� ���� �ִ� ����� ����
            text.text = slider.value.ToString();
        }

        // ���� ���� ����
        public void CurrentShield(float currentShield)
        {
            slider.value = currentShield; // slider�� ���� ���� ���� ����� ����
            text.text = slider.value.ToString(); // �ǽð� ���� ��ġ ����
        }
    }
}
