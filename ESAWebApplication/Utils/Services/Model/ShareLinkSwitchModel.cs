
namespace ESAWebApplication.Utils.Services
{
    /// <summary>
    /// 匿名共享开关对象
    /// </summary>
    public class ShareLinkSwitchModel
    {
        /// <summary>
        /// 实名共享
        /// </summary>
        public bool enable_user_doc_inner_link_share { get; set; }
        /// <summary>
        /// 匿名共享
        /// </summary>
        public bool enable_user_doc_out_link_share { get; set; }
    }
}