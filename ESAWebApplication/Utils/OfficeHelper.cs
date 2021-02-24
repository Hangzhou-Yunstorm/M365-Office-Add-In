using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Excel = Microsoft.Office.Interop.Excel;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;
using Word = Microsoft.Office.Interop.Word;

namespace ESAWebApplication.Utils
{
    /// <summary>
    /// Office帮助类
    /// </summary>
    public class OfficeHelper
    {
        /// <summary>
        /// log4net
        /// </summary>
        static log4net.ILog _log = log4net.LogManager.GetLogger("OfficeHelper");

        /// <summary>
        /// 比较文件
        /// </summary>
        /// <param name="filePath1">文件路径1</param>
        /// <param name="filePath2">文件路径2</param>
        /// <param name="savePath">文件保存路径</param>
        public static string CompareFile(string filePath1, string filePath2, string savePath)
        {
            Word.Application wordApp = null;
            Word.Document doc = null;
            Word.Document doc1 = null;
            Word.Document doc2 = null;

            try
            {
                _log.Debug($"OfficeHelper CompareFile Start.");
                wordApp = new Word.Application();
                wordApp.Visible = false;
                object wordTrue = (object)true;
                object wordFalse = (object)false;
                object missing = Type.Missing;
                object document1 = filePath1;
                object document2 = filePath2;

                // 对比文件1
                doc1 = wordApp.Documents.Open(ref document1, ref missing, ref wordFalse, ref wordFalse, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref wordTrue, ref missing, ref missing, ref missing, ref missing);
                // 对比文件2
                doc2 = wordApp.Documents.Open(ref document2, ref missing, ref wordFalse, ref wordFalse, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);

                // 清除文件备注
                var oDocBuiltInProps = doc2.BuiltInDocumentProperties;
                Type typeDocBuiltInProps = oDocBuiltInProps.GetType();
                //Set the Comments property.
                typeDocBuiltInProps.InvokeMember("Item", BindingFlags.Default | BindingFlags.SetProperty, null, oDocBuiltInProps, new object[] { "Comments", "" });
                doc2.Save();

                // 对比后文件
                doc = wordApp.CompareDocuments(doc1, doc2, Word.WdCompareDestination.wdCompareDestinationNew, Word.WdGranularity.wdGranularityWordLevel, true, true, true, true, true, true, true, true, true, true, "", false);

                _log.Debug("OfficeHelper CompareFile Save File Start");
                // 保存对比文件到本地
                object format = Word.WdSaveFormat.wdFormatDocumentDefault; //Word文档的保存格式  
                object documentCompare = $"{savePath}\\{doc.Name}";
                doc.SaveAs2(documentCompare, format);
                _log.Debug("OfficeHelper CompareFile Save File End");

                // 对比文件路径
                var cFilePath = $"{documentCompare}.docx";
                return cFilePath;
            }
            catch (Exception ex)
            {
                _log.Debug($"OfficeHelper CompareFile Exception: {ex.Message}");
                throw ex;
            }
            finally
            {
                KillProcessesTask("WINWORD");

                try
                {
                    if (doc != null)
                    {
                        doc.Close();
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(doc);
                    }
                }
                catch (Exception ex)
                {
                    doc = null;
                    _log.Debug($"OfficeHelper CompareFile DocClose Finally Exception: {ex.Message}");
                }
                try
                {
                    if (doc1 != null)
                    {
                        doc1.Close();
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(doc1);
                    }
                }
                catch (Exception ex)
                {
                    doc1 = null;
                    _log.Debug($"OfficeHelper CompareFile Doc1Close Finally Exception: {ex.Message}");
                }
                try
                {
                    if (doc2 != null)
                    {
                        doc2.Close();
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(doc2);
                    }
                }
                catch (Exception ex)
                {
                    doc2 = null;
                    _log.Debug($"OfficeHelper CompareFile Doc2Close Finally Exception: {ex.Message}");
                }
                try
                {
                    wordApp.Quit();
                }
                catch (Exception ex)
                {
                    wordApp = null;
                    _log.Debug($"OfficeHelper CompareFile AppClose Finally Exception: {ex.Message}");
                }
                _log.Debug($"OfficeHelper CompareFile End.");
            }
        }

