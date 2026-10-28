using System;
using System.Windows;
using System.Windows.Controls;

namespace CobimExplorer.Views.Page.Start
{
    /// <summary>
    /// Interaction logic for StartView.xaml
    /// </summary>
    public partial class StartV : UserControl
    {
        public StartV() 
        {
            InitializeComponent();

            // this.DataContext = new StartViewModel();   // View(StartView)와 ViewModel(StartViewModel) 연결, 참고 URL - https://github.com/canton7/Stylet/wiki/ViewModel-First
        }
    }
}
