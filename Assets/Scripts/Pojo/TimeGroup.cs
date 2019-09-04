/// <summary>
/// 设定最大最小值，判断变量是否超出范围，查看比率
/// </summary>
public class TimeGroup
{
    public float use { get; set; }
    public float account { get; set; }
    private readonly float least = 0f;
    public float rate
    {
        get
        {
            return use / (account - least);
        }
    }
    public TimeGroup(float account)
    {
        this.use = 0f;
        this.account = account;
    }
    /// <summary>
    /// 增长变量，超出范围取边缘值，超出返回为false
    /// </summary>
    /// <param name="a"></param>
    /// <returns></returns>
    public bool Add(float a)
    {
        use += a;
        if (use < least)
        {
            use = least;
            return false;
        }
        if (use > account)
        {
            use = account;
            return false;
        }
        return true;
    }
    public void UseToZero()
    {
        use = 0f;
    }
}