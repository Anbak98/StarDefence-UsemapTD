using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Camera))]
public class OrthoCameraFit : MonoBehaviour
{
    public SpriteRenderer targetSprite;

    private Camera cam;

    private void OnEnable()
    {
        cam = GetComponent<Camera>();
        Fit();
    }

    private void OnValidate()
    {
        cam = GetComponent<Camera>();
        Fit();
    }

    private void Update()
    {
        if (!Application.isPlaying)
            Fit();
    }

    private void Fit()
    {
        if (cam == null || targetSprite == null)
            return;

        if (!cam.orthographic)
            cam.orthographic = true;

        Bounds bounds = targetSprite.bounds;

        float spriteWidth = bounds.size.x;
        float spriteHeight = bounds.size.y;

        float camAspect = cam.aspect;

        // 카메라 높이 vs 스프라이트 높이 기준
        float sizeByHeight = spriteHeight * 0.5f;

        // 카메라 폭이 기준이 되는 경우
        float sizeByWidth = (spriteWidth * 0.5f) / camAspect;

        cam.orthographicSize = Mathf.Min(sizeByHeight, sizeByWidth);

        // 카메라를 스프라이트 정중앙으로 이동
        cam.transform.position = new Vector3(
            bounds.center.x,
            bounds.center.y,
            cam.transform.position.z
        );
    }
}
