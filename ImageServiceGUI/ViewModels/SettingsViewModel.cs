﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ImageServiceGUI.Annotations;

namespace ImageServiceGUI.ViewModels
{
    class SettingsViewModel : INotifyPropertyChanged
    {
        public SettingsViewModel()
        {
            Debug.WriteLine("SettingsViewModel c'tor");
            LogName = "[Log name here]";
            SourceName = "[Source Name Here]";
            OutputDirectory = "[Output Directory Here]";
            ThumbnailSize = 120;
        }

        public string LogName { get; set; }
        public string SourceName { get; set; }
        public string OutputDirectory { get; set; }
        public int ThumbnailSize { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}