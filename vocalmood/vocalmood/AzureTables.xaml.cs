using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace vocalmood
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AzureTables : ContentPage
	{
        MobileServiceClient client = AzureManager.AzureManagerInstance.AzureClient;

        public AzureTables ()
		{
			InitializeComponent ();
		}

        async void Handle_ClickedAsync(object sender, System.EventArgs e)
        {
            List<vocalmoodModel> vocalmoodInformation = await AzureManager.AzureManagerInstance.GetvocalmoodInformation();
            vocalmoodList.ItemsSource = vocalmoodInformation;
        }
    }
}
