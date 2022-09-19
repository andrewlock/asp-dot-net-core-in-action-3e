using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddScoped<IMessageSender, EmailSender>();
builder.Services.AddScoped<IMessageSender, SmsSender>();
builder.Services.AddScoped<IMessageSender, FacebookSender>();
builder.Services.TryAddScoped<IMessageSender, UnregisteredSender>();

var app = builder.Build();

app.MapGet("/", () => "Try calling /single-message/{username} or /multi-message/{username} and check the logs");
app.MapGet("/single-message/{username}", SendSingleMessage);
app.MapGet("/multi-message/{username}", SendMultiMessage);

app.Run();

string SendSingleMessage(string username, IMessageSender sender)
{
    sender.SendMessage($"Hello {username}!");
    return "Check the application logs to see what was called";
}

string SendMultiMessage(string username, IEnumerable<IMessageSender> senders)
{
    foreach(var sender in senders)
    {
        sender.SendMessage($"Hello {username}!");
    }

    return "Check the application logs to see what was called";
}

interface IMessageSender
{
    void SendMessage(string message);
}
class EmailSender : IMessageSender
{
    public void SendMessage(string message)
    {
        Console.WriteLine($"Sending Email message: {message}");
    }
}
class FacebookSender : IMessageSender
{
    public void SendMessage(string message)
    {
        Console.WriteLine($"Sending Facebook message: {message}");
    }
}
class SmsSender : IMessageSender
{
    public void SendMessage(string message)
    {
        Console.WriteLine($"Sending SMS: {message}");
    }
}
class UnregisteredSender : IMessageSender
{
    public void SendMessage(string message)
    {
        throw new Exception("I'm never registered so shouldn't be called");
    }
}