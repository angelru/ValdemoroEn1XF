using Prism.Mvvm;
using System.Windows.Input;

namespace ValdemoroEn1.Models
{
    public class TransportItem : BindableBase
    {
        public string Id { get; set; }

        string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public ICommand EditTimeBusCommand { get; set; }
        public ICommand DeleteTimeBusCommand { get; set; }
        public ICommand LoadTimeBusCommand { get; set; }
    }
}
