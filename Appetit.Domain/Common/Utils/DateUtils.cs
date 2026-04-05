namespace Appetit.Domain.Common.Utils
{
    public class DateUtils
    {
        public static DateTimeOffset GetCurrentUtcDateTime()
        {
            return DateTimeOffset.UtcNow;
        }
    }
}
