using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
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
        zoomBorder.MouseEnter += (s, e) => Mouse.OverrideCursor = Cursors.Hand;
        zoomBorder.MouseLeave += (s, e) => Mouse.OverrideCursor = Cursors.Arrow;
        zoomBorder.MouseEnter += (s, e) => buttonView.Visibility = Visibility.Visible;
        buttonView.MouseEnter += (s, e) => buttonView.Visibility = Visibility.Visible;

        zoomBorder.MouseLeave += (s, e) => buttonView.Visibility = Visibility.Hidden;

        zoomBorder.Focusable = true;

        zoomBorder.Focus();
    }

    private void Reset(object sender, RoutedEventArgs e)
    {
        zoomBorder.Reset();
    }
}
