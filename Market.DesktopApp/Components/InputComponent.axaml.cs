using Avalonia;
using Avalonia.Controls;

namespace Market.DesktopApp.Components;

public partial class InputComponent : UserControl
{
    public static readonly StyledProperty<object> LabelProperty = 
        AvaloniaProperty.Register<InputComponent, object>(nameof(Label));
    public object Label
    {
        get => GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }
    
    public static readonly StyledProperty<string> ValueProperty = 
        AvaloniaProperty.Register<InputComponent, string>(nameof(Value));
    public string Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }
    
    public InputComponent()
    {
        InitializeComponent();
    }
}