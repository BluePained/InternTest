using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ClockManager : MonoBehaviour
{
    [SerializeField] private Image periodImage;
    [SerializeField] private Image periodMeter;
    [SerializeField] private TMP_Text dayText;
    [SerializeField] private TMP_Text weekDayText;
    
    [Header("Time Icons")]
    [SerializeField] private Sprite[] timeIcons;
    
    [Header("Time Meter Value")]
    [SerializeField] private float[] timeMeterValue;
    
    [Header("Adjust Duration")]
    [Range(0,1)][SerializeField] private float durationAdjustment = 1f;
    
    private Coroutine _meterCoroutine;
    
#if UNITY_EDITOR
    
    private void OnValidate()
    {
        if (Application.isPlaying) return;
        
        int e = Enum.GetNames(typeof(DayPeriod)).Length;
        if (timeIcons.Length != e)
        {
            timeIcons = new Sprite[Enum.GetNames(typeof(DayPeriod)).Length];
        }
        EditorUtility.SetDirty(this);
    }
    
#endif
    
    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        TimeManager.Instance.OnTimeChanged -= OnUpdatePeriodMeter;
        TimeManager.Instance.OnFinishTimeChanged -= UpdateClock;
    }
    
    private void Start()
    {
        TimeManager.Instance.OnTimeChanged += OnUpdatePeriodMeter;
        TimeManager.Instance.OnFinishTimeChanged += UpdateClock;

        switch (TimeManager.Instance.GetCurrentTime())
        {
            case DayPeriod.Morning:
                periodImage.sprite = timeIcons[0];
                periodMeter.fillAmount = timeMeterValue[0];
                break;
            case DayPeriod.Afternoon:
                periodImage.sprite = timeIcons[1];
                periodMeter.fillAmount = timeMeterValue[1];
                break;
            case DayPeriod.Evening:
                periodImage.sprite = timeIcons[2];
                periodMeter.fillAmount = timeMeterValue[2];
                break;
        }
    }

    private void UpdateClock(WeekDay weekDay, DayPeriod dayPeriod,int day)
    {
        if (timeIcons[(int)dayPeriod] != null)
        {
            periodImage.sprite = timeIcons[(int)dayPeriod];
        }
        
        dayText.text = $"Day {day}";
        weekDayText.text = weekDay.ToString();
    }

    private void OnUpdatePeriodMeter(DayPeriod dayPeriod, float duration)
    {
        if (_meterCoroutine != null) return;

        _meterCoroutine = StartCoroutine(UpdatePeriodMeter(dayPeriod, duration));
    }

    private IEnumerator UpdatePeriodMeter(DayPeriod dayPeriod, float duration)
    {
        float transitionDuration = duration * durationAdjustment;
        float startValue = periodMeter.fillAmount;
        float endValue = timeMeterValue[(int)dayPeriod];
        float timer = 0f;

        while (timer < transitionDuration)
        {
            timer += Time.deltaTime;
            float elapsedTime = timer / transitionDuration;
            
            periodMeter.fillAmount = Mathf.Lerp(startValue, endValue, elapsedTime);
            
            yield return null;
        }
        
        periodMeter.fillAmount = endValue;
        
        _meterCoroutine = null;
        yield return null;
    }
}
