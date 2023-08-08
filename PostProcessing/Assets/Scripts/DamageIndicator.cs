using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DamageIndicator : MonoBehaviour
{
    public PostProcessVolume volume;
    private ColorGrading colorGrading;

    void Start()
    {
        // Post Process Volume에서 Color Grading 효과를 가져옴
        volume.profile.TryGetSettings(out colorGrading);
    }

    public void TakeDamage()
    {
        // 데미지를 입었을 때 ColorFilter 값을 변경
        colorGrading.colorFilter.value = new Color(1, 0, 0, 1);  // 진한 빨간색
        colorGrading.colorFilter.overrideState = true;

        // 일정 시간 후 원래의 값으로 돌림
        Invoke("ResetColor", 0.1f);
    }

    private void ResetColor()
    {
        colorGrading.colorFilter.value = Color.white;
        colorGrading.colorFilter.overrideState = false;
    }
}
