using GoogleDriveIntegration.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveIntegration.Helper
{
    public class ElasticsearchRepository
    {
        static ElasticsearchHelper esHelper = new ElasticsearchHelper();
        static ElasticClient client = esHelper.GetElasticsearchClient();
        public static string knowhowDriveIndex = "drive";

        public static Response<bool> insertItem(DriveItemModel item, string indexName, string dataID)
        {
            try
            {
                var resp = client.Index<DriveItemModel>(item, x => x.Index(indexName).Id(dataID));
                return new Response<bool>() { Success = resp.IsValid, Message = "Done" };

            }
            catch (Exception ex)
            {
                return new Response<bool>() { Success = false, Message = ex?.Message + " | " + ex?.StackTrace };

            }
        }

        public static Response<List<DriveItemModel>> getDriveFiles(int size = 20)
        {
            try
            {
                var response = client.Search<DriveItemModel>(q => q.Index(knowhowDriveIndex).Query(rq => rq.MatchAll()).Size(size));

                return new Response<List<DriveItemModel>>() { Success = response.IsValid, Message = "Done", Data = response.Documents.ToList() };

            }
            catch (Exception ex)
            {
                return new Response<List<DriveItemModel>>() { Success = false, Message = ex?.Message + " | " + ex?.StackTrace };
            }

        }

        public static Response<List<DriveItemModel>> SearchKeyword(string keyword, int size = 20)
        {
            try
            {
                /*
                * Searches Elasticsearch to match files and their content. Allows up to 5 words
                * Partial word matches areallowed. It will match "Elasticsearch data" with: elasti dat
                */
                string first_word = "";
                string second_word = "";
                string third_word = "";
                string fourth_word = "";
                string fifth_word = "";

                string[] words = keyword.ToLower().Split(null);
                first_word = words.ElementAtOrDefault(0);
                second_word = words.ElementAtOrDefault(1);
                third_word = words.ElementAtOrDefault(2);
                fourth_word = words.ElementAtOrDefault(3);
                fifth_word = words.ElementAtOrDefault(4);





                var resp = client.Search<DriveItemModel>(s => s
                    .Index("drive")
                        .Query(q => q.Bool(b => b.Should(
                                                         m => m.Regexp(t => t.Field("title").Value(".*" + first_word + ".*")) ||
                                                         m.Regexp(tq => tq.Field("content").Value(".*" + first_word + ".*")) ||

                                                         (!string.IsNullOrEmpty(second_word) ? m.Regexp(t => t.Field("title").Value(".*" + second_word + ".*")) : null) ||
                                                         (!string.IsNullOrEmpty(second_word) ? m.Regexp(t => t.Field("content").Value(".*" + second_word + ".*")) : null) ||
                                                         (!string.IsNullOrEmpty(third_word) ? m.Regexp(t => t.Field("title").Value(".*" + third_word + ".*")) : null) ||
                                                         (!string.IsNullOrEmpty(third_word) ? m.Regexp(t => t.Field("content").Value(".*" + third_word + ".*")) : null) ||
                                                         (!string.IsNullOrEmpty(fourth_word) ? m.Regexp(t => t.Field("title").Value(".*" + fourth_word + ".*")) : null) ||
                                                         (!string.IsNullOrEmpty(fourth_word) ? m.Regexp(t => t.Field("content").Value(".*" + fourth_word + ".*")) : null) ||
                                                         (!string.IsNullOrEmpty(fifth_word) ? m.Regexp(t => t.Field("title").Value(".*" + fifth_word + ".*")) : null) ||
                                                         (!string.IsNullOrEmpty(fifth_word) ? m.Regexp(t => t.Field("content").Value(".*" + fifth_word + ".*")) : null)


                                                        ))));


                return new Response<List<DriveItemModel>>() { Success = resp.IsValid, Message = "Done", Data = resp.Documents.ToList() };

            }
            catch (Exception ex)
            {
                return new Response<List<DriveItemModel>>() { Success = false, Message = ex?.Message + " | " + ex?.StackTrace };
            }

        }
        public static Response<bool> deleteByQuery(string indexName)
        {
            try
            {
                var response = client.DeleteByQuery<DriveItemModel>(q => q.Index(indexName)
                .Query(rq => rq
                    .MatchAll())
            );
                return new Response<bool>() { Success = response.IsValid, Message = "Done" };
            }
            catch (Exception ex)
            {
                return new Response<bool>() { Success = false, Message = ex?.Message + " | " + ex?.StackTrace };
            }

        }
    }


}
