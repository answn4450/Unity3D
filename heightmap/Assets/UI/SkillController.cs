using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillController : MonoBehaviour, UIInterface
{
    private SkillInfo Info;

    private Image SkillImage;

    void Start()
    {
        Info = GetComponent<SkillInfo>();

        SkillImage = GetComponent<Image>();
    }

    public void SetActive()
    {
        StartCoroutine(SetCooldown());
    }


    IEnumerator SetCooldown()
    {
        float fTime = 0.0f;

        while(fTime < Info.coolDown)
        {
            fTime += Time.deltaTime;

            SkillImage.fillAmount = (fTime / Info.coolDown);

            yield return null;
        }
    }
}
