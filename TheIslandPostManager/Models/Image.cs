﻿using CommunityToolkit.Mvvm.ComponentModel;
using MySqlX.XDevAPI.Common;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TheIslandPostManager.Services;
using Path = System.IO.Path;

namespace TheIslandPostManager.Models;

public partial class ImageObj : ObservableObject, IImage
{
    [ObservableProperty] private string imageUrl;
    [ObservableProperty] private string name;
    [ObservableProperty] private BitmapImage lowImage;
    [ObservableProperty] private BitmapImage hDImage;

    [ObservableProperty] private int printAmount = 1;
    [ObservableProperty] private bool isSelected;
    [ObservableProperty] private bool isPending;
    [ObservableProperty] private bool isPrintable;
    [ObservableProperty] private int index;
    private bool isDirty;
    private IFileService _fileService;

    [ObservableProperty] private string rootImageUrl;

    public ImageObj()
    {
        if(_fileService is null)
        {
            _fileService = App.GetService<IFileService>();
        }
    }

    public ImageObj(string path,bool loadImage = true)
    {
        RootImageUrl = path;
        this.name = Path.GetFileName(path);
        this.imageUrl = path;

        if (_fileService is null)
        {
            _fileService = App.GetService<IFileService>();
        }

        if(loadImage)
        {
            LoadImage(path);
        }
    }

    public ImageObj Copy(bool copy = false)
    {
        var newFile = Path.Combine(Path.GetDirectoryName(ImageUrl), $"{Guid.NewGuid().ToString("N")}.JPG");
        _fileService.Copy(ImageUrl, newFile);
        var result = new ImageObj();

        if(copy)
        {
            result = new ImageObj(newFile);
            result.Name = Name;
            result.Index = Index;
        }
        else
        {
            result.PrintAmount = PrintAmount;
            result.Name = Name;
            result.ImageUrl = ImageUrl;
            result.LowImage = LowImage;
            result.HDImage = HDImage;
            result.IsSelected = IsSelected;
            result.IsPending = IsPending;
            result.IsPrintable = IsPrintable;
            result.Index = Index;
        }


        return result;
    }

    public void Restore(IImage image)
    {
        PrintAmount = image.PrintAmount;
        Name = image.Name;
        ImageUrl = image.ImageUrl;
        LowImage = image.LowImage;
        HDImage = image.HDImage;
        IsSelected = image.IsSelected;
        IsPending = image.IsPending;
        IsPrintable = image.IsPrintable;
        Index = image.Index;
    }

    internal void Restore(string path)
    {
        ImageUrl = path;
        LoadImage(path);
    }

    public void LoadImage(string path)
    {
        HDImage = _fileService.LoadImageFile(path, true);
        LowImage = _fileService.LoadImageFile(path);
    }
    
    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        if (isDirty) return;

        if (e.PropertyName.Equals(nameof(IsSelected)))
        {
            if (!IsSelected)
            {
                isDirty = true;
                IsPrintable = false;
            }
        }

        if (e.PropertyName.Equals(nameof(IsPending)))
        {
            if (IsSelected)
            {
                isDirty = true;
                IsSelected = false;
            }
        }

        if (e.PropertyName.Equals(nameof(IsSelected)))
        {
            if (IsPending)
            {
                isDirty = true;

                IsPending = false;
            }
        }

        isDirty = false;
    }
}
