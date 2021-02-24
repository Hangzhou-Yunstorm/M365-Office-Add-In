using ESAWebApplication.Models;
using System.Collections.Generic;

namespace ESAWebApplication.Utils.Services
{
    public interface IShareLinkAPIHelper
    {
        /// <summary>
        /// 创建匿名共享
        /// </summary>
        /// <returns>匿名共享</returns>
        string CreatShareLink(string token, CreatShareLinkModel model, out long errorCode);

        /// <summary>
        /// 修改匿名共享
        /// </summary>
        /// <returns>匿名共享</returns>
        long UpdateShareLink(string token, UpdateShareLinkModel model);

        /// <summary>
        /// 删除匿名共享
        /// </summary>
        /// <returns>匿名共享</returns>
        long DeleteShareLink(string token, string link_id);

        /// <summary>
        ///  获取匿名共享
        /// </summary>
        /// <param name="item">ShareLinkItem</param>
        /// <returns>匿名共享集合</returns>
        List<ShareLinkModel> GetShareLink(string token, ShareLinkItem item, out long errorCode);

        /// <summary>
        ///  获取实名共享
        /// </summary>
        /// <param name="item">ShareLinkItem</param>
        /// <returns>实名共享ID</returns>
        string GetRealNameShareLink(string token, ShareLinkItem item, out long errorCode);

        /// <summary>
        /// 创建实名共享
        /// </summary>
        /// <returns>实名共享</returns>
        string CreatRealNameShareLink(string token, CreatRealNameShareLinkModel model, out long errorCode);

        /// <summary>
        /// 获取匿名共享模板
        /// </summary>
        /// <returns>匿名共享模板</returns>
        ShareLinkConfigModel GetShareLinkConfig(string token, out long errorCode);

        /// <summary>
        /// 获取共享开关
        /// </summary>
        /// <returns>共享开关</returns>
        ShareLinkSwitchModel GetShareLinkSwitch(string token, out long errorCode);

        /// <summary>
        /// 检查是否是所有者
        /// </summary>
        /// <returns>是否是所有者</returns>
        CheckOwnerModel CheckOwner(string token, string gnsId, out long errorCode);

        /// <summary>
        /// 获取文件夹大小
        /// </summary>
        /// <returns>文件夹大小</returns>
        DirSizeModel GetDirSize(string token, string gnsId, out long errorCode);
    }
}