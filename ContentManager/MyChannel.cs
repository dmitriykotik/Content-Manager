using Google.Apis.Auth.OAuth2;
using Google.Apis.YouTube.v3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;

namespace ContentManager
{
    internal class MyChannel
    {
        public static Dictionary<int, string> videoIds = new Dictionary<int, string>();
        public static string _api;
        public static string _id;
        public static UserCredential _credential;
        public static YouTubeService _youtubeService;

        public static string GetChannelId(string channelLink)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = client.GetAsync(channelLink).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string htmlCode = response.Content.ReadAsStringAsync().Result;
                        string pattern = @"<meta\s*property=""og:url""\s*content=""(.*?)""";
                        Match match = Regex.Match(htmlCode, pattern);

                        if (match.Success)
                        {
                            string channelUrl = match.Groups[1].Value;
                            Uri uri = new Uri(channelUrl);
                            string path = uri.AbsolutePath;
                            string[] segments = path.Trim('/').Split('/');
                            return segments[segments.Length - 1];
                        }
                        else return null;
                    }
                    else return null;
                }
                catch { return null; }
            }
        }
        public static Dictionary<int, string> GetAllVideosChannel(string channelId)
        {
            try
            {
                var listRequest = _youtubeService.Search.List("snippet");
                listRequest.ChannelId = channelId;
                listRequest.Type = "video";
                listRequest.MaxResults = 50; // Максимальное количество видеороликов для загрузки
                listRequest.Order = SearchResource.ListRequest.OrderEnum.Date; // Сортировка по дате
                listRequest.PublishedAfter = new DateTime(1970, 1, 1); // Получить все видео

                var listResponse = listRequest.Execute();

                if (listResponse.Items != null && listResponse.Items.Count > 0)
                {
                    int count = 1;

                    for (int i = listResponse.Items.Count - 1; i >= 0; i--)
                    {
                        var item = listResponse.Items[i];
                        videoIds[count] = item.Id.VideoId;
                        Console.WriteLine($"{count}) {item.Snippet.Title} - {item.Id.VideoId}");
                        count++;
                    }
                }
            }
            catch
            {
                Console.WriteLine("Ошибка при получении видеороликов на канале.");
            }
            return videoIds;
        }

        public static string GetChannelIdFromVideo(string videoId)
        {
            try
            {
                VideosResource.ListRequest listRequest = _youtubeService.Videos.List("snippet");
                listRequest.Id = videoId;
                var videoListResponse = listRequest.Execute();
                if (videoListResponse.Items != null && videoListResponse.Items.Count > 0)
                {
                    var video = videoListResponse.Items[0];
                    return video.Snippet.ChannelId;
                }
            }
            catch { return null; }
            return null;
        }

        public static string GetMyChannelId()
        {
            try
            {
                ChannelsResource.ListRequest listRequest = _youtubeService.Channels.List("id");
                listRequest.Mine = true;

                var channelsListResponse = listRequest.Execute();
                if (channelsListResponse.Items != null && channelsListResponse.Items.Count > 0)
                {
                    var channel = channelsListResponse.Items[0];
                    return channel.Id;
                }
                return null;
            }
            catch { return null; }
        }

        public static string GetChannelName(string channelId)
        {
            try
            {
                ChannelsResource.ListRequest listRequest = _youtubeService.Channels.List("snippet");
                listRequest.Id = channelId;

                var channelsListResponse = listRequest.Execute();
                if (channelsListResponse.Items != null && channelsListResponse.Items.Count > 0)
                {
                    var channel = channelsListResponse.Items[0];
                    return channel.Snippet.Title;
                }
                return null;
            }
            catch { return null; }
        }

        public static long GetChannelSubscribers(string channelId)
        {
            try
            {
                ChannelsResource.ListRequest listRequest = _youtubeService.Channels.List("statistics");
                listRequest.Id = channelId;

                var channelsListResponse = listRequest.Execute();
                if (channelsListResponse.Items != null && channelsListResponse.Items.Count > 0)
                {
                    var channel = channelsListResponse.Items[0];
                    return (long)(channel.Statistics.SubscriberCount ?? 0);
                }
                return 0;
            }
            catch { return 0; }
        }

        public static long GetChannelViews(string channelId)
        {
            try
            {
                ChannelsResource.ListRequest listRequest = _youtubeService.Channels.List("statistics");
                listRequest.Id = channelId;

                var channelsListResponse = listRequest.Execute();
                if (channelsListResponse.Items != null && channelsListResponse.Items.Count > 0)
                {
                    var channel = channelsListResponse.Items[0];
                    return (long)(channel.Statistics.ViewCount ?? 0);
                }
                return 0;
            }
            catch { return 0; }
        }

        public static DateTime? GetChannelCreate(string channelId)
        {
            try
            {
                var listRequest = _youtubeService.Channels.List("snippet");
                listRequest.Id = channelId;

                var listResponse = listRequest.Execute();
                if (listResponse.Items != null && listResponse.Items.Count > 0)
                {
                    var channelSnippet = listResponse.Items[0].Snippet;
#pragma warning disable CS0618
                    return channelSnippet.PublishedAt;
#pragma warning restore CS0618
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public static void RevokeAccess()
        {
            Console.WriteLine("Вы уверенны, что хотите выйти из аккаунта? (y/n)");
            if (Console.ReadLine().ToLower() == "y")
            {
                try
                {
                    _credential.RevokeTokenAsync(CancellationToken.None).Wait();
                    Console.WriteLine(@"Выход успешно выполнен!
Press any key to exit...");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Выход отменён");
                }
            }
            else Console.WriteLine("Выход отменён");
        }
    }
}
