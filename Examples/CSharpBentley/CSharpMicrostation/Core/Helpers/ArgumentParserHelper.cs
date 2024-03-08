using Bentley.DgnPlatformNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpMicrostation.Core.Helpers
{
    /// <summary>
    /// 参数解析帮助类
    /// </summary>
    public class ArgumentParserHelper
    {
        /// <summary>
        /// 转换成 ElementId
        /// </summary>
        /// <param name="unparsed"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static ElementId ParseElementId(string unparsed)
        {
            if(string.IsNullOrEmpty(unparsed))throw new ArgumentNullException(nameof(unparsed));
            if (!long.TryParse(unparsed,out var elementId))
            {
                throw new ArgumentException("参数不是有效的 ElementId", nameof(unparsed));
            }

            return (ElementId)elementId;
        }
    }
}
