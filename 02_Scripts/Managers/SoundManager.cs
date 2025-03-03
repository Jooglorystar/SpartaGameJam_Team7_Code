using System.Threading;
using UnityEngine;
using System.Collections.Generic;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] AudioSource _bgmSource;
    [SerializeField] AudioSource _sfxSource;
    [SerializeField] float _fadeDuration = 1.0f;

    public AudioSource BGMSource => _bgmSource;
    public AudioSource SFXSource => _sfxSource;

    CancellationTokenSource _source;

    Dictionary<string, AudioClip> _dic = new Dictionary<string, AudioClip>();

    public void PlayBGM(string clipName, float volume = 1f, bool loop = true)
    {
        if(!_dic.TryGetValue(clipName, out AudioClip clip))
        {
            clip = Resources.Load<AudioClip>($"BGM/{clipName}");

            if (clip == null)
                return;

            _dic[clipName] = clip;
        }

        PlayBGM(clip, volume, loop);
    }

    public void PlaySFX(string clipName, float volume = 1f)
    {
        if (!_dic.TryGetValue(clipName, out AudioClip clip))
        {
            clip = Resources.Load<AudioClip>($"SFX/{clipName}");

            if (clip == null)
                return;

            _dic[clipName] = clip;
        }

        PlaySFX(clip, volume);
    }

    /// <summary>
    /// ������� (BGM) ���
    /// </summary>
    void PlayBGM(AudioClip clip, float volume = 1f, bool loop = true)
    {
        if (_bgmSource.clip == clip) return; // ���� �����̸� ��� �� ��

        _source?.Cancel();
        _source = new CancellationTokenSource();

        _ = FadeOutAndChangeBGM(clip, volume, loop, _source.Token);
    }

    /// <summary>
    /// ȿ���� (SFX) ���
    /// </summary>
    void PlaySFX(AudioClip clip, float volume = 1f)
    {
        _sfxSource.PlayOneShot(clip, volume);
    }

    /// <summary>
    /// ��� ���� ����
    /// </summary>
    public void StopAllSounds()
    {
        _bgmSource.Stop();
        _sfxSource.Stop();
    }

    /// <summary>
    /// ������� ����
    /// </summary>
    public void StopBGM()
    {
        _bgmSource.Stop();
    }

    /// <summary>
    /// ������� ���̵� �ƿ� �� ���ο� ���� ���
    /// </summary>
    async Awaitable FadeOutAndChangeBGM(AudioClip newClip, float targetVolume, bool loop, CancellationToken token)
    {
        await FadeOutBGM(token);

        _bgmSource.clip = newClip;
        _bgmSource.loop = loop;
        _bgmSource.Play();

        await FadeInBGM(targetVolume, token);
    }

    async Awaitable FadeOutBGM(CancellationToken token)
    {
        float startVolume = _bgmSource.volume;

        for (float t = 0; t < _fadeDuration; t += Time.deltaTime)
        {
            if (token.IsCancellationRequested)
                return;

            _bgmSource.volume = Mathf.Lerp(startVolume, 0, t / _fadeDuration);
            await Awaitable.NextFrameAsync();
        }

        _bgmSource.volume = 0;
        _bgmSource.Stop();
    }

    async Awaitable FadeInBGM(float targetVolume, CancellationToken token)
    {
        float startVolume = 0;
        _bgmSource.volume = startVolume;

        for (float t = 0; t < _fadeDuration; t += Time.deltaTime)
        {
            if (token.IsCancellationRequested)
                return;

            _bgmSource.volume = Mathf.Lerp(startVolume, targetVolume, t / _fadeDuration);
            await Awaitable.NextFrameAsync();
        }

        _bgmSource.volume = targetVolume;
    }
}
