using System.Collections;
using UnityEngine;

public class PortalColorController : MonoBehaviour
{
    [SerializeField] private Color _defaultMainColor;
    [SerializeField] private Color _defaultSparkColor;
    [SerializeField] private Color _defaultSmokeColor;
    [SerializeField] private Color _bossMainColor;
    [SerializeField] private Color _bossSparkColor;
    [SerializeField] private Color _bossSmokeColor;

    [SerializeField] private ParticleSystem _mainParticle;
    [SerializeField] private ParticleSystem _sparkParticle;
    [SerializeField] private ParticleSystem _smokeParticle;

    private ParticleSystem.MainModule _mainParticleMain;
    private ParticleSystem.MainModule _sparkParticleMain;
    private ParticleSystem.MainModule _smokeParticleMain;

    private float _changeColorTimer;
    private float _changeColorTime = 2f;

    private void Start()
    {
        _mainParticleMain = _mainParticle.main;
        _sparkParticleMain = _sparkParticle.main;
        _smokeParticleMain = _smokeParticle.main;
    }

    public void ChangeBossPortal()
    {
        _changeColorTimer = 0f;
        StartCoroutine(ChangeColorToBoss());
    }

    public void ChangeDefaultPortal()
    {
        _changeColorTimer = 0f;
        StartCoroutine(ChangeColorToDefault());
    }

    private IEnumerator ChangeColorToBoss()
    {
        yield return null;

        while (_changeColorTimer < _changeColorTime)
        {
            _mainParticleMain.startColor = Color.Lerp(_defaultMainColor, _bossMainColor, _changeColorTime / _changeColorTimer);
            _sparkParticleMain.startColor = Color.Lerp(_defaultSparkColor, _bossSparkColor, _changeColorTime / _changeColorTimer);
            _smokeParticleMain.startColor = Color.Lerp(_defaultSmokeColor, _bossSmokeColor, _changeColorTime / _changeColorTimer);
            _changeColorTimer += Time.deltaTime;
        }
    }

    private IEnumerator ChangeColorToDefault()
    {
        yield return null;

        while (_changeColorTimer < _changeColorTime)
        {
            _mainParticleMain.startColor = Color.Lerp(_bossMainColor, _defaultMainColor, _changeColorTime / _changeColorTimer);
            _sparkParticleMain.startColor = Color.Lerp(_bossSparkColor, _defaultSparkColor, _changeColorTime / _changeColorTimer);
            _smokeParticleMain.startColor = Color.Lerp(_bossSmokeColor, _defaultSmokeColor, _changeColorTime / _changeColorTimer);
            _changeColorTimer += Time.deltaTime;
        }
    }
}
