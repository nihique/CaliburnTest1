using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using CaliTestApp1.Utils;
using Caliburn.Micro;

namespace CaliTestApp1.Views 
{
    [Export(typeof(ShellViewModel))]
    public class ShellViewModel : PropertyChangedBase
    {
        private int _count = 5;

        public int Count
        {
            get { return _count; }
            set
            {
                _count = value;
                NotifyOfPropertyChange(() => Count);
                NotifyOfPropertyChange(() => CanIncrementCount);
                NotifyOfPropertyChange(() => CanDecrementCount);
            }
        }

        public bool CanDecrementCount
        {
            get { return Count > 0; }
        }
        public bool CanIncrementCount
        {
            get { return Count < 100; }
        }

        public void IncrementCount(int delta)
        {
            Count += delta;
        }

        public void DecrementCount(int delta)
        {
            Count -= delta;
        }

        public IEnumerable<IResult> IncrementCountAsync()
        {
            IncrementCount(1);
            for (int i = 0; i < 19; i++)
            {
                yield return new ExecuteInBackground(() =>
                {
                    Thread.Sleep(200); 
                    IncrementCount(1);
                }); 
            }
        }
    }
}