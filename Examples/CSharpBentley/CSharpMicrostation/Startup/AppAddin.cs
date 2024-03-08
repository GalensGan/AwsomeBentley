using System;
using Bentley.DgnPlatformNET;
using Bentley.MstnPlatformNET;
using MSAddinTest.MSTestInterface;

namespace CSharpMicrostation.Startup
{
    /// <summary>
    /// Addin 定义
    /// 该类是自定义 addin 与 mstn 交互的入口
    /// </summary>
    [AddIn(MdlTaskID = "CSharpBentley")]
    public class AppAddin : MSTest_Addin
    // AddIn
    {
        public AppAddin(IntPtr mdlDescriptor) : base(mdlDescriptor)
        {
        }

        /// <summary>
        /// 该方法在加载 addin 时被调用
        /// 可以旋转一些初始化的代码在里面
        /// </summary>
        /// <param name="commandLine"></param>
        /// <returns></returns>
        protected override int Run(string[] commandLine)
        {
            return StatusInt.Success;
        }
    }
}
