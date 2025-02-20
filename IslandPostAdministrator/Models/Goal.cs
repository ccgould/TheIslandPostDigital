#region Copyright Syncfusion Inc. 2001-2024.
// Copyright Syncfusion Inc. 2001-2024. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using CommunityToolkit.Mvvm.ComponentModel;

namespace IslandPostAdministrator.Models;

public partial class Goal : ObservableObject
{
    [ObservableProperty] private string name;
    [ObservableProperty] private DateTime startDate;
    [ObservableProperty] private DateTime endDate;
    [ObservableProperty] private double target;
    [ObservableProperty] private double saving;

    private string ValidateName()
    {
        if (string.IsNullOrEmpty(Name))
        {
            return "Name cannot be empty";
        }
        return null;
    }

    private string ValidateEndDate()
    {
        if (EndDate.Date <= StartDate.Date)
        {
            return "End date cannot be less than today";
        }
        return null;
    }

    private string ValidateTarget()
    {
        if (Target < 1)
        {
            return "Target cannot be less than 1";
        }
        return null;
    }

    public string Status
    {
        get
        {
            if (Saving >= Target)
            {
                return "Successfully\nCompleted";
            }
            else if (DateTime.Now <= EndDate)
            {
                var days = (EndDate.Date - DateTime.Now.Date).Days;
                if (days > 30)
                {
                    return days / 30 + " months left";
                }
                else
                {
                    return days + " days left";
                }
            }
            else
            {
                return "Incomplete";
            }
        }
    }

    public double MonthlyTarget
    {
        get
        {
            var days = (EndDate - StartDate).Days;
            if (days > 30)
            {
                return target / (days / 30);
            }
            else
            {
                return target;
            }
        }
    }

    public double CompletePercent
    {
        get
        {
            return Saving / Target;
        }
    }


    public string Error
    {
        get
        {
            return ValidateName() ?? ValidateEndDate() ?? ValidateTarget() ?? null;
        }
    }

    //public string this[string columnName]
    //{
    //    get
    //    {
    //        RaisePropertyChanged(nameof(Error));
    //        switch (columnName)
    //        {
    //            case nameof(Name):
    //                return ValidateName();
    //            case nameof(EndDate):
    //                return ValidateEndDate();
    //            case nameof(Target):
    //                return ValidateTarget();
    //            default:
    //                return null;
    //        }
    //    }
    //}

    public Goal Clone()
    {
        var g = new Goal();
        g.Name = Name;
        g.StartDate = StartDate;
        g.EndDate = EndDate;
        g.Target = Target;
        g.Saving = Saving;
        return g;
    }

    public void Apply(Goal g)
    {
        Name = g.Name;
        StartDate = g.StartDate;
        EndDate = g.EndDate;
        Target = g.Target;
        Saving = g.Saving;
    }

}

public class AddGoal : Goal { }