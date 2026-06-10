using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;
    
    [SerializeField] private WeekDay dayOfTheWeek = WeekDay.Monday;
    [SerializeField] private DayPeriod dayPeriod = DayPeriod.Morning;
    [SerializeField] private int currentDay = 0;
    [SerializeField] private TimePeriodSO currentSkyPeriod;
    [SerializeField] private Light2D skyLight;
    [SerializeField] private float timeTransitionDuration = 1f;
    
    [SerializeField] private TimePeriodSO[] cyclePeriods;
    
    public event Action<DayPeriod> OnTimeChanged;
    public event Action<WeekDay, DayPeriod, int> OnFinishTimeChanged;
    
    private Coroutine _updatePeriodRoutine;
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnResetScene;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnResetScene;
    }
    
    private void Start()
    {
        FindComponent();
    }
    
    public DayPeriod GetCurrentTime()
    {
        return dayPeriod;
    }

    public float GetTransitionDuration()
    {
        return timeTransitionDuration;
    }
    
    public void UpdateCyclePeriod(TimePeriodSO[] period)
    {
        cyclePeriods = period;
    }
    
    /// <summary>
    /// Change the time by cycle. Skip the cycle by the input cycle int, will also skip the day if it passed evening.
    /// </summary>
    /// <param name="cycle">How many cycle should it skip.</param>
    public void ChangeTimePeriodByCycle(int cycle)
    {
        if (_updatePeriodRoutine != null) return;
        
        _updatePeriodRoutine = StartCoroutine(UpdateTimePeriod(cycle));
    }

    /// <summary>
    /// Change the time by day. 
    /// </summary>
    /// <param name="day">How many day should it skip.</param>
    /// <param name="period">Which period of time it should be at the last cycle.</param>
    public void ChangeTimePeriodByDay(int day, DayPeriod period)
    {
       /* if (_updatePeriodRoutine != null)
        {
            StopCoroutine(_updatePeriodRoutine);
        }
        
        _updatePeriodRoutine = StartCoroutine(UpdateTimePeriod(day));*/
    }
    

    private IEnumerator UpdateTimePeriod(int cycle)
    {
        TimePeriodSO[] tempPeriods = cyclePeriods;
    
        if (cycle <= 0)
            cycle = 1;
        
        for (int i = 0; i < cycle; i++)
        {
            Color32 startSkyColor = skyLight.color;
        
            int nextPeriod = (int)dayPeriod + 1;
            if (nextPeriod >= cyclePeriods.Length)
            {
                nextPeriod = 0;
            }
            
            Color32 endSkyColor = tempPeriods[nextPeriod].SkyLightColor;
            OnTimeChanged?.Invoke((DayPeriod)nextPeriod);
            float timer = 0f;
            while (timer < timeTransitionDuration)
            {
                timer += Time.deltaTime;
                float elapsedTime = timer / timeTransitionDuration;
                
                skyLight.color = Color.Lerp(startSkyColor, endSkyColor, elapsedTime);
                
                yield return null;
            }
        
            //skyTransform.localRotation = Quaternion.Euler(0,0, endSkyRotation.z);
            skyLight.color = endSkyColor;
        
            if (dayPeriod == DayPeriod.Evening && nextPeriod == 0)
            {
                int nextDay = (int)dayOfTheWeek + 1;

                if (nextDay >= Enum.GetValues(typeof(WeekDay)).Length)
                {
                    nextDay = 0;
                }

                dayOfTheWeek = (WeekDay)nextDay;
                currentDay++;
            }
        
            dayPeriod = (DayPeriod)nextPeriod;
            OnFinishTimeChanged?.Invoke(dayOfTheWeek, dayPeriod, currentDay);       
        }
        
        _updatePeriodRoutine = null;
    }

    private void OnResetScene(Scene scene, LoadSceneMode mode)
    {
        FindComponent();
    }

    private void FindComponent()
    {
        skyLight = GameObject.FindGameObjectWithTag("SkyLight").GetComponent<Light2D>();
        dayOfTheWeek = WeekDay.Monday;
        dayPeriod = DayPeriod.Morning;
        currentDay = 0;

        if (currentSkyPeriod == null || skyLight == null) return;
        skyLight.color = currentSkyPeriod.SkyLightColor;
    }
}
