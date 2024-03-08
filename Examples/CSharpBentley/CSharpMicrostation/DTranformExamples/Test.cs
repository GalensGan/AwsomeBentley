using Bentley.GeometryNET;
using log4net;
using log4net.Config;
using MSAddinTest.MSTestInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace CSharpMicrostation.DTranformExamples
{
    /// <summary>
    /// 测试调用入口
    /// </summary>
    internal class Test : IMSTest_StaticMethod
    {
        private static ILog _logger = LogManager.GetLogger(typeof(Test));

        /// <summary>
        /// 坐标变换测试
        /// </summary>
        /// <param name="unparsed"></param>
        [MSTest("CorrdinateTranslation")]
        public static void TestCorrdinateTranslation(string unparsed)
        {
            // 生成测试数据
            var originA = DPoint3d.Zero;
            var originB = DPoint3d.UnitZ;
            DSegment3d segment = new DSegment3d(originA, originB);
            CurvePrimitive line1 = CurvePrimitive.CreateLine(segment);

            // 世界坐标转局部坐标
            // 原坐标：(0,0,0)->(0,0,1)
            // 现坐标：以(1,1,1)为原点，X轴为(0,0,-1)，Y轴为(0,1,0)，Z轴为(1,0,0)的坐标系
            // 变换后，line 的坐标应为(1,1,1)和(1,1,0)
            DTransform3d local1 = DTransform3d.FromOriginAndColumnPoints(new DPoint3d(1, 1, 1),
                -DVector3d.UnitZ, DVector3d.UnitY, DVector3d.UnitX);
            line1.Transform(local1);
            // 判断结果
            line1.GetStartEnd(out var pa1, out var pb1);

            _logger.Info($"世界坐标转局部: {pa1.Equals(pa1)} origin line: {originA} - {originB}; transformed line: {pa1}-{pb1}");
        }
    }
}
