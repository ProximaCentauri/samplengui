using System;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using SSCOUIModels.Models;
using SSCOUIModels.Commands;
using FPsxWPF;

namespace SSCOUIModels
{
    public interface IMainViewModel : INotifyPropertyChanged, IDisposable, IDisplayNotify
    {
        ICommand ActionCommand { get; set; }

        string ActiveStateParam { get; }

        bool AttendantMode { get; }

        string BackgroundStateParam { get; }

        string CustomerBackgroundStateParam { get; }

        CustomerReceiptItem CurrentItem { get; }

        int CustomerLanguage { get; }

        string DegradedMode { get; }

        object GetPropertyValue(string propertyName);
                
        int Language { get; }

        bool ShowTransitionEffects { get; set; }

        string State { get; }

        string StateParam { get; }

        string StoreBackgroundStateParam { get; }

        bool StoreMode { get; }

        bool TrainingMode { get; }

        UIEchoField UIEchoField { get; }

        string TBState { get; }

        PerfCheck Perfcheck { get; set; }

        UIPicklistDisplayLevels UIPicklistDisplayLevels { get; }

        Dictionary<string, Context> ParamToViews { get; }

        bool UNav { get; }
    }
}
