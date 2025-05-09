using Generator.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Generator.ViewModel
{
    public class MainViewModel
    {
        public ICommand OpenCreateQuizCommand { get; }
        public ICommand OpenEditQuizCommand { get; }


        public MainViewModel()
        {
            OpenCreateQuizCommand = new RelayCommand(OpenCreateQuizWindow);
            OpenEditQuizCommand = new RelayCommand(OpenEditQuizWindow);
        }

        private void OpenCreateQuizWindow(object? parameter)
        {
            var window = new CreateQuizWindow();
            window.Show();
        }

        private void OpenEditQuizWindow(object? parameter)
        {
            var window = new ChooseQuizWindow();
            window.Show();
        }
    }
}
