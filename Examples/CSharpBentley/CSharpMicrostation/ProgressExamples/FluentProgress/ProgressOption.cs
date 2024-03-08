using Bentley.UI.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace CSharpMicrostation.ProgressExamples.FluentProgress
{
    /// <summary>
    /// 进度条设置
    /// </summary>
    public class ProgressOption : ViewModelBase
    {
        private DateTime _startDate = DateTime.Now;
        public ProgressOption()
        {
            _messageBuildersDic = new Dictionary<ProgressLabel, Func<string>>()
            {
                { ProgressLabel.ActualProgress,BuildActualProgress },
                { ProgressLabel.Percent,BuildPercent },
                { ProgressLabel.ToalTime,BuildTotalTime },
                { ProgressLabel.RemainTime,BuildRemainTime },
                { ProgressLabel.Speed,BuildSpeed },
            };
        }

        private int _maxValue = 100;
        /// <summary>
        /// 最大值
        /// </summary>
        public int MaxValue
        {
            get => _maxValue;
            set
            {
                if (_maxValue != value)
                {
                    _maxValue = value;
                    NotifyOfPropertyChange(() => MaxValue);
                }
            }
        }

        private int _progressValue = 0;
        public int ProgressValue
        {
            get => _progressValue;
            set
            {
                if (_progressValue != value)
                {
                    _progressValue = value;
                    // 更新显示进度
                    UpdateProgressInfo();
                    NotifyOfPropertyChange(() => ProgressValue);
                }
            }
        }

        private string _percentLabel = "0.0%";
        public string PercentLabel
        {
            get => _percentLabel;
            set
            {
                if (_percentLabel != value)
                {
                    _percentLabel = value;
                    NotifyOfPropertyChange(() => PercentLabel);
                }
            }
        }

        private string _message;

        /// <summary>
        /// 消息体
        /// </summary>
        public string Message
        {
            get => _message;
            set
            {
                if (_message != value)
                {
                    _message = value;
                    NotifyOfPropertyChange(() => Message);
                }
            }
        }

        #region 初始状态
        /// <summary>
        /// 进度条的宽度
        /// </summary>
        public int Width { get; set; } = 400;

        /// <summary>
        /// 进度条的高度
        /// </summary>
        public int Height { get; set; } = 24;

        /// <summary>
        /// 要显示的标签及顺序
        /// </summary>
        public List<ProgressLabel> Labels { get; set; } = new List<ProgressLabel>()
        {
            ProgressLabel.ActualProgress,
            ProgressLabel.Percent,
            ProgressLabel.ToalTime,
            ProgressLabel.RemainTime,
        };
        #endregion

        #region 内部方法
        private Dictionary<ProgressLabel, Func<string>> _messageBuildersDic;
        private void UpdateProgressInfo()
        {
            // 更新消息
            List<Func<string>> messageBuilders = Labels.ConvertAll(x =>
            {
                if (_messageBuildersDic.TryGetValue(x, out var builder)) return builder;
                return null;
            }).FindAll(x => x != null);

            PercentLabel = string.Join(" ", messageBuilders.ConvertAll(x => x.Invoke()));
        }

        private string BuildActualProgress()
        {
            return $"{ProgressValue}/{MaxValue}";
        }

        private string BuildPercent()
        {
            return $"[ {ProgressValue * 100.0 / MaxValue:0.0}% ]";
        }

        private string BuildTotalTime()
        {
            return $"in {DateTime.Now - _startDate:hh\\:mm\\:ss}";
        }

        private string BuildRemainTime()
        {
            // 计算剩余时间
            var remainTime = (DateTime.Now - _startDate).TotalSeconds / ProgressValue * (MaxValue - ProgressValue);
            var remainTimeSpan = new TimeSpan(0, 0, (int)remainTime);
            return $"( {remainTimeSpan:hh\\:mm\\:ss} )";
        }

        private string BuildSpeed()
        {
            return $"{ProgressValue / (DateTime.Now - _startDate).TotalSeconds:0.0}/s";
        }
        #endregion

        #region 入口函数
        /// <summary>
        /// 进度条实例
        /// </summary>
        private Window _window { get; set; }

        public void StartProgress()
        {
            // 新建一个线程并运行
            var thread = new Thread(() =>
            {
                _window = new ProgressWindow(this);
                _window.ShowDialog();
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        public void Close()
        {
            if (_window == null) return;
            _window.Dispatcher.Invoke(() =>
            {
                _window.Close();
            });
            _window = null;
        }
        #endregion
    }

    /// <summary>
    /// 进度条标签
    /// </summary>
    public enum ProgressLabel
    {
        /// <summary>
        /// 实际进度
        /// 28/100
        /// </summary>
        ActualProgress,

        /// <summary>
        /// 百分比
        /// [ 80% ]
        /// </summary>
        Percent,

        /// <summary>
        /// 显示动态图标
        /// 参考：https://jsfiddle.net/sindresorhus/2eLtsbey/embedded/result/
        /// </summary>
        //Spinner,

        /// <summary>
        /// 总耗时
        /// in 10s
        /// </summary>
        ToalTime,

        /// <summary>
        /// 剩余时间
        /// ( 10s )
        /// </summary>
        RemainTime,

        /// <summary>
        /// 执行速度
        /// 9.2/s
        /// </summary>
        Speed,
    }
}
