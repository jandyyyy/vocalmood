using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace vocalmood
{
    public class AzureManager
    {

        private static AzureManager instance;
        private MobileServiceClient client;
        private IMobileServiceTable<vocalmoodModel> vocalmoodTable;

        private AzureManager()
        {
            this.client = new MobileServiceClient("http://vocalmood.azurewebsites.net");
            this.vocalmoodTable = this.client.GetTable<vocalmoodModel>();
        }

        public MobileServiceClient AzureClient
        {
            get { return client; }
        }

        public static AzureManager AzureManagerInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AzureManager();
                }

                return instance;
            }
        }

        //GetvocalmoodInformation method creation
        public async Task<List<vocalmoodModel>> GetvocalmoodInformation()
        {
            return await this.vocalmoodTable.ToListAsync();
        }

        public async Task PostvocalmoodInformation(vocalmoodModel vocalmoodModel)
        {
            await this.vocalmoodTable.InsertAsync(vocalmoodModel);
        }

        public async Task UpdatevocalmoodInformation(vocalmoodModel vocalmoodModel)
        {
            await this.vocalmoodTable.UpdateAsync(vocalmoodModel);
        }

        public async Task DeletevocalmoodInformation(vocalmoodModel vocalmoodModel)
        {
            await this.vocalmoodTable.DeleteAsync(vocalmoodModel);
        }
    }
}
