using System.Collections.Generic;

namespace ESAWebApplication.Models
{
    /// <summary>
    /// 搜索结果对象
    /// </summary>
    public class SearchModel
    {
        public long start { get; set; }
        public long rows { get; set; }
        public string keys { get; set; }
        public List<string> range { get; set; }
        public List<string> keysfields { get; set; }
        /// <summary>
        /// 为1，查找文件
        /// 为2，查找文件夹 
        /// 为3，查找文件&文件夹 默认3
        /// </summary>
        public long doctype { get; set; }
        public bool hl { get; set; }
    }
}