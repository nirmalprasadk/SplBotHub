using SplBotHub.ViewModels;
using System.Windows;

namespace SplBotHub;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly MainWindowViewModel _mainWindowViewModel;

    public MainWindow(MainWindowViewModel mainWindowViewModel)
    {
        InitializeComponent();
        _mainWindowViewModel = mainWindowViewModel;
        DataContext = mainWindowViewModel;
    }
}