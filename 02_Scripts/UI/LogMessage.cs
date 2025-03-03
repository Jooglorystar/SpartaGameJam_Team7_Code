using TMPro;
using UnityEngine;
using DG.Tweening;

public class LogMessage : MonoBehaviour
{
    [SerializeField] float _duration = 0.5f;
    [SerializeField] float _holdTime = 1f;   // 유지 시간
    TextMeshProUGUI _text;

    Sequence _scaleSequence; // DOTween 시퀀스

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }
    public void SetLog(string message)
    {
        if (_scaleSequence != null)
        {
            _scaleSequence.Kill();
        }

        _text.transform.localScale = Vector3.zero;
        gameObject.SetActive(true); 

        _text.text = message; 

        _scaleSequence = DOTween.Sequence()
            .Append(_text.transform.DOScale(1f, _duration).SetEase(Ease.OutElastic)) 
            .AppendInterval(_holdTime) 
            .Append(_text.transform.DOScale(0f, _duration).SetEase(Ease.InBack)) 
            .OnComplete(() => gameObject.SetActive(false)); 

        _scaleSequence.Play();
    }
}
