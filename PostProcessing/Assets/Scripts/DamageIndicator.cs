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
        // Post Process Volume���� Color Grading ȿ���� ������
        volume.profile.TryGetSettings(out colorGrading);
    }

    public void TakeDamage()
    {
        // �������� �Ծ��� �� ColorFilter ���� ����
        colorGrading.colorFilter.value = new Color(1, 0, 0, 1);  // ���� ������
        colorGrading.colorFilter.overrideState = true;

        // ���� �ð� �� ������ ������ ����
        Invoke("ResetColor", 0.1f);
    }

    private void ResetColor()
    {
        colorGrading.colorFilter.value = Color.white;
        colorGrading.colorFilter.overrideState = false;
    }
}
