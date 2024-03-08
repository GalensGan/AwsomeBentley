using Bentley.DgnPlatformNET;
using Bentley.DgnPlatformNET.Elements;
using Bentley.GeometryNET;
using Bentley.MstnPlatformNET;
using CSharpMicrostation.Core.Helpers;
using log4net;
using MSAddinTest.MSTestInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpMicrostation.Threads
{
    /// <summary>
    /// 用于测试 Microstation 的多线程操作
    /// </summary>
    internal class Tests : IMSTest_StaticMethod
    {
        private static ILog _logger = LogManager.GetLogger(typeof(Tests));

        /// <summary>
        /// 经测试，多线程操作会导致程序崩溃
        /// </summary>
        /// <param name="unparsed"></param>
        [MSTest("TestThreadOpteration")]
        public static void TestThreadOpteration(string unparsed)
        {
            // 在另外的线程中查找元素、生成元素
            Task.Run(() =>
            {
                List<DPoint3d> pnts = new List<DPoint3d>()
                {
                    DPoint3d.Zero,
                    DPoint3d.Zero + DVector3d.UnitZ*10000
                };
                CurveVector cv = CurveVector.CreateLinear(pnts, CurveVector.BoundaryType.Open, true);

                DgnModel dgnm = Session.Instance.GetActiveDgnModel();

                Element element = DraftingElementSchema.ToElement(dgnm, cv, null);
                element.AddToModel();
               
                // 查找元素
                var element2 = dgnm.FindElementById(element.ElementId);
                if (element2 == null)
                {
                    _logger.Error($"未找到元素 {element.ElementId}");
                    return;
                }
            });
        }
    }
}
