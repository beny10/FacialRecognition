using FacialRecognitionLogical;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FacialRecognition
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {

        private DelegateCommand _checkCommand;
        private Recognizer _recognizer;
        private Camera _camera;
        public DelegateCommand CheckCommand
        {
            get { return _checkCommand; }
            set
            {
                _checkCommand = value;
                OnPropertyChanged("CheckCommand");
            }
        }



        public MainPage()
        {
            this.InitializeComponent();
            DataContext = this;
            CheckCommand = new DelegateCommand(CheckCamera);
            _recognizer = new Recognizer("test");
            _camera = new Camera(WebcamFeed);
        }

        private void WebcamFeed_Loaded(object sender, RoutedEventArgs e)
        {
            _camera.InitializingCamera();
        }
        private async void MessageBox(string text)
        {
            MessageDialog dialog = new MessageDialog(text);
            dialog.ShowAsync();
        }
        private async void CheckCamera()
        {
            Stream image = await _camera.Capture();
            try
            {
                List<Person> persons = await _recognizer.GetPersons(image);
                MessageBox(persons.Count.ToString());
            }
            catch (FaceAPIException ee)
            {
                MessageBox(ee.Message);
            }
            catch (System.Net.Http.HttpRequestException ee)
            {
                MessageBox(ee.Message);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
