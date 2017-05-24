using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourcesServer.Utils
{
    public class HeaderConstantsUtil
    {
        public const string PAGE_NO = "X-ConsincoAPI-Paging-PageNo";
        public const string PAGE_SIZE = "X-ConsincoAPI-Paging-PageSize";
        public const string PAGE_COUNT = "X-ConsincoAPI-Paging-PageCount";
        public const string PAGE_TOTAL = "X-ConsincoAPI-Paging-TotalRecordCount";

    }
}