﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace QuanLyDaiLy.ViewModels.MatHangViewModels
{
    public class MatHangPageViewModel : INotifyPropertyChanged
    {
        private string _hello = "Hello";
        public string Hello
        {
            get => _hello;
            set
            {
                _hello = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
