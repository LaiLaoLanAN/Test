using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour
{
    [Header("References")]
    private static MusicManager _instance;
    public static MusicManager Instance => _instance;
    [SerializeField] private AudioMixerGroup _musicMixerGroup;
    [SerializeField] private MusicData _musicData;

    [Header("Settings")]
    [Range(0f, 1f)] public float globalVolume = 1f;
    public float crossFadeDuration = 5f;

    public AudioSource _primarySource;
    public AudioSource _secondarySource;
    private Dictionary<string, AudioClip> _musicLibrary = new Dictionary<string, AudioClip>();
    private float _primaryVolume;
    private float _secondaryVolume;

    private void Awake()
    {
          if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeAudioSources();
        BuildMusicLibrary();
    }

    private void InitializeAudioSources()
    {
        _primarySource.outputAudioMixerGroup = _musicMixerGroup;
        _secondarySource.outputAudioMixerGroup = _musicMixerGroup;

        _primarySource.loop = true;
        _secondarySource.loop = true;
    }

    private void BuildMusicLibrary()
    {
        if (_musicData == null)
        {
            Debug.LogWarning("No MusicData assigned!");
            return;
        }

        foreach (var track in _musicData.musicTracks)
        {
            _musicLibrary.Add(track.trackName, track.clip);
        }
    }

    public void PlayTrack(string trackName, bool forceRestart = false)
    {
        if (!_musicLibrary.ContainsKey(trackName))
        {
            Debug.LogError($"Track '{trackName}' not found in library!");
            return;
        }

        if (!forceRestart && _primarySource.clip == _musicLibrary[trackName] && _primarySource.isPlaying)
        {
            Debug.Log($"Track '{trackName}' is already playing.");
            return;
        }

        StartCoroutine(CrossFade(_musicLibrary[trackName]));
    }

    private IEnumerator CrossFade(AudioClip newClip)
    {
        // 将当前主音源转移到次音源
        _secondarySource.clip = _primarySource.clip;
        _secondarySource.volume = _primarySource.volume;
        _secondarySource.time = _primarySource.time;
        _secondarySource.Play();
        yield return new WaitForSeconds(0.5f);
        // 设置新音源为主音源
        _primarySource.clip = newClip;
        _primarySource.volume = 0f;
        _primarySource.Play();

        float timer = 0f;
        while (timer < crossFadeDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / crossFadeDuration;

            _primarySource.volume = Mathf.Lerp(0f, globalVolume, progress);
            _secondarySource.volume = Mathf.Lerp(globalVolume, 0f, progress);

            yield return null;
        }

        _primarySource.volume = globalVolume;
        _secondarySource.Stop();
    }

    public void PauseMusic()
    {
        _primarySource.Pause();
        _secondarySource.Pause();
    }

    public void ResumeMusic()
    {
        _primarySource.UnPause();
        _secondarySource.UnPause();
    }

    public void StopMusic()
    {
        _primarySource.Stop();
        _secondarySource.Stop();
    }

    public void SetVolume(float volume)
    {
        globalVolume = Mathf.Clamp01(volume);
        _primarySource.volume = globalVolume;
        
        // 保存音量设置
        PlayerPrefs.SetFloat("MusicVolume", globalVolume);
        PlayerPrefs.Save();
    }

    public void FadeOut(float duration)
    {
        StartCoroutine(FadeOutRoutine(duration));
    }

    private IEnumerator FadeOutRoutine(float duration)
    {
        float startVolume = _primarySource.volume;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            _primarySource.volume = Mathf.Lerp(startVolume, 0f, timer / duration);
            yield return null;
        }

        _primarySource.Stop();
    }
}