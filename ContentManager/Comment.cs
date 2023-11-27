using Google.Apis.Auth.OAuth2;
using Google.Apis.YouTube.v3;
using System;
using System.IO;

namespace ContentManager
{
    internal class Comment
    {
        public static string _api;
        public static string _id;

        public static UserCredential _credential;
        public static YouTubeService _youtubeService;

        public static string GetLastCommentID(string videoId)
        {
            try
            {
                var commentThreadsListRequest = _youtubeService.CommentThreads.List("snippet");
                commentThreadsListRequest.VideoId = videoId;
                commentThreadsListRequest.MaxResults = 1;
                commentThreadsListRequest.Order = CommentThreadsResource.ListRequest.OrderEnum.Time;

                var commentThreadsListResponse = commentThreadsListRequest.Execute();
                if (commentThreadsListResponse.Items != null && commentThreadsListResponse.Items.Count > 0) return commentThreadsListResponse.Items[0].Id;
                return null;
            }
            catch { return null; }
        }

        public static string GetCommentText(string commentId)
        {
            try
            {
                var commentThreadsListRequest = _youtubeService.CommentThreads.List("snippet");
                commentThreadsListRequest.Id = commentId;

                var commentThreadsListResponse = commentThreadsListRequest.Execute();
                if (commentThreadsListResponse.Items != null && commentThreadsListResponse.Items.Count > 0) return commentThreadsListResponse.Items[0].Snippet.TopLevelComment.Snippet.TextDisplay;
                return null;
            }
            catch { return null; }
        }

        public static string GetCommentAuthor(string commentId)
        {
            try
            {
                var commentThreadsListRequest = _youtubeService.CommentThreads.List("snippet");
                commentThreadsListRequest.Id = commentId;

                var commentThreadsListResponse = commentThreadsListRequest.Execute();
                if (commentThreadsListResponse.Items != null && commentThreadsListResponse.Items.Count > 0) return commentThreadsListResponse.Items[0].Snippet.TopLevelComment.Snippet.AuthorDisplayName;
                return null;
            }catch { return null; }
        }

        public static DateTime? GetCommentPublishDate(string commentId)
        {
            try
            {
                var commentThreadsListRequest = _youtubeService.CommentThreads.List("snippet");
                commentThreadsListRequest.Id = commentId;

                var commentThreadsListResponse = commentThreadsListRequest.Execute();
                if (commentThreadsListResponse.Items != null && commentThreadsListResponse.Items.Count > 0)
                #pragma warning disable CS0618
                return commentThreadsListResponse.Items[0].Snippet.TopLevelComment.Snippet.PublishedAt; 
                #pragma warning restore CS0618
                return null;
            }catch { return null; }
        }
        public static int GetCommentLikes(string commentId)
        {
            try
            {
                var commentRequest = _youtubeService.CommentThreads.List("snippet");
                commentRequest.Id = commentId;

                var commentResponse = commentRequest.Execute();
                if (commentResponse.Items != null && commentResponse.Items.Count > 0) return (int)commentResponse.Items[0].Snippet.TopLevelComment.Snippet.LikeCount;
                return 0;
            }catch { return 0; }
        }

        public static string GetPopularComment(string videoId)
        {
            try
            {
                var commentThreadsListRequest = _youtubeService.CommentThreads.List("snippet");
                commentThreadsListRequest.VideoId = videoId;
                commentThreadsListRequest.MaxResults = 1;
                commentThreadsListRequest.Order = CommentThreadsResource.ListRequest.OrderEnum.Relevance;

                var commentThreadsListResponse = commentThreadsListRequest.Execute();
                if (commentThreadsListResponse.Items != null && commentThreadsListResponse.Items.Count > 0) return commentThreadsListResponse.Items[0].Id;
                return null;
            }catch { return null;  }
        }
    }
}
