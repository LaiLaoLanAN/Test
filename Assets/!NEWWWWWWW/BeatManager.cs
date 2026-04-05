using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class BeatManager : MonoBehaviour
{
    public bool IsRecording=false;
    private InputManager inputManager;
    [HideInInspector]public bool IsPlaying=false;
    private float StartTime;
    public BeatData beatData;
    public AudioSource audioSource;
    public List<AudioClip> audioClip;

    private MusicManager musicManager;
    public static BeatManager Instance { get; private set; }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        inputManager = InputManager.Instance;
        musicManager = MusicManager.Instance;
        inputManager.GetNormalKeyEvent("Play").AddListener(PlayMusic);
    }
    private void OnDestroy()
    {
        inputManager.GetNormalKeyEvent("Play").RemoveListener(PlayMusic);
    }
    // Update is called once per frame
    void Update()
    {

    }
    private void PlayMusic()
    {
        if (!IsPlaying)
        {
            IsPlaying = true;
            musicManager.PlayTrack("彻夜", true);
            if (IsRecording)
            {
                StartCoroutine(Record());
            }
            else
            {
                StartCoroutine(Play());
            }
        }
    } 
    IEnumerator Record()
    {
        beatData.beats.Clear();
        beatData.TotalBeatNum = 0;
        while (true)
        {
            if (inputManager.GetMouseDownInGame(0))
            {
                beatData.beats.Add(musicManager._primarySource.time);
                Debug.Log(musicManager._primarySource.time);
                beatData.TotalBeatNum++;
            }
            yield return null;
        }
    }
    IEnumerator Play()
    {
        //BeatStartTime= Time.time - MusicManager.Instance._primarySource.time;
        CurrentBeatNum=0;
        //while (true)
        //{
        //    if (inputManager.GetKeyDownWithBeat("Jump"))
        //    {
        //        Debug.Log("Jump!");
        //    }
        //    yield return null;
        //}
        yield return null;
    }


    //public float BeatStartTime;
    [HideInInspector]public int CurrentBeatNum;
    [HideInInspector] public Dictionary<int,bool> BeatCatched=new Dictionary<int, bool>();
    public WorldNumber worldNumber;
    [HideInInspector]public int ContinueBeat=0;
    public bool IsInBeat()
    {
        if (CurrentBeatNum != 0)
        {
            if (musicManager._primarySource.time < beatData.beats[CurrentBeatNum - 1] - beatData.BeatWindowTime / 2)
            {
                return false;
            }
        }
        bool IsBeatsCatch = musicManager._primarySource.time < beatData.beats[CurrentBeatNum] + beatData.BeatWindowTime / 2 && musicManager._primarySource.time > beatData.beats[CurrentBeatNum] - beatData.BeatWindowTime / 2;
        BeatAdd(IsBeatsCatch);
        //if (IsBeatsCatch)
        //{
        //    audioSource.clip = audioClip[Random.Range(0, audioClip.Count)];
        //    audioSource.Play();
        //}
        return IsBeatsCatch;
    }
    public bool IsInBeatOnlyRead()
    {
        return musicManager._primarySource.time < beatData.beats[CurrentBeatNum] + beatData.BeatWindowTime / 2 && musicManager._primarySource.time > beatData.beats[CurrentBeatNum] - beatData.BeatWindowTime / 2;
    }
    public void JudgeNextBeat()
    {
        if (musicManager._primarySource.time > beatData.beats[CurrentBeatNum] + beatData.BeatWindowTime / 2)
        {
            BeatAdd(false);
        }
    }
    private void BeatAdd(bool IsCatched)
    {
        BeatCatched[CurrentBeatNum] = IsCatched;
        if (IsCatched)
        {
            ContinueBeat++;
            worldNumber.ChangeWorldNumber(ContinueBeat);
        }
        else
        {
            if (ContinueBeat != 0)
            {
                ContinueBeat = 0;
                worldNumber.ChangeWorldNumber(ContinueBeat);
            }
        }
        if (CurrentBeatNum < beatData.TotalBeatNum - 2)
        {
            CurrentBeatNum++;
        }
    }
    public float GetFillAmount()
    {
        if (musicManager._primarySource.time > beatData.beats[CurrentBeatNum])
        {
            return 1;
        }
        if (CurrentBeatNum != 0)
        {
            return (beatData.beats[CurrentBeatNum] - (musicManager._primarySource.time)) / (beatData.beats[CurrentBeatNum] - beatData.beats[CurrentBeatNum - 1]);
        }
        return (beatData.beats[CurrentBeatNum] - (musicManager._primarySource.time)) / (beatData.beats[CurrentBeatNum]);
    }
    public bool IsMissed(int CurrentI)
    {
        return musicManager._primarySource.time > beatData.beats[CurrentI] + beatData.BeatWindowTime / 2;
    }
    public float NotePos(int CurrentI)
    {
        return (beatData.beats[CurrentI] - musicManager._primarySource.time);
    }
    public float BeatAppearTime(int CurrentI)
    {
        return beatData.beats[CurrentI] - musicManager._primarySource.time;
    }
}
