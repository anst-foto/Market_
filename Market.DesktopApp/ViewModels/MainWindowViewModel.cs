using System;
using System.Collections.ObjectModel;
using System.Linq;
using DynamicData;
using Market.BLL;
using Market.Models;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace Market.DesktopApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly Service _service = new();

    public ObservableCollection<Product> Products { get; } = [];
    
    [Reactive] private Product? _selectedProduct;
    
    [Reactive] private string? _searchText;

    [Reactive] private int? _id;
    [Reactive] private string? _name;
    [Reactive] private decimal? _price;

    private readonly IObservable<bool> _canExecuteDelete;
    private readonly IObservable<bool> _canExecuteSave;
    private readonly IObservable<bool> _canExecuteClear;
    
    public MainWindowViewModel()
    {
        this.WhenAnyValue(vm => vm.SelectedProduct)
            .WhereNotNull()
            .Subscribe(p =>
            {
                Id = p.Id;
                Name = p.Name;
                Price = p.Price;
            });
        this.WhenAnyValue(vm => vm.SearchText)
            .WhereNotNull()
            .Subscribe(t =>
            {
                var products = _service.GetByName(t);
                
                Products.Clear();
                foreach (var product in products)
                {
                    Products.Add(product);
                }
            });

        _canExecuteDelete = this.WhenAnyValue(
            wm => wm.SelectedProduct,
            vm => vm.Id,
            (p1, p2) => p1 is not null ||
                        p2 is not null);

        _canExecuteSave = this.WhenAnyValue(
            vm => vm.Name,
            vm => vm.Price,
            (n, p) => !string.IsNullOrWhiteSpace(n) && 
                      p is not null);
        
        _canExecuteClear = this.WhenAnyValue(
            vm => vm.Name,
            vm => vm.Price,
            (n, p) => !string.IsNullOrWhiteSpace(n) || 
                      p is not null);
    }

    [ReactiveCommand]
    private void Load()
    {
        var products = _service.GetAll();
        
        Products.Clear();
        foreach (var product in products)
        {
            Products.Add(product);
        }
    }

    [ReactiveCommand(CanExecute = nameof(_canExecuteClear))]
    private void Clear()
    {
        SelectedProduct = null;
        
        Id = null;
        Name = null;
        Price = null;
    }

    [ReactiveCommand(CanExecute = nameof(_canExecuteDelete))]
    private void Delete()
    {
        _service.Delete(SelectedProduct!);
        
        Clear();
        Load();
    } 

    [ReactiveCommand(CanExecute = nameof(_canExecuteSave))]
    private void Save()
    {
        if (SelectedProduct is null)
        {
            _service.Add(new Product(Name!, Price!.Value));
        }
        else
        {
            _service.Update(new Product(Id!.Value, Name!, Price!.Value));
        }
        
        Clear();
        Load();
    }
}