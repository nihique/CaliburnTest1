using System.ComponentModel.Composition;

namespace CaliTestApp1.Views 
{
    [Export(typeof(IShell))]
    public class ShellViewModel : IShell {}
}