using Google.Apis.Auth.OAuth2;
using Google.Apis.YouTube.v3;
using System;
using System.Text.RegularExpressions;

namespace ContentManager
{
    internal class Functions
    {
        public static string _api;
        public static string _id;

        public static UserCredential _credential;
        public static YouTubeService _youtubeService;

        public static string GetInfoLastVideo()
        {
            Program.channelID = MyChannel.GetChannelIdFromVideo(Video.GetLastVideoID());
            if (Video.GetLastVideoID() == null) return null;
            string Comments = Convert.ToString(Video.GetCommentsEnable(Video.GetLastVideoID()));
            string YouTube_Kids = Convert.ToString(Video.GetYTKidsEnable(Video.GetLastVideoID()));
            if (Comments.ToLower() == "true") { Comments = "Да"; } else { Comments = "Нет"; }
            if (YouTube_Kids.ToLower() == "true") { YouTube_Kids = "Да"; } else { YouTube_Kids = "Нет"; }

            return Comment.GetLastCommentID(Video.GetLastVideoID()) == null
                ? $@"-=- Последний ролик | {Video.GetVideoTitle(Video.GetLastVideoID())} -=-
Айди ролика 🪪: {Video.GetLastVideoID()}
Ссылка на ролик 🔗: https://www.youtube.com/watch?v={Video.GetLastVideoID()}
Имя ролика 🎞️: {Video.GetVideoTitle(Video.GetLastVideoID())}
Видео опубликовано ⌚: {Video.GetVideoPublish(Video.GetLastVideoID())}
С момента публикации прошло ⌛: {Video.GetVideoPublishTimeLeft(Video.GetLastVideoID())}
Кол-во просмотров 👁️: {Video.GetVideoViews(Video.GetLastVideoID())}
Кол-во лайков 👍: {Video.GetVideoLikes(Video.GetLastVideoID())}
Кол-во диз-лайков 👎: {Video.GetVideoDislikes(Video.GetLastVideoID())}
Включены ли на ролике комментарии 💬? {Comments}
Ролик для детей 👶? {YouTube_Kids}
Кол-во комментариев 💬: {Video.GetVideoComments(Video.GetLastVideoID())}
[Введите команду ""сap"", чтобы получить больше подробностей по каналу]
-=-=-=-=-=-=-=-=-=-=-=-"
                : $@"-=- Последний ролик | {Video.GetVideoTitle(Video.GetLastVideoID())} -=-
Айди ролика 🪪: {Video.GetLastVideoID()}
Ссылка на ролик 🔗: https://www.youtube.com/watch?v={Video.GetLastVideoID()}
Имя ролика 🎞️: {Video.GetVideoTitle(Video.GetLastVideoID())}
Видео опубликовано ⌚: {Video.GetVideoPublish(Video.GetLastVideoID())}
С момента публикации прошло ⌛: {Video.GetVideoPublishTimeLeft(Video.GetLastVideoID())}
Кол-во просмотров 👁️: {Video.GetVideoViews(Video.GetLastVideoID())}
Кол-во лайков 👍: {Video.GetVideoLikes(Video.GetLastVideoID())}
Кол-во диз-лайков 👎: {Video.GetVideoDislikes(Video.GetLastVideoID())}
Включены ли на ролике комментарии 💬? {Comments}
Ролик для детей 👶? {YouTube_Kids}
Кол-во комментариев 💬: {Video.GetVideoComments(Video.GetLastVideoID())}
[Введите команду ""сap"", чтобы получить больше подробностей по каналу]

Айди последнего комментария 🪪: {Comment.GetLastCommentID(Video.GetLastVideoID())}
Последний комментарий 💬: ({Comment.GetCommentAuthor(Comment.GetLastCommentID(Video.GetLastVideoID()))}) {Comment.GetCommentPublishDate(Comment.GetLastCommentID(Video.GetLastVideoID()))} - {Comment.GetCommentText(Comment.GetLastCommentID(Video.GetLastVideoID()))} | Кол-во лайков 👍: {Comment.GetCommentLikes(Comment.GetLastCommentID(Video.GetLastVideoID()))}

Айди последнего популярного комментария 🪪: {Comment.GetPopularComment(Video.GetLastVideoID())}
Самый популярный комментарий 💬: ({Comment.GetCommentAuthor(Comment.GetPopularComment(Video.GetLastVideoID()))}) {Comment.GetCommentPublishDate(Comment.GetPopularComment(Video.GetLastVideoID()))} - {Comment.GetCommentText(Comment.GetPopularComment(Video.GetLastVideoID()))} | Кол-во лайков 👍: {Comment.GetCommentLikes(Comment.GetPopularComment(Video.GetLastVideoID()))}
-=-=-=-=-=-=-=-=-=-=-=-";
        }

