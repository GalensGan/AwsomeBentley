using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpMicrostation.ProgressExamples.FluentProgress
{
    /// <summary>
    /// 多线程进度条
    /// 由于 MS 是线程不安全的，所以需要在主线程中执行任务，在其它线程显示进度条
    /// </summary>
    public class ThreadProgress : ProgressBase
    {
        public ThreadProgress(string name)
        {
            _name = name;
        }

        #region 内部实现细节
        private static Dictionary<string, ProgressOption> _progressOptionsDic = new Dictionary<string, ProgressOption>();
        private string _name;
        private ProgressOption _options;
        #endregion

        #region 抽象实现
        /// <summary>
        /// 默认自动打开
        /// </summary>
        public override void Open()
        {
            if (_options != default) return;

            // 若有相同名称，则直接复用
            if (_progressOptionsDic.TryGetValue(_name, out var options)) _options = options;
            else
            {
                // 新建一个 Option
                _options = new ProgressOption()
                {
                    MaxValue = Max,
                };
                _options.StartProgress();
                _progressOptionsDic.Add(_name, _options);
            }
        }

        private int _progressValue = 0;
        public override void Update(string message, int progress)
        {
            if (AutoOpen) Open();
            if (_options == null) return;
            _progressValue = progress;
            _options.ProgressValue = progress;
            _options.Message = message;
        }

        public override void Update(string message)
        {
            Update(message, _progressValue + 1);
        }

        public override void DisplayMessage(string message)
        {
            Update(message);
        }

        public override void Close()
        {
            if (_options == null) return;
            _options.Close();
            _progressOptionsDic.Remove(_name);
            _options = null;
        }
        #endregion
    }
}