using System;
using System.ComponentModel;
using Caliburn.Micro;

namespace CaliTestApp1.Utils
{
    public class ExecuteInBackground : IResult
    {
        private readonly System.Action action;

        public ExecuteInBackground(System.Action action)
        {
            this.action = action;
        }

        public void Execute(ActionExecutionContext context)
        {
            using (var backgroundWorker = new BackgroundWorker())
            {
                backgroundWorker.DoWork += (e, sender) => action();
                backgroundWorker.RunWorkerCompleted += (e, sender) => Completed(this, new ResultCompletionEventArgs());
                backgroundWorker.RunWorkerAsync();
            }
        }

        public event EventHandler<ResultCompletionEventArgs> Completed = delegate { };
    }
}