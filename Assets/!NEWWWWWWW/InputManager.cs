using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    #region ScreenSystem
    public enum ScreenType
    {
        GameScreen,
        MachineInfoScreen,
        MenuScreen
    }
    public ScreenType ActiveScreenType = ScreenType.GameScreen;
    #endregion
    #region InputSystem
    [System.Serializable]
    public struct DictionaryPair
    {
        public string name;
        public KeyCode keyCode;
        public ScreenType screenType;
        [HideInInspector]public UnityEvent IsKeyPressed;
    }
    public DictionaryPair[] NormalKeyDictionarypPair;
    public Dictionary<string, DictionaryPair> NormalKeyDictionary = new Dictionary<string, DictionaryPair>();
    public DictionaryPair[] BeatKeyDictionarypPair;
    public Dictionary<string, DictionaryPair> BeatKeyDictionary = new Dictionary<string, DictionaryPair>();
    public float ExtraMouseWindowTime;
    [HideInInspector]public float LastKeyDownTime=-100;
    [HideInInspector]public KeyCode LastKeyCode;

    public UnityEvent GetNormalKeyEvent(string name)
    {
        return NormalKeyDictionary[name].IsKeyPressed;
    }
    public UnityEvent GetBeatKeyEvent(string name)
    {
        return BeatKeyDictionary[name].IsKeyPressed;
    }
    public bool GetBeatKeyUp(string name)
    {
        
        return Input.GetKeyUp(BeatKeyDictionary[name].keyCode);
    }
    public bool GetMouseInGame(int i)
    {
        return ActiveScreenType == ScreenType.GameScreen && Input.GetMouseButton(i);
    }
    public bool GetMouseDownInGame(int i)
    {
        return ActiveScreenType == ScreenType.GameScreen && Input.GetMouseButtonDown(i);
    }
    public bool GetMouseDownWithBeatInGame(int i)
    {
        if (!BeatManager.Instance.IsPlaying || BeatManager.Instance.IsRecording)
        {
            return false;
        }
        return ActiveScreenType == ScreenType.GameScreen && Input.GetMouseButtonDown(i) && BeatManager.Instance.IsInBeat();
    }
    public bool GetMouseUpInGame(int i)
    {
        return ActiveScreenType == ScreenType.GameScreen && Input.GetMouseButtonUp(i);
    }
    #endregion
    #region public
    private BeatManager beatManager;
    public static InputManager Instance { get; private set; }
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
        foreach (DictionaryPair pair in NormalKeyDictionarypPair)
        {
            NormalKeyDictionary[pair.name] = pair;
        }
        foreach (DictionaryPair pair in BeatKeyDictionarypPair)
        {
            BeatKeyDictionary[pair.name] = pair;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        beatManager =BeatManager.Instance;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.anyKeyDown)
        {
            foreach (DictionaryPair pair in NormalKeyDictionary.Values)
            {
                if (Input.GetKeyDown(pair.keyCode) && ActiveScreenType == pair.screenType)
                {
                    pair.IsKeyPressed.Invoke();
                }
            }
            if (true)
            {
                foreach (DictionaryPair pair in BeatKeyDictionary.Values)
                {
                    if (Input.GetKeyDown(pair.keyCode) && ActiveScreenType == pair.screenType)
                    {
                        //if (beatManager.IsInBeat())
                        //{
                        //    pair.IsKeyPressed.Invoke();
                        //    LastKeyDownTime = Time.time;
                        //    LastKeyCode = pair.keyCode;
                        //}
                        pair.IsKeyPressed.Invoke();
                        break;
                    }
                }
            }
        }
    }
    void LateUpdate()
    {
        if (!beatManager.IsRecording&& beatManager.IsPlaying)
        {
            beatManager.JudgeNextBeat();
        }
    }
    #endregion
}