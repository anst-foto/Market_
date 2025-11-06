using System.Collections.ObjectModel;
using System.Reactive;
using Market.BLL;
using Market.Models;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace Market.DesktopApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly Service _service = new();
    
    public ObservableCollection<Product> Products { get; } = [];
    
    [Reactive] private string? searchText;
    
    public ReactiveCommand<Unit, Unit> CommandLoad { get; }

    public MainWindowViewModel()
    {
        CommandLoad = ReactiveCommand.Create(() =>
        {
            var products = _service.GetAll();
            
            Products.Clear();
            foreach (var product in products)
            {
                Products.Add(product);
            }
        });
    }
}