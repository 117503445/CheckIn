public enum CheckKind
{
    MorningRead = 6,
    MorningExercise = 9,
    MorningEye = 10,
    NoonSleep = 12,
    AfternoonEye = 15,
    NightStudy = 17,
    NightEye = 20,
    None = 37628,
}
public enum CheckType
{
    /// <summary>
    /// 在场
    /// </summary>
    Present,
    /// <summary>
    /// 缺席
    /// </summary>
    Absent,
    /// <summary>
    /// 请假
    /// </summary>
    Leave
}