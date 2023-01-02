using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Televent.Service.Telegram.Attributes;
using Televent.Service.Telegram.Constants;
using Televent.Service.Telegram.Handlers.Interfaces;

namespace Televent.Service.Telegram.Handlers;

[InlineHandler(CQDataPrefix)]
[StaticCommandHandler($"/{CQDataPrefix}")]
public class RulesHandler : IHandler
{
    public const string CQDataPrefix = "rules";

    private readonly ITelegramBotClient _bot;

    public RulesHandler(ITelegramBotClient bot)
    {
        _bot = bot;
    }

    public async Task HandleAsync(Update update, object? extraData = null, CancellationToken token = default)
    {
        var chatId = update.CallbackQuery?.Message?.Chat.Id ?? update.Message!.Chat.Id;
        var data = update.CallbackQuery?.Data;

        (var text, var isStartMessage) = data switch
        {
            CQDataSlaveQuestion => ("""
            Это человек, которому тебе предстоит подарить Новогодний Подарок. 5 января ты получишь всю необходимую информацию о своем подопечном, например, как его зовут, где он живет и что любит. До 7 января тебе нужно будет сделать подарок этому человеку и тайно подарить ему.
            """, false),
            CQDataSantaQuestion => ("""
            Это ты! Автор новогоднего чуда и хорошего настроение. Человек, который сделает одного отдыхающего на смене счастливым обладателем подарка.
            
            Не переживай, у тебя тоже будет свой Тайный Дед МОроз, который подарит тебе подарок. Для этого тебе нужно зарегистрироваться в этом боте.
            """, false),
            CQDataGiftQuestion => ("""
            1. Укрась или упакуй свой подарок, в этом тебе помогут Хобби-Студии и твои вожатые 
            2. Подпиши имя своего подопечного на подарке (отряд, имя и фамилия), и место, в котором он живет (корпус и комната)
            3. Ты можешь подарить что угодно, от маленькой вкусняшки до самодельной подделки. Сделай подарок на хобби-студии и угости своего подопечного новогодними сладостями. Ты узнаешь информацию о том, что любит твой подопечный, используй ее, чтобы сделать подарок максимально полезным.
            """, false),
            _ => ("""
            Идея игры состоит в том, чтобы обменяться небольшими подарками с другими отдыхающими на смене. Каждый участник станет Дедом Морозом, которому предстоит подарить подарок своему подопечному. 

            Главное и основное правило «Тайного Деда Мороза»: подарки нужно дарить анонимно.
            """, true)
        };

        var keyboard = new InlineKeyboardMarkup(new[]
        {
            new[] { InlineKeyboardButton.WithCallbackData("Кто такой подопечный?", CQDataSlaveQuestion) },
            new[] { InlineKeyboardButton.WithCallbackData("Кто такой тайный Санта?", CQDataSantaQuestion) },
            new[] { InlineKeyboardButton.WithCallbackData("Какой подарок мне нужно подарить?", CQDataGiftQuestion) },
        });
        if (isStartMessage || update.CallbackQuery == null)
        {
            await _bot.SendTextMessageAsync(
            chatId: chatId,
            text: """
                Игра будет проходить на протяжении всей смены.

                ✨ До 4 января (2 день смены)✨
                Ты можешь успеть зарегистрироваться в игре, чтобы стать чьим-то Дедом Морозом и самому получить тайный подарок.

                ✨ 5 января (3 день смены)✨
                Рано утром тебе придет сообщение с информацией о том, кто твой подопечный. Этому человеку ты должен будешь подарить подарок и сделать это тайно, как настоящий Дед Мороз.

                ✨ 7 января (5 день смены)✨
                До этого дня ты должен успеть сделать и подарить подарок своему подопечному. Мы расскажем тебе из какого он отряда, где живет и что любит, а тебе нужно будет подкинуть этому человеку подарок и порадовать его в эти зимние холодные дни.
                """,
            cancellationToken: token);
        }

        if (update.CallbackQuery != null && !isStartMessage)
        {
            await _bot.SendTextMessageAsync(
                chatId: chatId,
                text: text,
                replyMarkup: keyboard,
                cancellationToken: token);
        }
        else
        {
            await _bot.SendTextMessageAsync(
                chatId: chatId,
                text: text,
                replyMarkup: keyboard,
                cancellationToken: token);
        }

        if (update.CallbackQuery != null) await _bot.AnswerCallbackQueryAsync(update.CallbackQuery.Id, cancellationToken: token);
    }

    private const string CQDataSlaveQuestion = $"{CQDataPrefix}{CQDefaults.PrefixDelimiter}slave_question";
    private const string CQDataSantaQuestion = $"{CQDataPrefix}{CQDefaults.PrefixDelimiter}santa_question";
    private const string CQDataGiftQuestion = $"{CQDataPrefix}{CQDefaults.PrefixDelimiter}gift_question";
}