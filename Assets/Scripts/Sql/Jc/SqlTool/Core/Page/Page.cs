using System.Collections.Generic;
using Jc.ToStringTool;
namespace Jc.SqlTool.Core.Page
{
    public class Page<T> : AbstractToStringObject
    {
        /// <summary>
        /// 第几页，从0开始计数
        /// </summary>
        /// <value></value>
        public int StartIndex { get; set; }
        /// <summary>
        /// 每页长度
        /// </summary>
        /// <value></value>
        public int PageSize { get; set; }
        /// <summary>
        /// 数据表的全部数量
        /// </summary>
        /// <value></value>
        public int Count { get; set; }
        /// <summary>
        /// 数据集
        /// </summary>
        /// <value></value>
        public List<T> Data { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        /// <returns></returns>
        public int PageCount { get { return (int)(Count * 1.0f / PageSize + 0.5f); } }
        /// <summary>
        /// 是否为最后一页
        /// 由于startIndex从0开始计数，PageCount最小为1，需要+1
        /// </summary>
        /// <value></value>
        public bool IsEnd { get { return StartIndex + 1 >= PageCount; } }
        /// <summary>
        /// 是否为首页
        /// </summary>
        /// <value></value>
        public bool IsFirst { get { return StartIndex == 0; } }
        public Page(int startIndex, int pageSize)
        {
            this.StartIndex = startIndex;
            this.PageSize = pageSize;
        }
        public Page()
        {

        }
    }
}