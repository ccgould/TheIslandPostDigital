﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TheIslandPostManager.ViewModels;

namespace TheIslandPostManager.Views.Pages;
/// <summary>
/// Interaction logic for EarningsPage.xaml
/// </summary>
public partial class EarningsPage : Page
{
    public EarningsPage(EarningsPageViewmodel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}
