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

    public async Task HandleAsync(Update update, CancellationToken token)
    {
        var chatId = update.CallbackQuery?.Message?.Chat.Id ?? update.Message!.Chat.Id;
        var data = update.CallbackQuery?.Data;

        (var text, var keyboard, var isNewMessage) = data switch
        {
            CQDataSlaveQuestion => ("Подопечный - это тот, кто получит подарок", null, true),
            CQDataSantaQuestion => ("Тайный Санта - это тот, кто подарит подарок", null, true),
            CQDataGiftQuestion => ("Подарок - это любой предмет, который вы сможете подарить", null, true),
            _ => ("Правила игры", new InlineKeyboardMarkup(new[]
            {
                new[] { InlineKeyboardButton.WithCallbackData("Кто такой подопечный?", CQDataSlaveQuestion) },
                new[] { InlineKeyboardButton.WithCallbackData("Кто такой тайный Санта?", CQDataSantaQuestion) },
                new[] { InlineKeyboardButton.WithCallbackData("Какой подарок мне нужно подарить?", CQDataGiftQuestion) },
            }), false)
        };

        if (isNewMessage || update.CallbackQuery == null)
        {
            await _bot.SendTextMessageAsync(
                chatId: chatId,
                text: text,
                replyMarkup: keyboard,
                cancellationToken: token);
        }
        else
        {
            await _bot.EditMessageTextAsync(
                chatId: chatId,
                messageId: update.CallbackQuery?.Message?.MessageId ?? update.Message!.MessageId,
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