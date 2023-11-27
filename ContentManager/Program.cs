using System;
using System.IO;
using System.Linq;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;

namespace ContentManager
{
    internal class Program
    {
        #region Assembly Info
        public const string AssemblyTitle = "Content Manager";
        public const string AssemblyProduct = "Content Manager";
        public const string AssemblyDescription = "Content Manager - Позволяет анализировать каналы/видео и т.д.";
        public const string AssemblyCompany = "MultiPlayer";
        public const string AssemblyCopyright = "Copyright © MultiPlayer 2019-2023";
        public const string AssemblyTrademark = "MultiPlayer";

        public const string AssemblyVersion = "1.2.44.3";
        public const string AssemblyFileVersion = "1.2.44.3";
        #endregion

        const bool alphaTest = true;

        const string _api = "AIzaSyBYdWgiFndIPSSFn-lAQexR1TbS_30d6Ig";
        static string _id;

        public static UserCredential _credential;
        public static YouTubeService _youtubeService;

        const string dev_func_name = "dev_func";

        public static string videoID;
        public static string channelID;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Menu(args);
        }

        static void Menu(string[] args)
        {
            bool OTAE = args.Any(arg => arg == "-onlyterminal");
            Initializate();
            Console.Clear();
            _id = MyChannel.GetMyChannelId();
            if (OTAE == false)
            {
                Console.WriteLine(MyChannel.GetChannelName(_id) + ", пожалуйста, ожидай подгрузки статистики...");
                try
                {
                    Console.WriteLine($@"Привет, {MyChannel.GetChannelName(_id)}!
Текущее кол-во подписчиков: {MyChannel.GetChannelSubscribers(_id)}
Общее кол-во просмотров: {MyChannel.GetChannelViews(_id)}

{Functions.GetInfoLastVideo()}
");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n[EXCEPTION]\nMessage: {ex.Message}\n-=-=-\nStack Trace: {ex.StackTrace}\n-=-=-\nTarget Site: {ex.TargetSite}\n-=-=-\nSource: {ex.Source}\n[END]");
                }
            }
            else
            {
                Console.WriteLine(MyChannel.GetChannelName(_id) + ", пожалуйста, ожидай...");
            }

            while (true)
            {
                Console.Write("> ");
                string[] input = Console.ReadLine().Split(' ');
                switch (input[0])
                {
                    case "info":
                        if (input.Length == 2)
                        {
                            if (input[1].ToLower() == "mychannel")
                            {
                                if (Functions.GetInfoMyChannel() == null)
                                {
                                    Console.WriteLine("Нам не удалось получить информацию о вашем канале!");
                                }
                                Console.WriteLine($@"
{Functions.GetInfoMyChannel()}
");
                            }
                            else
                            {
                                Console.WriteLine(MyChannel.GetChannelName(_id) + ", пожалуйста, ожидай...");
                                try
                                {
                                    if (Functions.GetInfoChannel(input[1]) == null)
                                    {
                                        if (Functions.GetInfoVideo(input[1]) == null)
                                        {

                                            Console.WriteLine("Этого ролика/канала не существует или нам в данный момент не удалось получить данные о этом ролике/канале.");
                                            break;
                                        }
                                        Console.WriteLine($@"
{Functions.GetInfoVideo(input[1])}
");
                                        break;
                                    }
                                    Console.WriteLine($@"
{Functions.GetInfoChannel(input[1])}
");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"\n[EXCEPTION]\nMessage: {ex.Message}\n-=-=-\n[END]");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Не достаточно аргументов!");
                        }
                        break;
                    case "tap":
                        if (input.Length >= 2)
                        {
                            if (input[1].ToLower() == "clear")
                            {
                                if (string.IsNullOrEmpty(videoID))
                                {
                                    Console.WriteLine("Буфер уже чист!");
                                }
                                else
                                {
                                    Console.WriteLine("Вы уверенны, что хотите принудительно очистить буфер? (y/n)");
                                    if (Console.ReadLine().ToLower() == "y")
                                    {
                                        videoID = null;
                                        Console.WriteLine("Буфер очищен!");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Очистка буфера приостановлена пользователем!");
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("Неизвестный аргумент!");
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(videoID))
                            {
                                Console.WriteLine("В буфере подозрительно чисто, нужно проверять каналы");
                            }
                            else
                            {
                                Console.WriteLine(MyChannel.GetChannelName(_id) + ", пожалуйста, ожидай...");
                                try
                                {
                                    Console.WriteLine(Functions.GetInfoVideo(videoID));
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"\n[EXCEPTION]\nMessage: {ex.Message}\n-=-=-\n[END]");
                                }
                            }
                        }
                        break;
                    case "cap":
                        if (input.Length >= 2)
                        {
                            if (input[1].ToLower() == "clear")
                            {
                                if (string.IsNullOrEmpty(channelID))
                                {
                                    Console.WriteLine("Буфер уже чист!");
                                }
                                else
                                {
                                    Console.WriteLine("Вы уверенны, что хотите принудительно очистить буфер? (y/n)");
                                    if (Console.ReadLine().ToLower() == "y")
                                    {
                                        channelID = null;
                                        Console.WriteLine("Буфер очищен!");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Очистка буфера приостановлена пользователем!");
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("Неизвестный аргумент!");
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(channelID))
                            {
                                Console.WriteLine("В буфере подозрительно чисто, нужно проверять ролики");
                            }
                            else
                            {
                                Console.WriteLine(MyChannel.GetChannelName(_id) + ", пожалуйста, ожидай...");
                                try
                                {
                                    Console.WriteLine(Functions.GetInfoChannel(channelID));
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"\n[EXCEPTION]\nMessage: {ex.Message}\n-=-=-\n[END]");
                                }
                            }
                        }
                        break;
                    case "lap":
                        if (input.Length == 2)
                        {
                            if (input[1] == "clear")
                            {
                                MyChannel.videoIds = null;
                            }
                            if (MyChannel.videoIds.Count > 0)
                            {
                                try
                                {
                                    Convert.ToInt32(input[1]);
                                }
                                catch
                                {
                                    Console.WriteLine("Введите число!");
                                    break;
                                }
                                try
                                {
                                    if (MyChannel.videoIds.Count >= Convert.ToInt32(input[1]))
                                    {
                                        string id = MyChannel.videoIds[Convert.ToInt32(input[1])];
                                        Console.WriteLine(MyChannel.GetChannelName(MyChannel.GetMyChannelId()) + ", пожалуйста, подожди...");
                                        Console.WriteLine(Functions.GetInfoVideo(id));
                                    }
                                    else
                                    {
                                        Console.WriteLine("Не удалось найти ролик с таким порядковым номером!");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                    break;
                                }
                            }
                            else
                            {
                                Console.WriteLine("В буфере подозрительно чисто, нужно проверять списки роликов");
                                break;
                            }
                            
                        }
                        else
                        {
                            Console.WriteLine("Недостаточно аргументов!");
                        }
                        break;
                    case "help":
                        Console.WriteLine($@"
info (video id or link; channel id or link) - Анализирует ролик или канал
tap [clear] - Анализирует ролик из буфера или очищает буфер
cap [clear] - Анализирует канал из буфера или очищает буфер
lap (number or arg clear) - Анализирует видео по порядковому номеру, после команды list или очищает буфер
list (channel id or link) - Выдаёт список роликов на канале по его айди или ссылке, а также записывает в буфер для получения подробной информации
version - Информация о программе
revoke - Выход из аккаунта гугл в программе
clear - Очищает терминал
");
                        break;
                    case "clear":
                        Console.Clear();
                        break;
                    case "list":
                        if (input.Length >= 2)
                        {
                            if (MyChannel.GetChannelCreate(MyChannel.GetChannelId(input[1])) == null)
                            {
                                if (MyChannel.GetChannelCreate(input[1]) == null)
                                {
                                    Console.WriteLine("Не удалось получить список! Возможно, не верный айди или ссылка на канал.");
                                    break;
                                }
                                else
                                {
                                    MyChannel.GetAllVideosChannel(input[1]);
                                }
                            }
                            else
                            {
                                MyChannel.GetAllVideosChannel(MyChannel.GetChannelId(input[1]));
                            }
                        }
                        else
                        {
                            Console.WriteLine("Не достаточно аргументов!");
                        }
                        break;
                    case dev_func_name:
#pragma warning disable CS0162
                        if (alphaTest)
                        {
                            if (input.Length >= 2)
                            {
                                
                            }

                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($@"В данный момент команда ""{dev_func_name}"" недоступна!");
                            Console.ResetColor();
                        }
#pragma warning restore CS0162
                        break;
                    case "live":
                        Console.WriteLine(MyChannel.GetChannelName(_id) + ", ты уверен, что хочешь войти в Live-режим? Больше из него выйти нельзя будет! (Только с помощью перезапуска програмы) (y/n)");
                        if (Console.ReadLine().ToLower() == "y")
                        {
                            long s;
                            long v;
                            long lv;
                            string channelName;

                            long ts;
                            long tv;
                            channelName = MyChannel.GetChannelName(_id);
                            s = MyChannel.GetChannelSubscribers(_id);
                            v = MyChannel.GetChannelViews(_id);
                            lv = Video.GetVideoViews(Video.GetLastVideoID());

                            string lvI = Video.GetLastVideoID();
                            string lvT = Video.GetVideoTitle(lvI);
                            long lvV = Video.GetVideoViews(lvI);
                            long lvL = Video.GetVideoLikes(lvI);
                            long lvD = Video.GetVideoDislikes(lvI);

                            long vV;
                            long vL;
                            long vD;

                            Thread t = new Thread(() => 
                            {
                                if (lvI == null)
                                {
                                    while (true)
                                    {
                                        ts = MyChannel.GetChannelSubscribers(_id);
                                        tv = MyChannel.GetChannelViews(_id);
                                        Console.Clear();
                                        if (ts >= s)
                                        {

                                            Console.WriteLine($@"== LIVE ==============================================
{channelName}
Подписчики: {ts} +{ts - s}
Просмотры: {tv} +{tv - v}");
                                        }
                                        else
                                        {
                                            Console.WriteLine($@"== LIVE ==============================================
{channelName}
Подписчики: {ts} {ts - s}
Просмотры: {tv} +{tv - v}");
                                        }
                                        Thread.Sleep(1000);
                                    }
                                }
                                while (true)
                                {
                                    ts = MyChannel.GetChannelSubscribers(_id);
                                    tv = MyChannel.GetChannelViews(_id);
                                    vV = Video.GetVideoViews(lvI);
                                    vL = Video.GetVideoLikes(lvI);
                                    vD = Video.GetVideoDislikes(lvI);
                                    Console.Clear();
                                    if (ts >= s)
                                    {
                                        
                                        Console.WriteLine($@"== LIVE ==============================================
{channelName}
Подписчики: {ts} +{ts - s}
Просмотры: {tv} +{tv - v}

-= Последний ролик | {lvT} =-
Айди ролика: {lvI}
Кол-во просмотров: {vV} +{vV - lvV}
Кол-во лайков: {vL} +{vL - lvL}
Кол-во диз-лайков: {vD} +{vD - lvD}");
                                    }
                                    else
                                    {
                                        Console.WriteLine($@"== LIVE ==============================================
{channelName}
Подписчики: {ts} {ts - s}
Просмотры: {tv} +{tv - v}

-= Последний ролик | {lvT} =-
Айди ролика: {lvI}
Кол-во просмотров: {vV} +{vV - lvV}
Кол-во лайков: {vL} +{vL - lvL}
Кол-во диз-лайков: {vD} +{vD - lvD}");
                                    }
                                    Thread.Sleep(5000);
                                }
                            });
                            t.Start();
                        }
                        else
                        {

                        }
                        break;
                    case "revoke":
                        try
                        {
                            MyChannel.RevokeAccess();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"\n[EXCEPTION]\nMessage: {ex.Message}\n-=-=-\nStack Trace: {ex.StackTrace}\n-=-=-\nTarget Site: {ex.TargetSite}\n-=-=-\nSource: {ex.Source}\n[END]");
                        }
                        break;
                    case "version":
#pragma warning disable CS0162
                        if (alphaTest) Console.WriteLine($@"
   _____      __  __  _____ _____  
  / ____|    |  \/  |/ ____|  __ \ 
 | |   ______| \  / | |  __| |__) |
 | |  |______| |\/| | | |_ |  _  / 
 | |____     | |  | | |__| | | \ \ 
  \_____|    |_|  |_|\_____|_|  \_\
                                   
Product name: {AssemblyProduct}
Product version: {AssemblyVersion}
-= Attention! The alpha testing function is enabled! =-
");
                        else Console.WriteLine($@"
   _____      __  __  _____ _____  
  / ____|    |  \/  |/ ____|  __ \ 
 | |   ______| \  / | |  __| |__) |
 | |  |______| |\/| | | |_ |  _  / 
 | |____     | |  | | |__| | | \ \ 
  \_____|    |_|  |_|\_____|_|  \_\

Product name: {AssemblyProduct}
Product version: {AssemblyVersion}
");
#pragma warning restore CS0162

                        break;
                    default:
                        if (string.IsNullOrEmpty(input[0]))
                        {
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($@"""{input[0]}"" не является внутренней или внешней
командой, исполняемой программой или пакетным файлом.");
                            Console.ResetColor();
                        }
                        break;
                }
            }
            
        }

        static void Initializate()
        {
            
            var clientSecrets = new ClientSecrets
            {
                ClientId = "439459946482-ko966n1ao28a1j363rqp2mas47qjmn30.apps.googleusercontent.com",
                ClientSecret = "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"
            };

            _credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
            clientSecrets,
            new[] { YouTubeService.Scope.YoutubeForceSsl },
            "user",
            CancellationToken.None).Result;

            _youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = _credential,
                ApplicationName = "ytm"
            });

            MyChannel._api = _api;
            MyChannel._id = _id;
            MyChannel._credential = _credential;
            MyChannel._youtubeService = _youtubeService;

            Video._api = _api;
            Video._id = _id;
            Video._credential = _credential;
            Video._youtubeService = _youtubeService;

            Comment._api = _api;
            Comment._id = _id;
            Comment._credential = _credential;
            Comment._youtubeService = _youtubeService;

            Functions._api = _api;
            Functions._id = _id;
            Functions._credential = _credential;
            Functions._youtubeService = _youtubeService;

            try
            {
                if (MyChannel.GetMyChannelId() == null)
                {
                    Console.WriteLine("К сожалению в данный момент приложение не работает! Попробуйте перезапустить или повторите попытку позднее.");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
            }
            catch
            {
                Console.WriteLine("К сожалению в данный момент приложение не работает! Попробуйте перезапустить или повторите попытку позднее.");
                Console.ReadKey();
                Environment.Exit(0);
            }
        } 
    }
}
