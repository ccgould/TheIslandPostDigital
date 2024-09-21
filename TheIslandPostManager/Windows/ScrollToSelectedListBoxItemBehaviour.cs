using Microsoft.Xaml.Behaviors;
using System.Windows.Controls;
using System.Windows;
using TheIslandPostManager.Models;

namespace TheIslandPostManager.Windows;
public class ScrollToSelectedListBoxItemBehavior : Behavior<ListBox>
{
    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.SelectionChanged += AssociatedObjectOnSelectionChanged;
        AssociatedObject.IsVisibleChanged += AssociatedObjectOnIsVisibleChanged;
    }

    protected override void OnDetaching()
    {
        AssociatedObject.SelectionChanged -= AssociatedObjectOnSelectionChanged;
        AssociatedObject.IsVisibleChanged -= AssociatedObjectOnIsVisibleChanged;
        base.OnDetaching();
    }

    private static void AssociatedObjectOnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        ScrollIntoFirstSelectedItem(sender);
    }

    private static void AssociatedObjectOnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ScrollIntoFirstSelectedItem(sender);
    }

    private static void ScrollIntoFirstSelectedItem(object sender)
    {
        var settings = App.AppConfig.GetSection("AppSettings") as AppSettings;

        if (!(sender is ListBox listBox) || !settings.ScrollSync)
            return;
        var selectedItems = listBox.SelectedItems;
        if (selectedItems.Count > 0)
            listBox.ScrollIntoView(selectedItems[0]);
    }
}