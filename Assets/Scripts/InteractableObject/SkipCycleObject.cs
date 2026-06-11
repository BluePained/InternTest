using UnityEngine;

public class SkipCycleObject : ReactObject
{
    [SerializeField] private int cycle = 1;
    [SerializeField] private int day = 1;
    [SerializeField] private DayPeriod dayPeriod;

    protected override void React()
    {
        TimeManager.Instance.ChangeTimePeriodByCycle(cycle);
    }
}
