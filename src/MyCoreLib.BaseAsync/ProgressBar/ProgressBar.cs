using MyCoreLib.BaseAsync.TAP;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyCoreLib.BaseAsync.ProgressBar
{
    public delegate void ProgressingHandler(ProgressArgs args);
    public delegate void ProgressedHandler(ProgressArgs args);

    public class ProgressArgs : EventArgs
    {
        public bool IsRound;
        public int MaxRange;
        public int MinRange;
        public double CurrentPercent;
        public string CurrentMessage;
    }

    public class ProgressBar
    {
        private Task _task;
        public bool IsRound = false;
        public int MaxRange;
        public int MinRange;
        public double CurrentPercent;
        public string BaseMessage;
        public string CurrentMessage;
        public event ProgressingHandler OnProgressing;
        public event ProgressingHandler OnProgressed;

        public void Start()
        {
            _task = TaskManager.StartNew(() =>
            {
                ChangePercent();
                ChangeMessage();
                if (OnProgressing != null)
                    OnProgressing(new ProgressArgs()
                    {
                        IsRound = this.IsRound,
                        MinRange = this.MinRange,
                        MaxRange = this.MaxRange,
                        CurrentPercent = this.CurrentPercent,
                        CurrentMessage = this.CurrentMessage
                    });
            }, () =>
            {
                if (OnProgressed != null)
                    OnProgressed(new ProgressArgs()
                    {
                        IsRound = this.IsRound,
                        MinRange = this.MinRange,
                        MaxRange = this.MaxRange,
                        CurrentPercent = this.CurrentPercent,
                        CurrentMessage = this.CurrentMessage
                    });
            });
            TaskManager.AddCancel();
        }

        public void Cancel()
        {
            TaskManager.CancelTasks();
        }

        private void ChangePercent()
        {
            MinRange++;
            CurrentPercent = MinRange / MaxRange;
        }

        public void ChangeMessage(string msg = "")
        {
            if (string.IsNullOrWhiteSpace(msg))
            {
                CurrentMessage = string.Format(BaseMessage, CurrentPercent);
            }
            else
            {
                CurrentMessage = msg;
            }
        }
    }
}
