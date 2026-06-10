using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "TimePeriodSO", menuName = "TimePeriodSO")]
public class TimePeriodSO : ScriptableObject
{
    [field: SerializeField] public string Name { get; set; }
    [field: SerializeField] public Color32 SkyLightColor { get; private set; }
}
