using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using TheIslandPostManager.Services;
using TheIslandPostManager.ViewModels;

namespace TheIslandPostManager.Windows;

/// <summary>
/// Interaction logic for CustomerWindow.xaml
/// </summary>
public partial class CustomerWindow
{
    public CustomerWindow(CustomerWindowViewmodel vm)
    {
        InitializeComponent();
        DataContext = vm;
        zoomBorder.Focusable = false;

        zoomBorder.Focus();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Maximized;
    }

    private void Reset(object sender, RoutedEventArgs e)
    {
        zoomBorder.Reset();
    }
}
