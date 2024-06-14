using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KHC
{
    public class StaminaBar : MonoBehaviour
    {
        public Slider slider;

        // Start is called before the first frame update
        void Start()
        {
            slider = GetComponent<Slider>();
        }

        public void MaxStamina(float maxStamina)
        {
            slider.maxValue = maxStamina;
            slider.value = maxStamina;
        }

        public void CurrentStamina(float currentStamina)
        {
            slider.value = currentStamina;
        }
    }
}
