using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private Transform target; // 체력바가 따라갈 오브젝트
    [SerializeField] private Vector3 offset = new Vector3(0, 2, 0);

    private void LateUpdate()
    {
        if (target != null)
            transform.position = target.position + offset;
    }

    public void SetHealth(float current, float max)
    {
        fillImage.fillAmount = current / max;
    }
}
