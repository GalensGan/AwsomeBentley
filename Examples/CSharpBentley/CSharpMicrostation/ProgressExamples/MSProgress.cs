using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace CSharpMicrostation.ProgressExamples
{
    /// <summary>
    /// MS 进度条
    /// 参考 <see cref="https://communities.bentley.com/communities/other_communities/chinafirst/f/microstation-projectwise/214999/ms-clr/654058"/>
    /// 作者: galens
    /// </summary>
    public class MSProgress : ProgressBase
    {
        #region P/Invoke
        /// <summary>
        /// 打开窗体
        /// </summary>
        /// <param name="messageText"></param>
        /// <returns></returns>
        [DllImport("ustation.dll", CharSet = CharSet.Unicode)]
        public extern static IntPtr mdlDialog_completionBarOpen(string messageText);
        /// <summary>
        /// 更新窗体内容
        /// </summary>
        /// <param name="dialog"></param>
        /// <param name="messageText"></param>
        /// <param name="percent"></param>
        [DllImport("ustation.dll", CharSet = CharSet.Unicode)]
        public extern static void mdlDialog_completionBarUpdate(IntPtr dialog, string messageText, int percent);
        /// <summary>
        /// 显示消息
        /// </summary>
        /// <param name="dialog"></param>
        /// <param name="messageText"></param>
        [DllImport("ustation.dll", CharSet = CharSet.Unicode)]
        public extern static void mdlDialog_completionBarDisplayMessage(IntPtr dialog, string messageText);
        /// <summary>
        /// 关闭进度条
        /// </summary>
        /// <param name="dialog"></param>
        [DllImport("ustation.dll", CharSet = CharSet.Unicode)]
        public extern static void mdlDialog_completionBarClose(IntPtr dialog);
        #endregion

        #region 全局变量
        private Dictionary<string, IntPtr> _dialogDic = new Dictionary<string, IntPtr>();
        #endregion

        #region 构造函数
        /// <summary>
        /// 实例化时，自动打开进度窗体
        /// </summary>
        /// <param name="name"></param>
        public MSProgress(string name)
        {
            _name = name;
        }
        #endregion

        #region 内部细节
        private readonly string _name;
        private IntPtr _dialogPtr;
        #endregion

        #region 外部入口
        /// <summary>
        /// 在里面执行操作
        /// </summary>
        /// <param name="action"></param>
        public void Run(Action<MSProgress> action)
        {
            if (action == null) return;
            try
            {
                action(this);
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
        public T Run<T>(Func<MSProgress, T> func)
        {
            if (func == null) return default;
            try
            {
                return func(this);
            }
            finally
            {
                // 关闭进度条
                Close();
            }
        }
        #endregion

        #region 进度条操作
        public override void Open()
        {
            if (_dialogPtr != default) return;

            // 若有相同名称，则直接复用
            if (_dialogDic.TryGetValue(_name, out var dialog)) _dialogPtr = dialog;
            else
            {
                _dialogPtr = mdlDialog_completionBarOpen(string.Empty);
                _dialogDic.Add(_name, _dialogPtr);
            }
        }

        private int _progress = 0;
        /// <summary>
        /// 更新进度条
        /// 总进度为 100
        /// </summary>
        /// <param name="message"></param>
        /// <param name="progress">进度值，默认 0-100</param>
        public override void Update(string message, int progress)
        {
            if (AutoOpen) Open();
            _progress = progress;

            // 如果进度条最大值不是 100，则进行换算
            int percent = _progress;
            if (Max != 100)
            {
                percent = _progress / 100 * Max;
            }

            if (_dialogPtr == default) return;
            mdlDialog_completionBarUpdate(_dialogPtr, message, percent);

            // 让窗体处理消息事件
            Application.DoEvents();
        }

        /// <summary>
        /// 更新进度，在原来的基础上自动加 1
        /// </summary>
        /// <param name="message"></param>
        public override void Update(string message)
        {
            Update(message, _progress + 1);
        }

        /// <summary>
        /// 显示消息
        /// </summary>
        /// <param name="message"></param>
        public override void DisplayMessage(string message)
        {
            if (AutoOpen) Open();
            if (_dialogPtr == default) return;
            mdlDialog_completionBarDisplayMessage(_dialogPtr, message);
        }

        private bool _closed = false;
        /// <summary>
        /// 关闭窗体
        /// </summary>
        public override void Close()
        {
            if (_closed) return;
            _closed = true;
            mdlDialog_completionBarClose(_dialogPtr);
        }
        #endregion

        #region 静态帮助方法
        public static MSProgress GetMSProgress(string name)
        {
            return new MSProgress(name);
        }
        #endregion
    }
}
