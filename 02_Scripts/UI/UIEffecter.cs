using UnityEngine;
using DG.Tweening;

public class UIEffecter : MonoBehaviour
{
    [SerializeField] private float duration = 0.5f; 
    private RectTransform rectTransform;
    private Tween scaleTween;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        if (scaleTween != null)
        {
            scaleTween.Kill();
        }

        rectTransform.localScale = Vector3.zero;

        scaleTween = rectTransform.DOScale(1f, duration)
            .SetEase(Ease.OutBack)
            .OnKill(() => scaleTween = null);
    }
}
