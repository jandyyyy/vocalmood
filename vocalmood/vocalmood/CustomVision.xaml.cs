using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Geolocator;
using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vocalmood.Model;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json.Linq;

namespace vocalmood
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CustomVision : ContentPage
	{
		public CustomVision ()
		{
			InitializeComponent ();
		}

        private async void loadCamera(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            MediaFile file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                PhotoSize = PhotoSize.Medium,
                Directory = "Sample",
                Name = $"{DateTime.UtcNow}.jpg"
            });

            if (file == null)
                return;

            image.Source = ImageSource.FromStream(() =>
            {
                return file.GetStream();
            });

            await postLocationAsync();
            await MakePredictionRequest(file);
            file.Dispose();
        }

        async Task postLocationAsync()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;

            var position = await locator.GetPositionAsync();

            vocalmoodModel model = new vocalmoodModel()
            {
                Longitude = (float)position.Longitude,
                Latitude = (float)position.Latitude
            };

            await AzureManager.AzureManagerInstance.PostvocalmoodInformation(model);
        }
        
        static byte[] GetImageAsByteArray(MediaFile file)
        {
            var stream = file.GetStream();
            BinaryReader binaryReader = new BinaryReader(stream);
            return binaryReader.ReadBytes((int)stream.Length);
        }

        async Task MakePredictionRequest(MediaFile file)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("Prediction-Key", "10ef0f79376d445f957a9c3c8e97e04f");

            string url = "https://southcentralus.api.cognitive.microsoft.com/customvision/v1.0/Prediction/ab32939d-6293-4c1e-bc93-65f838a0d4e7/image?iterationId=0142fdf8-0df6-4a85-9073-8f99fd4ab91c";

            HttpResponseMessage response;

            byte[] byteData = GetImageAsByteArray(file);

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    ///*
                    JObject rss = JObject.Parse(responseString);

                    var Probability = from p in rss["Predictions"] select (int)p["Probability"];
                    var Tag = from p in rss["Predictions"] select (string)p["Tag"];

                    foreach (var item in Tag)
                    {
                        TagLabel.Text += item + ": \n";
                    }

                    foreach (var item in Probability)
                    {

                        PredictionLabel.Text += item + "\n";
                    }
                    //*/
                    /*

                    EvaluationModel responseModel = JsonConvert.DeserializeObject<EvaluationModel>(responseString);

                    double max = responseModel.Predictions.Max(m => m.Probability);

                    TagLabel.Text = (max >= 0.5) ? "Beagle" : "Border Collie";
                    */
                }
                else
                {
                    TagLabel.Text = "Something is not right";
                }

                file.Dispose();
            }
        }

    }
}