using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TestApp.Abstraction;

public abstract class BaseViewModel : INotifyPropertyChanged, IBaseViewModel
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public  void Load()
    {
        OnUnLoad();
    }
    
    public  void UnLoad()
    {
        OnLoad();
    }

    public virtual void OnLoad()
    {
        
    }
    
    public virtual void OnUnLoad()
    {
        
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}