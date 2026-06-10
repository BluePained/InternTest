using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] background;
    [SerializeField] private SpriteRenderer[] extraSkyDay;
    [SerializeField] private SpriteRenderer[] extraSkyNight;
    
    [SerializeField] private Color32 morningColorSky;
    [SerializeField] private Color32 afternoonColorSky;
    [SerializeField] private Color32 eveningColorSky;
    
    [Header("Adjust Duration")]
    [Range(0,1)][SerializeField] private float durationAdjustment = 1f;
    
    private Coroutine _backgroundCoroutine;

#if UNITY_EDITOR
    
    private void OnValidate()
    {
        if (Application.isPlaying) return;
        
        FindComponent();
        
        EditorUtility.SetDirty(this);
    }
    
#endif
    

    private void Start()
    {
        TimeManager.Instance.OnTimeChanged += OnTimeChanged;
        ResetBackgroundComponent();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnResetScene;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnResetScene;
        TimeManager.Instance.OnTimeChanged -= OnTimeChanged;
    }

    private void OnTimeChanged(DayPeriod dayPeriod, float duration)
    {  
        if(_backgroundCoroutine != null) return;

       _backgroundCoroutine = StartCoroutine(TimeChange(dayPeriod, duration));
    }

    private IEnumerator TimeChange(DayPeriod dayPeriod, float duration)
    {
        float transitionDuration = duration * durationAdjustment;
        Color startColor = Color.white;
        Color endColor = Color.clear;
        
        Color backgroundStartColor = background[0].color;
        float timer = 0;

        while (timer < transitionDuration)
        {
            timer += Time.deltaTime;
            float elapsedTime = timer / transitionDuration;
            
            switch (dayPeriod)
            {
                case DayPeriod.Morning:
                    //Enable Cloud
                    //Disable Star
                    LerpColorTo(background,backgroundStartColor, morningColorSky,elapsedTime);
                    LerpColorTo(extraSkyDay,endColor, startColor, elapsedTime);
                    LerpColorTo(extraSkyNight,startColor, endColor, elapsedTime);
                
                    break;
                case DayPeriod.Afternoon:
                    LerpColorTo(background,backgroundStartColor, afternoonColorSky,elapsedTime);
                    
                    break;
                
                case DayPeriod.Evening:
                    //Enable Star
                    //Disable Cloud
                    LerpColorTo(background,backgroundStartColor, eveningColorSky,elapsedTime);
                    LerpColorTo(extraSkyDay,startColor, endColor, elapsedTime);
                    LerpColorTo(extraSkyNight,endColor, startColor, elapsedTime);
                    
                    break;
            }

            yield return null;
        }

        switch (dayPeriod)
        {
            case DayPeriod.Morning:
                SetColorTo(background, morningColorSky);
                SetColorTo(extraSkyDay, startColor);
                SetColorTo(extraSkyNight, endColor);
                break;
            case DayPeriod.Afternoon:
                SetColorTo(background, afternoonColorSky);
                break;
            case DayPeriod.Evening:
                SetColorTo(background, eveningColorSky);
                SetColorTo(extraSkyDay, endColor);
                SetColorTo(extraSkyNight, startColor);
                break;
        }
        
        _backgroundCoroutine = null;
    }

    private void SetColorTo(SpriteRenderer[] spriteRenderers, Color color)
    {
        foreach (var s in spriteRenderers)
        {
            s.color = color;
        }
    }

    private void LerpColorTo(SpriteRenderer[] spriteRenderers, Color startColor,Color endColor, float duration)
    {
        foreach (var s in spriteRenderers)
        {
            s.color = Color.Lerp(startColor, endColor, duration);
        }
    }
    
    private void OnResetScene(Scene scene, LoadSceneMode mode)
    {
        FindComponent();
        ResetBackgroundComponent();
    }

    private void FindComponent()
    {
        GameObject[] bg = GameObject.FindGameObjectsWithTag("Background");
        
        background = new SpriteRenderer[bg.Length];
        for (int i = 0; i < background.Length; i++)
        {
            background[i] = new SpriteRenderer();
            background[i] = bg[i].GetComponent<SpriteRenderer>();
        }
        
        GameObject[] exD = GameObject.FindGameObjectsWithTag("ExtraBackgroundDay");
        
        extraSkyDay = new SpriteRenderer[exD.Length];
        for (int i = 0; i < extraSkyDay.Length; i++)
        {
            extraSkyDay[i] = new SpriteRenderer();
            extraSkyDay[i] = exD[i].GetComponent<SpriteRenderer>();
        }
        
        GameObject[] exN = GameObject.FindGameObjectsWithTag("ExtraBackgroundNight");
        
        extraSkyNight = new SpriteRenderer[exN.Length];
        for (int i = 0; i < extraSkyNight.Length; i++)
        {
            extraSkyNight[i] = new SpriteRenderer();
            extraSkyNight[i] = exN[i].GetComponent<SpriteRenderer>();
        }
        
    }
    
    private void ResetBackgroundComponent()
    {
        switch (TimeManager.Instance.GetCurrentTime())
        {
            case DayPeriod.Morning:
                foreach (var e in extraSkyDay)
                {
                    e.color = Color.white;
                }

                foreach (var e in extraSkyNight)
                {
                    e.color = Color.clear;
                }
                
                foreach (var bg in background)
                {
                    bg.color = morningColorSky;
                }

                break;
            case DayPeriod.Afternoon:
                foreach (var e in extraSkyDay)
                {
                    e.color = Color.white;
                }

                foreach (var e in extraSkyNight)
                {
                    e.color = Color.clear;
                }
                
                foreach (var bg in background)
                {
                    bg.color = afternoonColorSky;
                }

                
                break;
            case DayPeriod.Evening:
                foreach (var e in extraSkyDay)
                {
                    e.color = Color.clear;
                }

                foreach (var e in extraSkyNight)
                {
                    e.color = Color.white;
                }
                
                foreach (var bg in background)
                {
                    bg.color = eveningColorSky;
                }

                break;
        }
    }
}
