using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;
using STARTD.Common;

public class HealthBarManager : SingletonBehaviour<HealthBarManager>
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private RectTransform canvasRect;
    [SerializeField] private GameObject healthBarPrefab; // Image prefab
    [SerializeField] private Vector3 offset = new Vector3(0, 2, 0);

    // 생성된 HealthBar를 관리
    private Dictionary<Transform, RectTransform> healthBars = new Dictionary<Transform, RectTransform>();

    private void LateUpdate()
    {
        // 모든 등록된 HealthBar 위치 업데이트
        foreach (var kvp in healthBars)
        {
            UpdateHealthBar(kvp.Key, kvp.Value, offset);
        }
    }

    /// <summary>
    /// HealthBar 생성 및 등록
    /// </summary>
    public void CreateHealthBar(Transform target, Color color) 
    {
        if (healthBars.ContainsKey(target))
            return; // 이미 생성되어 있음

        GameObject barObj = Instantiate(healthBarPrefab, canvasRect);
        RectTransform barRect = barObj.GetComponent<RectTransform>();


        Image fillImage = barObj.GetComponent<Image>();
        fillImage.color = color;

        // 처음에는 비활성화
        barRect.gameObject.SetActive(false);

        healthBars.Add(target, barRect);
    }

    /// <summary>
    /// HealthBar 제거
    /// </summary>
    public void RemoveHealthBar(Transform target)
    {
        if (healthBars.TryGetValue(target, out RectTransform bar))
        {
            Destroy(bar.gameObject);
            healthBars.Remove(target);
        }
    }

    /// <summary>
    /// HealthBar 위치 업데이트
    /// </summary>
    private void UpdateHealthBar(Transform target, RectTransform healthBar, Vector3 offset)
    {
        Vector3 worldPos = target.position + offset;
        healthBar.position = worldPos;

        // 카메라 바라보기
        healthBar.rotation = Quaternion.LookRotation(healthBar.position - Camera.main.transform.position);
    }

    /// <summary>
    /// 체력 비율 업데이트
    /// </summary>
    public void SetHealth(Transform target, float current, float max)
    {
        if (healthBars.TryGetValue(target, out RectTransform bar))
        {
            Image fillImage = bar.GetComponent<Image>();
            if (fillImage != null)
            {
                // 체력이 줄어들었을 때 활성화
                if (!bar.gameObject.activeSelf && current < max)
                    bar.gameObject.SetActive(true);

                fillImage.fillAmount = Mathf.Clamp01(current / max);
            }
        }
    }
}