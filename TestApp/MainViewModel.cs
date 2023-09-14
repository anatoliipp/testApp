using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Newtonsoft.Json;
using Prism.Events;
using TestApp.Abstraction;
using TestApp.Infrastructure;
using TestApp.Infrastructure.Events;
using TestApp.Infrastructure.Models;
using TestApp.Infrastructure.Repository;

namespace TestApp;

public class MainViewModel : BaseViewModel
{
    private readonly IEventAggregator _eventAggregator;
    private readonly IRepository<Person> _personRepository;
    public ICommand LoadDataCommand { get; private set; }
    public ObservableCollection<Person> Persons { get; set; }

    public MainViewModel(IEventAggregator eventAggregator, IRepository<Person> personRepository)
    {
        _eventAggregator = eventAggregator;
        _personRepository = personRepository;
        Persons = new ObservableCollection<Person>();
        LoadDataCommand = new RelayCommand(LoadDataAction, i => true);
    }

    private async void LoadDataAction(object? obj)
    {
        var persons = await _personRepository.ListAsync();
        RefreshPersons(persons);
    }

    private void RefreshPersons(List<Person> persons)
    {
        Persons.Clear();
        foreach (var person in persons)
        {
            Persons.Add(person);
        }
    }
    
    private async void OnFileChangeEvent()
    {
        var persons = await _personRepository.ListAsync();
        RefreshPersons(persons);
    }

    public override void OnLoad()
    {
        LoadDataAction(null);
        _eventAggregator.GetEvent<DataChangeEvent>().Subscribe(OnFileChangeEvent);
    }

    public override void OnUnLoad()
    {
        _eventAggregator.GetEvent<DataChangeEvent>().Unsubscribe(OnFileChangeEvent);
    }
}