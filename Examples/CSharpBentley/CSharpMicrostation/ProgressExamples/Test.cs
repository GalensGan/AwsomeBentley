using Bentley.MstnPlatformNET;
using CSharpMicrostation.Core.Logs;
using CSharpMicrostation.ProgressExamples.FluentProgress;
using log4net;
using MSAddinTest.MSTestInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace CSharpMicrostation.ProgressExamples
{
    /// <summary>
    /// 测试入口
    /// </summary>
    internal class Test : IMSTest_StaticMethod
    {
        [MSTest("TestMSProgress")]
        public static void TestMSProgress(string unparsed)
        {
            var progress = new MSProgress("test");
            progress.Run(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    var message = $"当前进度: {i}";
                    progress.Update(message, i + 1);
                    Thread.Sleep(50);
                }
            });
            progress.Close();
        }

        [MSTest("TestThreadProgress")]
        public static void TestThreadProgress(string unparsed)
        {
            var startDate = DateTime.Now;
            var progress = new ThreadProgress("test");
            progress.Run(()=>
            {
                var sameNameProgress = new ThreadProgress("test");
                for (int i = 0; i < 100; i++)
                {
                    var message = $"当前进度: {i}";
                    Logger.Log(message);
                    if (i % 2 == 0) progress.Update(message, i + 1);
                    else sameNameProgress.Update(message, i + 1);

                    Thread.Sleep(50);
                }
            });

            var endDate = DateTime.Now;
            Logger.Log($"耗时：{endDate - startDate}", "");
        }
    }
}
