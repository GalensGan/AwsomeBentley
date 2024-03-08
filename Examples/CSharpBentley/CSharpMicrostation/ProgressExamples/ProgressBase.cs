using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpMicrostation.ProgressExamples
{
    /// <summary>
    /// 进度条基类
    /// </summary>
    public abstract class ProgressBase
    {
        #region 设置
        /// <summary>
        /// 进度条的最大值
        /// </summary>
        public int Max { get; set; } = 100;

        /// <summary>
        /// 自动打开
        /// </summary>
        public bool AutoOpen { get; set; } = true;
        #endregion

        #region 外部入口
        /// <summary>
        /// 在里面执行操作
        /// </summary>
        /// <param name="action"></param>
        public void Run(Action action)
        {
            if (action == null) return;
            try
            {
                action();
            }
            finally
            {
                // 关闭进度条
                Close();
            }
        }

        /// <summary>
        /// 带有返回值的操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public T Run<T>(Func<T> func)
        {
            if (func == null) return default;
            try
            {
                return func();
            }
            finally
            {
                // 关闭进度条
                Close();
            }
        }
        #endregion

        #region 进度条操作
        public abstract void Open();

        /// <summary>
        /// 更新进度条
        /// 总进度为 100
        /// </summary>
        /// <param name="message"></param>
        /// <param name="progress">进度值，默认 0-100</param>
        public abstract void Update(string message, int progress);

        /// <summary>
        /// 更新进度，在原来的基础上自动加 1
        /// </summary>
        /// <param name="message"></param>
        public abstract void Update(string message);

        /// <summary>
        /// 显示消息
        /// </summary>
        /// <param name="message"></param>
        public abstract void DisplayMessage(string message);

        /// <summary>
        /// 关闭窗体
        /// </summary>
        public abstract void Close();
        #endregion
    }
}
