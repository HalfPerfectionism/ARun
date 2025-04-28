using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatBar : MonoBehaviour
{
    public Image healthImage;
    public Image healthDelayImage;
    public Image BlueImage;

    private void Update()
    {
        //Ѫ������Ч��
        if(healthDelayImage.fillAmount > healthImage.fillAmount)
        {
            healthDelayImage.fillAmount -= Time.deltaTime * 0.1f;
        }
    }

    public void OnHealthChange(float persentage)
    {
        healthImage.fillAmount = persentage; 
    }
}
