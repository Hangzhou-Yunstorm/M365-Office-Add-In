
namespace ESAWebApplication.Utils.Services
{
    /// <summary>
    /// 用户对象
    /// </summary>
    public class UserModel
    {
        public string userid { get; set; }
        public string account { get; set; }
        public string name { get; set; }
        public string mail { get; set; }
        public long csflevel { get; set; }
        public long leakproofvalue { get; set; }
        public long pwdcontrol { get; set; }
        public long usertype { get; set; }
        public long[] roletypes { get; set; }
        public Directdepinfo[] directdepinfos { get; set; }
        public bool needsecondauth { get; set; }
        public bool freezestatus { get; set; }
        public bool agreedtotermsofuse { get; set; }
        public bool ismanager { get; set; }
        public string telnumber { get; set; }
        public Roleinfo[] roleinfos { get; set; }
    }

    public class Directdepinfo
    {
        public string depid { get; set; }
        public string name { get; set; }
    }

    public class Roleinfo
    {
        public string id { get; set; }
        public string name { get; set; }
    }

}