        public static string GetInfoVideo(string ID)
        {
            
            if (Video.GetVideoPublish(ID) == null)
            {
                if (Video.GetVideoPublish(Functions.GetYouTubeVideoId(ID)) == null)
                {
                    return null;
                }
                else
                {
                    string _Comments = Convert.ToString(Video.GetCommentsEnable(Functions.GetYouTubeVideoId(ID)));
                    string _YouTube_Kids = Convert.ToString(Video.GetYTKidsEnable(Functions.GetYouTubeVideoId(ID)));
                    if (_Comments.ToLower() == "true") { _Comments = "Да"; } else { _Comments = "Нет"; }
                    if (_YouTube_Kids.ToLower() == "true") { _YouTube_Kids = "Да"; } else { _YouTube_Kids = "Нет"; }
                    Program.channelID = MyChannel.GetChannelIdFromVideo(Functions.GetYouTubeVideoId(ID));
                    return Comment.GetLastCommentID(Functions.GetYouTubeVideoId(ID)) == null
                ? $@"-=- Информация по видео: | {Video.GetVideoTitle(Functions.GetYouTubeVideoId(ID))} -=-
Айди ролика 🪪: {Functions.GetYouTubeVideoId(ID)}
Ссылка на ролик 🔗: https://www.youtube.com/watch?v={Functions.GetYouTubeVideoId(ID)}
Имя ролика 🎞️: {Video.GetVideoTitle(Functions.GetYouTubeVideoId(ID))}
Видео опубликовано ⌚: {Video.GetVideoPublish(Functions.GetYouTubeVideoId(ID))}
С момента публикации прошло ⌛: {Video.GetVideoPublishTimeLeft(Functions.GetYouTubeVideoId(ID))}
Кол-во просмотров 👁️: {Video.GetVideoViews(Functions.GetYouTubeVideoId(ID))}
Кол-во лайков 👍: {Video.GetVideoLikes(Functions.GetYouTubeVideoId(ID))}
Кол-во диз-лайков 👎: {Video.GetVideoDislikes(Functions.GetYouTubeVideoId(ID))}
Включены ли на ролике комментарии 💬? {_Comments}
Ролик для детей 👶? {_YouTube_Kids}
Кол-во комментариев 💬: {Video.GetVideoComments(Functions.GetYouTubeVideoId(ID))}
[Введите команду ""сap"", чтобы получить больше подробностей по каналу]
-=-=-=-=-=-=-=-=-=-=-=-"
                : $@"-=- Информация по видео: | {Video.GetVideoTitle(Functions.GetYouTubeVideoId(ID))} -=-
Айди ролика 🪪: {Functions.GetYouTubeVideoId(ID)}
Ссылка на ролик 🔗: https://www.youtube.com/watch?v={Functions.GetYouTubeVideoId(ID)}
Имя ролика 🎞️: {Video.GetVideoTitle(Functions.GetYouTubeVideoId(ID))}
Видео опубликовано ⌚: {Video.GetVideoPublish(Functions.GetYouTubeVideoId(ID))}
С момента публикации прошло ⌛: {Video.GetVideoPublishTimeLeft(Functions.GetYouTubeVideoId(ID))}
Кол-во просмотров 👁️: {Video.GetVideoViews(Functions.GetYouTubeVideoId(ID))}
Кол-во лайков 👍: {Video.GetVideoLikes(Functions.GetYouTubeVideoId(ID))}
Кол-во диз-лайков 👎: {Video.GetVideoDislikes(Functions.GetYouTubeVideoId(ID))}
Включены ли на ролике комментарии 💬? {_Comments}
Ролик для детей 👶? {_YouTube_Kids}
Кол-во комментариев 💬: {Video.GetVideoComments(Functions.GetYouTubeVideoId(ID))}
[Введите команду ""сap"", чтобы получить больше подробностей по каналу]

Айди последнего комментария 🪪: {Comment.GetLastCommentID(Functions.GetYouTubeVideoId(ID))}
Последний комментарий 💬: ({Comment.GetCommentAuthor(Comment.GetLastCommentID(Functions.GetYouTubeVideoId(ID)))}) {Comment.GetCommentPublishDate(Comment.GetLastCommentID(Functions.GetYouTubeVideoId(ID)))} - {Comment.GetCommentText(Comment.GetLastCommentID(Functions.GetYouTubeVideoId(ID)))} | Кол-во лайков 👍: {Comment.GetCommentLikes(Comment.GetLastCommentID(Functions.GetYouTubeVideoId(ID)))}

Айди последнего популярного комментария 🪪: {Comment.GetPopularComment(Functions.GetYouTubeVideoId(ID))}
Самый популярный комментарий 💬: ({Comment.GetCommentAuthor(Comment.GetPopularComment(Functions.GetYouTubeVideoId(ID)))}) {Comment.GetCommentPublishDate(Comment.GetPopularComment(Functions.GetYouTubeVideoId(ID)))} - {Comment.GetCommentText(Comment.GetPopularComment(Functions.GetYouTubeVideoId(ID)))} | Кол-во лайков 👍: {Comment.GetCommentLikes(Comment.GetPopularComment(Functions.GetYouTubeVideoId(ID)))}
-=-=-=-=-=-=-=-=-=-=-=-";
                }
            }
            Program.channelID = MyChannel.GetChannelIdFromVideo(ID);
            string Comments = Convert.ToString(Video.GetCommentsEnable(ID));
            string YouTube_Kids = Convert.ToString(Video.GetYTKidsEnable(ID));
            if (Comments.ToLower() == "true") { Comments = "Да"; } else { Comments = "Нет"; }
            if (YouTube_Kids.ToLower() == "true") { YouTube_Kids = "Да"; } else { YouTube_Kids = "Нет"; }
            return Comment.GetLastCommentID(ID) == null
                ? $@"-=- Информация по видео: | {Video.GetVideoTitle(ID)} -=-
Айди ролика 🪪: {ID}
Ссылка на ролик 🔗: https://www.youtube.com/watch?v={ID}
Имя ролика 🎞️: {Video.GetVideoTitle(ID)}
Видео опубликовано ⌚: {Video.GetVideoPublish(ID)}
С момента публикации прошло ⌛: {Video.GetVideoPublishTimeLeft(ID)}
Кол-во просмотров 👁️: {Video.GetVideoViews(ID)}
Кол-во лайков 👍: {Video.GetVideoLikes(ID)}
Кол-во ди-лайков 👎: {Video.GetVideoDislikes(ID)}
Включены ли на ролике комментарии 💬? {Comments}
Ролик для детей 👶? {YouTube_Kids}
Кол-во комментариев 💬: {Video.GetVideoComments(ID)}
[Введите команду ""сap"", чтобы получить больше подробностей по каналу]
-=-=-=-=-=-=-=-=-=-=-=-"
                : $@"-=- Информация по видео: | {Video.GetVideoTitle(ID)} -=-
Айди ролика 🪪: {ID}
Ссылка на ролик 🔗: https://www.youtube.com/watch?v={ID}
Имя ролика 🎞️: {Video.GetVideoTitle(ID)}
Видео опубликовано ⌚: {Video.GetVideoPublish(ID)}
С момента публикации прошло ⌛: {Video.GetVideoPublishTimeLeft(ID)}
Кол-во просмотров 👁️: {Video.GetVideoViews(ID)}
Кол-во лайков 👍: {Video.GetVideoLikes(ID)}
Кол-во ди-лайков 👎: {Video.GetVideoDislikes(ID)}
Включены ли на ролике комментарии 💬? {Comments}
Ролик для детей 👶? {YouTube_Kids}
Кол-во комментариев 💬: {Video.GetVideoComments(ID)}
[Введите команду ""сap"", чтобы получить больше подробностей по каналу]

Айди последнего комментария 🪪: {Comment.GetLastCommentID(ID)}
Последний комментарий 💬: ({Comment.GetCommentAuthor(Comment.GetLastCommentID(ID))}) {Comment.GetCommentPublishDate(Comment.GetLastCommentID(ID))} - {Comment.GetCommentText(Comment.GetLastCommentID(ID))} | Кол-во лайков 👍: {Comment.GetCommentLikes(Comment.GetLastCommentID(ID))}

Айди последнего популярного комментария 🪪: {Comment.GetPopularComment(ID)}
Самый популярный комментарий 💬: ({Comment.GetCommentAuthor(Comment.GetPopularComment(ID))}) {Comment.GetCommentPublishDate(Comment.GetPopularComment(ID))} - {Comment.GetCommentText(Comment.GetPopularComment(ID))} | Кол-во лайков 👍: {Comment.GetCommentLikes(Comment.GetPopularComment(ID))}
-=-=-=-=-=-=-=-=-=-=-=-";
        }

