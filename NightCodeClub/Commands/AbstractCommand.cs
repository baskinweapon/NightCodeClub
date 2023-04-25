using Telegram.Bot.Types;

public interface ICommand {
    public void SendMessage(Update update, CancellationToken cancellationToken);
}

public abstract class AbstractCommand {
    public abstract void SendMessage(Update update, CancellationToken cancellationToken);
}