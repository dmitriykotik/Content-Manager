using Google.Apis.Auth.OAuth2;
using Google.Apis.YouTube.v3;
using System;
using System.IO;
using System.Linq;

namespace ContentManager
{
    internal class Video
    {
        public static string _api;
        public static string _id;

        public static UserCredential _credential;
        public static YouTubeService _youtubeService;

        public static string GetLastVideoID(string channelId)
        {
            try
            {
                var searchListRequest = _youtubeService.Search.List("snippet");
                searchListRequest.ChannelId = channelId;
                searchListRequest.Type = "video";
                searchListRequest.Order = SearchResource.ListRequest.OrderEnum.Date;
                searchListRequest.MaxResults = 1;

                var searchListResponse = searchListRequest.Execute();
                if (searchListResponse.Items != null && searchListResponse.Items.Count > 0)
                {
                    var video = searchListResponse.Items[0];
                    return video.Id.VideoId;
                }
                return null;
            }
            catch { return null; }
        }

        public static string GetLastVideoID()
        {
            try
            {
                var channelsListRequest = _youtubeService.Channels.List("contentDetails");
                channelsListRequest.Mine = true;

                var channelsListResponse = channelsListRequest.Execute();
                if (channelsListResponse.Items != null && channelsListResponse.Items.Count > 0)
                {
                    var uploadsPlaylistId = channelsListResponse.Items[0].ContentDetails.RelatedPlaylists.Uploads;

                    var listRequest = _youtubeService.PlaylistItems.List("snippet,contentDetails");
                    listRequest.PlaylistId = uploadsPlaylistId;
                    listRequest.MaxResults = 50; // Максимальное количество видеороликов для загрузки

                    var listResponse = listRequest.Execute();

                    if (listResponse.Items != null && listResponse.Items.Count > 0)
                    {
                        foreach (var item in listResponse.Items)
                        {
                            var videoId = item.ContentDetails.VideoId;
                            var videoDetailsRequest = _youtubeService.Videos.List("status");
                            videoDetailsRequest.Id = videoId;

                            var videoDetailsResponse = videoDetailsRequest.Execute();

                            if (videoDetailsResponse.Items != null && videoDetailsResponse.Items.Count > 0)
                            {
                                var videoStatus = videoDetailsResponse.Items[0].Status;

                                if (videoStatus.PrivacyStatus == "public" && !videoStatus.UploadStatus.Equals("processing"))
                                {
                                    return videoId;
                                }
                            }
                        }
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }


        public static string GetVideoTitle(string videoId)
        {
            try
            {
                var videoListRequest = _youtubeService.Videos.List("snippet");
                videoListRequest.Id = videoId;

                var videoListResponse = videoListRequest.Execute();
                if (videoListResponse.Items != null && videoListResponse.Items.Count > 0) return videoListResponse.Items[0].Snippet.Title;
                return null;
            }
            catch {  return null; }
        }

        public static long GetVideoViews(string videoId)
        {
            try
            {
                var videoListRequest = _youtubeService.Videos.List("statistics");
                videoListRequest.Id = videoId;

                var videoListResponse = videoListRequest.Execute();
                if (videoListResponse.Items != null && videoListResponse.Items.Count > 0) return (long)(videoListResponse.Items[0].Statistics.ViewCount ?? 0);
                return 0;
            }
            catch { return 0; }
        }

        public static long GetVideoLikes(string videoId)
        {
            try
            {
                var videoListRequest = _youtubeService.Videos.List("statistics");
                videoListRequest.Id = videoId;

                var videoListResponse = videoListRequest.Execute();
                if (videoListResponse.Items != null && videoListResponse.Items.Count > 0) return (long)(videoListResponse.Items[0].Statistics.LikeCount ?? 0);
                return 0;
            }
            catch { return 0; }
        }

        public static long GetVideoDislikes(string videoId)
        {
            try
            {
                var videoListRequest = _youtubeService.Videos.List("statistics");
                videoListRequest.Id = videoId;

                var videoListResponse = videoListRequest.Execute();
                if (videoListResponse.Items != null && videoListResponse.Items.Count > 0) return (long)(videoListResponse.Items[0].Statistics.DislikeCount ?? 0);
                return 0;
            }catch { return 0; }
        }

        public static long GetVideoComments(string videoId)
        {
            try
            {
                var videoListRequest = _youtubeService.Videos.List("statistics");
                videoListRequest.Id = videoId;

                var videoListResponse = videoListRequest.Execute();
                if (videoListResponse.Items != null && videoListResponse.Items.Count > 0) return (long)(videoListResponse.Items[0].Statistics.CommentCount ?? 0);
                return 0;
            }
            catch { return 0; }
        }

        public static DateTime? GetVideoPublish(string videoId)
        {
            try
            {
                var listRequest = _youtubeService.Videos.List("snippet");
                listRequest.Id = videoId;

                var listResponse = listRequest.Execute();
                if (listResponse.Items != null && listResponse.Items.Count > 0)
                {
                    var videoSnippet = listResponse.Items[0].Snippet;
#pragma warning disable CS0618
                    return videoSnippet.PublishedAt;
#pragma warning restore CS0618
                }
                return null;
            }catch { return null; }
        }

        public static string GetVideoPublishTimeLeft(string videoId)
        {
            try
            {
                DateTime? publishedDate = GetVideoPublish(videoId);
                DateTime currentTime = DateTime.Now;
                TimeSpan ts = currentTime - publishedDate.Value;
                return $"{ts.Days} days, {ts.Hours} hours and {ts.Minutes} minutes";
            }catch { return null; }
        }

        public static int GetVideoPublishTimeLeftDays(string videoId)
        {
            try
            {
                DateTime? publishedDate = GetVideoPublish(videoId);
                DateTime currentTime = DateTime.Now;
                TimeSpan ts = currentTime - publishedDate.Value;
                return ts.Days;
            }catch 
            {
                return 0;
            }
        }

        public static int GetVideoPublishTimeLeftHours(string videoId)
        {
            try
            {
                DateTime? publishedDate = GetVideoPublish(videoId);
                DateTime currentTime = DateTime.Now;
                TimeSpan ts = currentTime - publishedDate.Value;
                return ts.Hours;
            }catch
            {
                return 0;
            }
        }

        public static int GetVideoPublishTimeLeftMinutes(string videoId)
        {
            try
            {
                DateTime? publishedDate = GetVideoPublish(videoId);
                DateTime currentTime = DateTime.Now;
                TimeSpan ts = currentTime - publishedDate.Value;
                return ts.Minutes;
            }catch
            {
                return 0;
            }
        }

        public static bool GetCommentsEnable(string videoId)
        {
            try
            {
                var videoListRequest = _youtubeService.Videos.List("statistics");
                videoListRequest.Id = videoId;

                var videoListResponse = videoListRequest.Execute();
                if (videoListResponse.Items != null && videoListResponse.Items.Count > 0)
                {
                    var commentsCount = videoListResponse.Items[0].Statistics.CommentCount;
                    return commentsCount != null && commentsCount > 0;
                }
                return false;
            }
            catch { return false; }
        }

        public static bool GetYTKidsEnable(string videoId)
        {
            try
            {
                var videoListRequest = _youtubeService.Videos.List("snippet");
                videoListRequest.Id = videoId;
                var videoListResponse = videoListRequest.Execute();
                if (videoListResponse.Items != null && videoListResponse.Items.Count > 0)
                {
                    var videoSnippet = videoListResponse.Items[0].Snippet;
                    var videoCategoryId = videoSnippet.CategoryId;
                    if (videoCategoryId == "27") return true;
                }
                return false;
            }
            catch { return false; }
        }
    }
}