        public static string GetInfoMyChannel()
        {
            try
            {
                _id = MyChannel.GetMyChannelId();
                Program.videoID = Video.GetLastVideoID();
                return $@"
{MyChannel.GetChannelName(_id)}, вот, что мне удалось найти :D
Айди канала: {_id}
Имя канала: {MyChannel.GetChannelName(_id)}
Общее кол-во просмотров на канале: {MyChannel.GetChannelViews(_id)}
Кол-во подписчиков: {MyChannel.GetChannelSubscribers(_id)}
Имя последнего ролика: {Video.GetVideoTitle(Video.GetLastVideoID())}
| Кол-во просмотров: {Video.GetVideoViews(Video.GetLastVideoID())}
| Он был выпущен {Video.GetVideoPublishTimeLeft(Video.GetLastVideoID())} назад
| [Введите команду ""tap"", чтобы получить больше подробностей по ролику]
Статус с последнего ролика: {Functions.GetScoreTimeLeftLastVideoPublish()}
";
            }
            catch
            {
                return null;
            }
        }

        public static string GetInfoChannel(string channelId)
        {
            try
            {
                if (MyChannel.GetChannelCreate(MyChannel.GetChannelId(channelId)) == null)
                {
                    if (MyChannel.GetChannelCreate(channelId) == null)
                    {
                        return null;
                    }
                    else
                    {
                        string res = channelId;
                        if (Video.GetVideoPublishTimeLeft(Video.GetLastVideoID(res)) == null) return $@"
Айди канала: {res}
Имя канала: {MyChannel.GetChannelName(res)}
Дата создания канала: {MyChannel.GetChannelCreate(res)}
Общее кол-во просмотров на канале: {MyChannel.GetChannelViews(res)}
Кол-во подписчиков: {MyChannel.GetChannelSubscribers(res)}
";
                        Program.videoID = Video.GetLastVideoID(res);
                        return $@"
Айди канала: {res}
Имя канала: {MyChannel.GetChannelName(res)}
Дата создания канала: {MyChannel.GetChannelCreate(res)}
Общее кол-во просмотров на канале: {MyChannel.GetChannelViews(res)}
Кол-во подписчиков: {MyChannel.GetChannelSubscribers(res)}
Имя последнего ролика: {Video.GetVideoTitle(Video.GetLastVideoID(res))}
| Кол-во просмотров: {Video.GetVideoViews(Video.GetLastVideoID(res))}
| Он был выпущен {Video.GetVideoPublishTimeLeft(Video.GetLastVideoID(res))} назад
| [Введите команду ""tap"", чтобы получить больше подробностей по ролику]
";
                    }
                }
                else
                {
                    string res = MyChannel.GetChannelId(channelId);
                    if (Video.GetVideoPublishTimeLeft(Video.GetLastVideoID(res)) == null) return $@"
Айди канала: {res}
Имя канала: {MyChannel.GetChannelName(res)}
Дата создания канала: {MyChannel.GetChannelCreate(res)}
Общее кол-во просмотров на канале: {MyChannel.GetChannelViews(res)}
Кол-во подписчиков: {MyChannel.GetChannelSubscribers(res)}
";
                    Program.videoID = Video.GetLastVideoID(res);
                    return $@"
Айди канала: {res}
Имя канала: {MyChannel.GetChannelName(res)}
Дата создания канала: {MyChannel.GetChannelCreate(res)}
Общее кол-во просмотров на канале: {MyChannel.GetChannelViews(res)}
Кол-во подписчиков: {MyChannel.GetChannelSubscribers(res)}
Имя последнего ролика: {Video.GetVideoTitle(Video.GetLastVideoID(res))}
| Кол-во просмотров: {Video.GetVideoViews(Video.GetLastVideoID(res))}
| Он был выпущен {Video.GetVideoPublishTimeLeft(Video.GetLastVideoID(res))} назад
| [Введите команду ""tap"", чтобы получить больше подробностей по ролику]
";
                }
            }
            catch
            {
                return null;
            }
        }