        /// <summary>
        /// 设置Word文件属性
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="fileId">文件Id</param>
        public static void SetWordFileID(ref string filePath, string fileId)
        {
            Word.Application application = null;
            Word.Document doc = null;
            try
            {
                _log.Debug($"SetWordFileID Start.");

                application = new Word.Application();
                application.Visible = false;
                object documentPath = filePath;

                // 设置文件
                doc = application.Documents.Open(ref documentPath);
                var oDocBuiltInProps = doc.BuiltInDocumentProperties;
                Type typeDocBuiltInProps = oDocBuiltInProps.GetType();

                //Set the Comments property.
                typeDocBuiltInProps.InvokeMember("Item", BindingFlags.Default | BindingFlags.SetProperty, null, oDocBuiltInProps, new object[] { "Comments", fileId });
                doc.Save();

                var dirPath = Path.GetDirectoryName(filePath);
                var ext = Path.GetExtension(filePath);
                filePath = $"{dirPath}\\{Guid.NewGuid()}{ext}";
                doc.SaveAs2(filePath);
            }
            catch (Exception ex)
            {
                _log.Debug($"SetWordFileID Exception: {ex.Message}");
            }
            finally
            {
                KillProcessesTask("WINWORD");

                try
                {
                    if (doc != null)
                    {
                        doc.Close();
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(doc);
                    }
                }
                catch (Exception ex)
                {
                    doc = null;
                    _log.Debug($"SetWordFileID DocClose Finally Exception: {ex.Message}");
                }
                try
                {
                    application.Quit();
                }
                catch (Exception ex)
                {
                    application = null;
                    _log.Debug($"SetWordFileID AppClose Finally Exception: {ex.Message}");
                }
                _log.Debug($"SetWordFileID End.");
            }
        }

        /// <summary>
        /// 设置Excel文件属性
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="fileId">文件Id</param>
        public static void SetExcelFileID(string filePath, string fileId)
        {
            Excel.Application application = null;
            Excel.Workbook workBook = null;
            try
            {
                _log.Debug($"SetExcelFileID Start.");

                application = new Excel.Application();
                application.Visible = false;

                // 设置文件
                workBook = application.Workbooks.Open(filePath);
                workBook.Comments = fileId;
                workBook.Save();
            }
            catch (Exception ex)
            {
                _log.Debug($"SetExcelFileID Exception: {ex.Message}");
            }
            finally
            {
                KillProcessesTask("EXCEL");

                try
                {
                    if (workBook != null)
                    {
                        workBook.Close(true, filePath, true);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(workBook);
                    }
                }
                catch (Exception ex)
                {
                    workBook = null;
                    _log.Debug($"SetExcelFileID DocClose Finally Exception: {ex.Message}");
                }
                try
                {
                    application.Quit();
                }
                catch (Exception ex)
                {
                    application = null;
                    _log.Debug($"SetExcelFileID AppClose Finally Exception: {ex.Message}");
                }
                _log.Debug($"SetExcelFileID End.");
            }
        }

        /// <summary>
        /// 设置PPT文件属性
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="fileId">文件Id</param>
        public static void SetPPTFileID(string filePath, string fileId)
        {
            PowerPoint.Application application = null;
            PowerPoint.Presentation presentation = null;
            try
            {
                _log.Debug($"SetPPTFileID Start.");
                application = new PowerPoint.Application();

                // 设置文件
                presentation = application.Presentations.Open(filePath, WithWindow: Microsoft.Office.Core.MsoTriState.msoFalse);
                var oDocBuiltInProps = presentation.BuiltInDocumentProperties;
                Type typeDocBuiltInProps = oDocBuiltInProps.GetType();

                //Set the Comments property.
                typeDocBuiltInProps.InvokeMember("Item", BindingFlags.Default | BindingFlags.SetProperty, null, oDocBuiltInProps, new object[] { "Comments", fileId });

                presentation.Save();
            }
            catch (Exception ex)
            {
                _log.Debug($"SetPPTFileID Exception: {ex.Message}");
            }
            finally
            {
                KillProcessesTask("POWERPOINT");

                try
                {
                    if (presentation != null)
                    {
                        presentation.Close();
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(presentation);
                    }
                }
                catch (Exception ex)
                {
                    presentation = null;
                    _log.Debug($"SetPPTFileID DocClose Finally Exception: {ex.Message}");
                }
                try
                {
                    application.Quit();
                }
                catch (Exception ex)
                {
                    application = null;
                    _log.Debug($"SetPPTFileID AppClose Finally Exception: {ex.Message}");
                }
                _log.Debug($"SetPPTFileID End.");
            }
        }

        /// <summary>
        /// 关闭进程任务
        /// </summary>
        /// <param name="pName">进程名</param>
        private static void KillProcessesTask(string pName)
        {
            try
            {
                System.Threading.Tasks.Task.Run(() =>
                {
                    KillProcesses(pName);
                });
            }
            catch (Exception ex)
            {
                _log.Debug($"KillProcessesTask {pName}  Exception: {ex.Message}");
            }
        }

        /// <summary>
        /// 关闭进程
        /// </summary>
        /// <param name="pName">进程名</param>
        private static void KillProcesses(string pName)
        {
            try
            {
                Process[] localByNameApp = Process.GetProcessesByName(pName);
                if (localByNameApp.Length > 0)
                {
                    foreach (var app in localByNameApp)
                    {
                        // 关闭6小时以上的未关闭进程
                        if (!app.HasExited && (DateTime.Now - app.StartTime).TotalHours > 6)
                        {
                            //关闭进程  
                            app.Kill();
                        }
                    }
                }
                GC.Collect();
            }
            catch (Exception ex)
            {
                _log.Debug($"KillProcesses {pName}  Exception: {ex.Message}");
            }
        }
    }
}