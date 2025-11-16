using BotHub.ViewModels;
using System.Windows;

namespace BotHub;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(MainWindowViewModel mainWindowViewModel)
    {
        InitializeComponent();
        DataContext = mainWindowViewModel;
        Closed += mainWindowViewModel.OnWindowClosed;
    }
}