        public static string GetYouTubeVideoId(string url)
        {
            string pattern = @"(?:\?v=|&v=|/v/|youtu\.be/)([a-zA-Z0-9_-]{11})";
            Regex regex = new Regex(pattern);
            Match match = regex.Match(url);

            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            else
            {
                return null;
            }
        }

        public static string GetScoreTimeLeftLastVideoPublish()
        {
            if (Video.GetVideoPublishTimeLeftDays(Video.GetLastVideoID()) < 7)
            {
                return "Видео ещё свеженькое";
            }
            else if (Video.GetVideoPublishTimeLeftDays(Video.GetLastVideoID()) < 14)
            {
                return "Пора выпускать новый ролик";
            }
            else if (Video.GetVideoPublishTimeLeftDays(Video.GetLastVideoID()) < 16)
            {
                return "Статистика скоро начнёт падать";
            }
            else if (Video.GetVideoPublishTimeLeftDays(Video.GetLastVideoID()) < 30)
            {
                return "Слишком долго нету роликов, нужно начинать делать, уже статистика падает";
            }
            else if (Video.GetVideoPublishTimeLeftDays(Video.GetLastVideoID()) > 30)
            {
                return "Канал умирает!";
            }
            else
            {
                return "Ой, что-то странное происходит тут...";
            }
        }
    }
}
