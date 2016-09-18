using FacialRecognitionLogical;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;

namespace FacialRecognition
{
    public class Camera
    {
        private CaptureElement _webcamFeed;
        private MediaCapture _mediaCapture;
        public MediaCapture MediaCapture
        {
            get { return _mediaCapture; }
            set
            {
                _mediaCapture = value;
            }
        }
        public Camera(CaptureElement webcamFeed)
        {
            _webcamFeed = webcamFeed;
        }
        public async void InitializingCamera()
        {
            MediaCapture = new MediaCapture();
            await MediaCapture.InitializeAsync();
            _webcamFeed.Source = MediaCapture;
            await MediaCapture.StartPreviewAsync();
        }
        public async Task<Stream> Capture()
        {
            StorageFile file = await ApplicationData.Current.TemporaryFolder.CreateFileAsync($"{Helpers.GenerateTimeStamp()}.jpg");
            await _mediaCapture.CapturePhotoToStorageFileAsync(ImageEncodingProperties.CreateJpeg(), file);
            return File.OpenRead(file.Path);
            return await file.OpenStreamForReadAsync();
        }
    }
}
