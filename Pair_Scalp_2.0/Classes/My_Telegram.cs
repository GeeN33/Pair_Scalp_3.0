using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Pair_Scalp_2._0.Classes
{
    public static class My_Telegram
    {
        public delegate void delegUp_data(string str); // 1. Объявляем делегат
        public static event delegUp_data evenvUp_data;

        public static long id = 0;
        public static string availableBalance = "";
        public static string balance = "";
        //   static string Token { get; set; } = "5469109803:AAF9Wh6d8yTcW5VdQ-z8JY1t9QJJzxw_m7Q";
        
         static string Token { get; set; } = "5580910093:AAFf8w6KNW30LsWLz9-Mdf0hLUzqY61d2yU";

        static ITelegramBotClient bot = new TelegramBotClient(Token);

        static CancellationTokenSource cts = new CancellationTokenSource();
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Некоторые действия
            evenvUp_data?.Invoke(Newtonsoft.Json.JsonConvert.SerializeObject(update));

            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                var message = update.Message;

                if (message.Text.ToLower() == "/start")
                {
                    await botClient.SendTextMessageAsync(message.Chat, "Добро пожаловать на борт, добрый путник!");
                    await botClient.SendTextMessageAsync(message.Chat, "Введите PIN-код!");
                    return;
                }
                if (id != message.Chat.Id && message.Text == "2357")
                {
                    id = message.Chat.Id;
                    await botClient.SendTextMessageAsync(message.Chat, "Ура, ты на борту!");

                    return;
                }

                if (id != message.Chat.Id)
                {
                    await botClient.SendTextMessageAsync(message.Chat, "Я не могу пустить тебя на борт пока ты не введёшь PIN-код!");
                    await botClient.SendTextMessageAsync(message.Chat, "Введите PIN-код!");
                    return;
                }

                if (id == message.Chat.Id)
                {
                    id = message.Chat.Id;
                    await botClient.SendTextMessageAsync(message.Chat, "Хозяин я тут, твой баланс составляет $" + balance + " из них доступно $" + availableBalance);

                    return;
                }


            }
        }

        static Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };
            evenvUp_data?.Invoke(ErrorMessage);
            return Task.CompletedTask;
        }

        public static void Start()
        {

                evenvUp_data?.Invoke("Запущен бот " + bot.GetMeAsync().Result.FirstName);

           

                var cts = new CancellationTokenSource();
                var cancellationToken = cts.Token;
                var receiverOptions = new ReceiverOptions
                {
                    AllowedUpdates = Array.Empty<UpdateType>() // receive all update types
                };
                bot.StartReceiving(
                    HandleUpdateAsync,
                    HandlePollingErrorAsync,
                    receiverOptions,
                    cancellationToken
                );

        }

        public static async void Send_Message(string str)
        {

            if (id != 0) await bot.SendTextMessageAsync(id, str);

        }

        public static void Cancel_T()
        {
            cts.Cancel();
        }

    }
}
