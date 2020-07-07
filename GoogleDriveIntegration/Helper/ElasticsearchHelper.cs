using Nest;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveIntegration.Helper
{
    public class ElasticsearchHelper
    {
        private static string conn = ConfigurationManager.AppSettings["ES_URL"];


        static ConnectionSettings connectionSettings;
        ElasticClient client;

        public ElasticsearchHelper()
        {
            connectionSettings = new ConnectionSettings(new Uri(conn)).DisableDirectStreaming();
#if debug
                connectionSettings.EnableDebugMode();
#endif
            client = new ElasticClient(connectionSettings);
        }
        public ElasticClient GetElasticsearchClient()
        {
            return client;
        }


    }
}
