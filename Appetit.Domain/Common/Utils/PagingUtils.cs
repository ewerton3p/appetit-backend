using Appetit.Domain.Common.Constants;

namespace Appetit.Domain.Common.Utils
{
    public static class PagingUtils
    {
        public static int GetPageOffset(int page)
        {
            return page * Paging.DefaultPageSize - Paging.DefaultPageSize;
        }
    }
}
