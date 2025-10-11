// File: DamagePopUpAnimation.cs
using TMPro;
using UnityEngine;

public class DamagePopUpAnimation : MonoBehaviour
{
    public AnimationCurve opacityCurve;
    public AnimationCurve scaleCurve;
    public AnimationCurve heightCurve;
    public TextMeshProUGUI tmp;
    public float duration = 1f;

    private float time = 0;
    private Vector3 originPosition;
    private Vector3 originScale; // Biến này sẽ được gán từ bên ngoài

    void Awake()
    {
        originPosition = transform.position;
        // XÓA DÒNG NÀY ĐI: originScale = DamePopUpGenerator.Instance.originScaleDame;

        if (tmp == null)
        {
            tmp = GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    // ---- HÀM MỚI ĐỂ NHẬN GIÁ TRỊ TỪ BÊN NGOÀI ----
    public void Initialize(Vector3 startScale)
    {
        this.originScale = startScale;
        // Áp dụng kích thước ban đầu ngay lập tức để tránh bị giật
        transform.localScale = startScale;
    }

    void Update()
    {
        time += Time.deltaTime;
        float progress = time / duration;

        float opacity = opacityCurve.Evaluate(progress);
        tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, opacity);

        // Logic này bây giờ sẽ hoạt động đúng vì originScale đã chính xác
        float scaleMultiplier = scaleCurve.Evaluate(progress);
        transform.localScale = originScale * scaleMultiplier;

        float height = heightCurve.Evaluate(progress);
        transform.position = originPosition + new Vector3(0, height, 0);

        if (time >= duration)
        {
            Destroy(gameObject);
        }
    }
}