public static class FormatterUtil
{
    public static string GetSizeString(float size)
    {
        const float gb = 1024 * 1024 * 1024, mb = 1024 * 1024, kb = 1024;
        string rs;
        if (size > gb)
        {
            rs = (size / gb).ToString("f2") + "GB";
        }
        else if (size > mb)
        {
            rs = (size / mb).ToString("f2") + "MB";
        }
        else if (size > kb)
        {
            rs = (size / kb).ToString("f2") + "KB";
        }
        else
        {
            rs = size.ToString("f2") + "B";
        }
        return rs;
    }
}