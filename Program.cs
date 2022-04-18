using Telegram.Bot;
using Telegram.Bot.Args;
using HtmlAgilityPack;

namespace telegram_bot
{
    class Program
    {
        private static string token { get; set; } = "*****"; // You can get your token in BotFather in Telegram.
        private static TelegramBotClient client;
        public static string url = "https://minfin.com.ua/ua/currency/";
        public static void Main()
        {
            client = new TelegramBotClient(token);
            client.StartReceiving();
            client.OnMessage += OnMessageHandler;
            Console.ReadLine();
            client.StopReceiving();
        }

        private static async void OnMessageHandler(object? sender, MessageEventArgs e)
        {
            var msg = e.Message;
            if (msg.Text != null) 
            {
                try
                {
                    await client.SendTextMessageAsync(msg.Chat.Id,
                        $"{msg.Text} USD = {Math.Round(Convert.ToDouble(msg.Text) * GetUSDRate(url), 2)} UAH  ({GetUSDRate(url)})\n" +
                        $"{msg.Text} EUR = {Math.Round(Convert.ToDouble(msg.Text) * GetEURRate(url), 2)} UAH  ({GetEURRate(url)})\n" +
                        $"{msg.Text} RUB = {Math.Round(Convert.ToDouble(msg.Text) * GetRUBRate(url), 2)} UAH  ({GetRUBRate(url)})\n" +
                        $"{msg.Text} PLN = {Math.Round(Convert.ToDouble(msg.Text) * GetPLNRate(url), 2)} UAH  ({GetPLNRate(url)})\n" +
                        $"{msg.Text} GBP = {Math.Round(Convert.ToDouble(msg.Text) * GetGBPRate(url), 2)} UAH  ({GetGBPRate(url)})",
                        replyToMessageId: msg.MessageId);
                }
                catch (Exception)
                {
                    await client.SendTextMessageAsync(msg.Chat.Id, $"Please, enter the number only", replyToMessageId: msg.MessageId);
                }
            }
        }

        private static double GetUSDRate(string url)
        {
            HtmlWeb web = new();
            var doc = web.Load(url);
            var rate = doc.DocumentNode.SelectSingleNode(".//tr[1]//td[3]//span[1]").InnerText[1..8].Replace('.',',');
            return double.Parse(rate);
        }
        private static double GetEURRate(string url)
        {
            HtmlWeb web = new();
            var doc = web.Load(url);
            var rate = doc.DocumentNode.SelectSingleNode(".//tr[2]//td[3]//span[1]").InnerText[1..8].Replace('.', ',');
            return double.Parse(rate);
        }
        private static double GetRUBRate(string url)
        {
            HtmlWeb web = new();
            var doc = web.Load(url);
            var rate = doc.DocumentNode.SelectSingleNode(".//tr[3]//td[3]//span[1]").InnerText[1..8].Replace('.', ',');
            return double.Parse(rate);
        }
        private static double GetPLNRate(string url)
        {
            HtmlWeb web = new();
            var doc = web.Load(url);
            var rate = doc.DocumentNode.SelectSingleNode(".//tr[4]//td[3]//span[1]").InnerText[1..8].Replace('.', ',');
            return double.Parse(rate);
        }
        private static double GetGBPRate(string url)
        {
            HtmlWeb web = new();
            var doc = web.Load(url);
            var rate = doc.DocumentNode.SelectSingleNode(".//tr[5]//td[3]//span[1]").InnerText[1..8].Replace('.', ',');
            return double.Parse(rate);
        }
    }
}
