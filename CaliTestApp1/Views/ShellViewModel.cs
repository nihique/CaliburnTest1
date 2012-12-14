using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace CaliTestApp1.Views 
{
    [Export(typeof(ShellViewModel))]
    public class ShellViewModel : PropertyChangedBase
    {
        private int _count = 50;

        public int Count
        {
            get { return _count; }
            set
            {
                _count = value;
                NotifyOfPropertyChange(() => Count);
                NotifyOfPropertyChange(() => CanChangeCount);
            }
        }

        public void ChangeCount(int delta)
        {
            Count = Count + delta;
        }

        public bool CanChangeCount
        {
            get { return Count < 100; }
        }
    }
}