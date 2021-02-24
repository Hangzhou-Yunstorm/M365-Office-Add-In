using System.Collections.Generic;

namespace ESAWebApplication.Utils.Services
{
    /// <summary>
    /// 搜索结果
    /// </summary>
    public class SearchResult
    {
        public Response response { get; set; }
        public List<List<string>> similardocs { get; set; }
    }

    public class _source
    {
        public string basename { get; set; }
        public long created { get; set; }
        public string creator { get; set; }
        public long csflevel { get; set; }
        public string docid { get; set; }
        public string editor { get; set; }
        public string ext { get; set; }
        public long modified { get; set; }
        public string parentpath { get; set; }
        public long size { get; set; }
        public double distance { get; set; }
        public string summary { get; set; }
        public List<string> tags { get; set; }
    }

    public class Position
    {
        public long bottom { get; set; }
        public long left { get; set; }
        public long right { get; set; }
        public long top { get; set; }
    }

    public class Highlight
    {
        public List<string> basename { get; set; }
        public List<string> content { get; set; }
        public Position position { get; set; }
    }

    public class DocsItem
    {
        public double _score { get; set; }
        public _source _source { get; set; }
        public Highlight highlight { get; set; }
    }

    public class Response
    {
        public List<DocsItem> docs { get; set; }
        public long hits { get; set; }
        public long next { get; set; }
    }
}