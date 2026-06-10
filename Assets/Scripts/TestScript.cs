using UnityEngine;
using UnityEngine.InputSystem;

public class TestScript : MonoBehaviour
{
    [SerializeField] private int cycle = 1;
    [SerializeField] private int day = 1;
    [SerializeField] private DayPeriod dayPeriod;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            TimeManager.Instance.ChangeTimePeriodByCycle(cycle);
        }

        if (Keyboard.current.vKey.wasPressedThisFrame)
        {
            TimeManager.Instance.ChangeTimePeriodByDay(day, dayPeriod);
        }
    }
}
