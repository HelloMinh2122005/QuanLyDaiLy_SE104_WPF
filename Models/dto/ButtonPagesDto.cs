using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows;

namespace QuanLyDaiLy.Models.dto
{
    public class ButtonPagesDto : ObservableObject
    {
        private string _buttonContent = "";
        public string ButtonContent
        {
            get => _buttonContent;
            set => SetProperty(ref _buttonContent, value);
        }

        private string _buttonParam = "";
        public string ButtonParam
        {
            get => _buttonParam;
            set => SetProperty(ref _buttonParam, value);
        }

        private Style? _buttonStyle;
        public Style ButtonStyle
        {
            get => _buttonStyle ?? new Style(); 
            set => SetProperty(ref _buttonStyle, value);
        }

        private Visibility _buttonVisibility = Visibility.Visible;
        public Visibility ButtonVisibility
        {
            get => _buttonVisibility;
            set => SetProperty(ref _buttonVisibility, value);
        }
    }
}
