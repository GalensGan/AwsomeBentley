using MSAddinTest.MSTestInterface;

namespace CSharpMicrostation.Startup
{
    /// <summary>
    /// Keyin 命令调用的函数定义
    /// 命令会直接从此处开始响应
    /// </summary>
    public class KeyinFunctions
    {
        /// <summary>
        /// 每个 keyin 对应的函数
        /// </summary>
        /// <param name="unparsed"></param>
        public static void Func(string unparsed)
        {
            // 此处为 keyin 命令的响应函数
        }
    }
}
