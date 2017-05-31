using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Media.Capture;
using Windows.System.Display;
using Windows.Media.MediaProperties;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace InventoryCheck
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        MediaCapture _mediaCapture;
        DisplayRequest _displayRequest = new DisplayRequest();

        public MainPage()
        {
            this.InitializeComponent();
        }

        void Page_Loaded(object sender, RoutedEventArgs e)
        {
        }

        async Task MakePredictionRequest(byte[] photoBytes)
        {
            var client = new HttpClient();

            // Request headers - replace this example key with your valid subscription key.
            client.DefaultRequestHeaders.Add("Prediction-Key", "72ce1b8d4771440bbed9789b08f4f3ec");

            // Prediction URL - replace this example URL with your valid prediction URL.
            string url = "https://southcentralus.api.cognitive.microsoft.com/customvision/v1.0/Prediction/222a4ac1-0a89-42b7-9df8-9bf77a7e4d4d/image?iterationId=19651a29-428a-41da-8839-50ec3667216b";
            HttpResponseMessage response;

            // Request body. Try this sample with a locally stored image.
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Pictures/2cuptest.jpg"));

            //byte[] imageToBytes = await ReadFile(file);

            using (var content = new ByteArrayContent(photoBytes))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(url, content);
                string contentString = await response.Content.ReadAsStringAsync();
                jsonInfo.Text = contentString;

                if (String.IsNullOrWhiteSpace(jsonInfo.Text))
                {
                    jsonInfo.Text = "nothing";
                }
            }
        }

        //public async Task<byte[]> ReadFile(StorageFile file)
        //{
        //    byte[] fileBytes = null;
        //    using (IRandomAccessStreamWithContentType stream = await file.OpenReadAsync())
        //    {
        //        fileBytes = new byte[stream.Size];
        //        using (DataReader reader = new DataReader(stream))
        //        {
        //            await reader.LoadAsync((uint)stream.Size);
        //            reader.ReadBytes(fileBytes);
        //        }
        //    }

        //    return fileBytes;
        //}

        
        async private void Button_Click(object sender, RoutedEventArgs e)
        {
            _mediaCapture = new MediaCapture();
            await _mediaCapture.InitializeAsync();

            // Prepare and capture photo
            var lowLagCapture = await _mediaCapture.PrepareLowLagPhotoCaptureAsync(ImageEncodingProperties.CreateUncompressed(MediaPixelFormat.Bgra8));

            var capturedPhoto = await lowLagCapture.CaptureAsync();
            var softwareBitmap = capturedPhoto.Frame.SoftwareBitmap;

            using (var captureStream = new InMemoryRandomAccessStream())
            {
                await _mediaCapture.CapturePhotoToStreamAsync(ImageEncodingProperties.CreateJpeg(), captureStream);

            }

            byte[] imageByteArray = new byte[] { };
        }
    }
}
