using WuphfWeb;
using WuphfWeb.Services.IServices;
using WuphfWeb.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using WuphfWeb.Hubs;
using Microsoft.AspNetCore.Identity.UI.Services;
using WuphfUtility;
using Telegram.Bot.Polling;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Azure.AI.Language.Conversations;
using Azure.Core;
using Azure;
using System.Text.Json;
using System.Configuration;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(MappingConfig));


builder.Services.Configure<EmailSenderOptions>(builder.Configuration.GetSection("EmailSender"));
builder.Services.AddTransient<IEmailSender, EmailSender>();


builder.Services.AddHttpClient<IPostService, PostService>();
builder.Services.AddScoped<IPostService, PostService>();

builder.Services.AddHttpClient<ICommentoService, CommentoService>();
builder.Services.AddScoped<ICommentoService, CommentoService>();

builder.Services.AddHttpClient<IUtenteService, UtenteService>();
builder.Services.AddScoped<IUtenteService, UtenteService>();

builder.Services.AddHttpClient<ILikeService, LikeService>();
builder.Services.AddScoped<ILikeService, LikeService>();

builder.Services.AddHttpClient<IChatService, ChatService>();
builder.Services.AddScoped<IChatService, ChatService>();

builder.Services.AddHttpClient<IStoriaService, StoriaService>();
builder.Services.AddScoped<IStoriaService, StoriaService>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddHttpClient<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSignalR();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
              .AddCookie(options =>
              {
                  options.Cookie.HttpOnly = true;
                  options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                  options.LoginPath = "/Auth/Login";
                  options.AccessDeniedPath = "/Auth/AccessDenied";
                  options.SlidingExpiration = true;
              });
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<ChatHub>("/chathub");



var cts = new CancellationTokenSource();
var cancellationToken = cts.Token;
var receiverOptions = new ReceiverOptions
{
    AllowedUpdates = { }, // receive all update types
};

Utility.credential = new AzureKeyCredential(builder.Configuration["TelegramBotsKeys:Azure"]);
Utility.client = new ConversationAnalysisClient(Utility.endpoint, Utility.credential);
Utility.bot = new TelegramBotClientHelp(builder.Configuration["TelegramBotsKeys:Telegram"]);
Utility.bot.StartReceiving(

    Utility.HandleUpdateAsync,
    Utility.HandleErrorAsync,
    receiverOptions,
    cancellationToken
);

app.Run();


public class Utility
{
    public static Uri endpoint = new Uri("https://wuphfbot.cognitiveservices.azure.com/");
    public static AzureKeyCredential credential;
    public static ConversationAnalysisClient client;


    public static ITelegramBotClient bot;
    public static async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
    {
        if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
        {
            var message = update.Message;
            if (message == null || message.Type != MessageType.Text)
                return;
            string projectName = "Wuphf";
            string deploymentName = "post2";
            var text = new
            {
                analysisInput = new
                {
                    conversationItem = new
                    {
                        text = update.Message.Text,
                        id = update.Message.MessageId.ToString(),
                        participantId = "1"
                    }
                },
                parameters = new
                {
                    projectName,
                    deploymentName,

                    // Use Utf16CodeUnit for strings in .NET.
                    stringIndexType = "Utf16CodeUnit",
                },
                kind = "Conversation",
            };
            Response response = client.AnalyzeConversation(RequestContent.Create(text));
            using JsonDocument result = JsonDocument.Parse(response.ContentStream);
            JsonElement conversationalTaskResult = result.RootElement;
            JsonElement conversationPrediction = conversationalTaskResult.GetProperty("result").GetProperty("prediction");

            var topintent = conversationPrediction.GetProperty("topIntent").GetString();
            string? entità = null;
            if (conversationPrediction.GetProperty("entities").GetArrayLength() != 0)
            {
                entità = conversationPrediction.GetProperty("entities")[0].GetProperty("category").GetString();
            }


            if (entità != null)
            {
                switch (topintent)
                {
                    case "Commenti":
                        {
                            switch (entità)
                            {
                                case "Modifica":
                                    {
                                        await bot.SendTextMessageAsync(message.Chat, "Non è possibile modificare alcun commento già pubblicato");
                                    }
                                    break;
                                case "Eliminazione":
                                    {
                                        await bot.SendTextMessageAsync(message.Chat, "Di fianco ad ogni tuo commento il tasto per il like viene sostituito da un tasto per l'eliminazione");

                                    }
                                    break;
                                case "Creazione":
                                    {
                                        await bot.SendTextMessageAsync(message.Chat, "Per creare un commento è necessario andare alla pagina relativa al post in cui si vuole inserire il proprio commento.Per inserire un commento sotto un post clicca il tasto 'commento' che ti reindirizzerà alla pagina dove potrai inserire il tuo commento personale");

                                    }
                                    break;
                                default:
                                    {
                                        await bot.SendTextMessageAsync(message.Chat, "Istruzione non riconosciuta");
                                    }
                                    break;
                            }
                        }
                        break;
                    case "Post":
                        {
                            switch (entità)
                            {
                                case "Modifica":
                                    {
                                        await bot.SendTextMessageAsync(message.Chat, "Per modificare un post è necessario recarsi alla propria pagina profilo. Per ogni post è presente un bottone che ti permetterà di modificare il medesimo post");
                                    }
                                    break;
                                case "Eliminazione":
                                    {
                                        await bot.SendTextMessageAsync(message.Chat, "è possibile eliminare un post nella sezione 'modifica post' dove troverai un bottone per la sua eliminazione");
                                    }
                                    break;
                                case "Creazione":
                                    {
                                        await bot.SendTextMessageAsync(message.Chat, "In ogni pagina nella parte superiore troverai un link 'post' che ti reindirizzerà alla pagina per la creazione di un nuovo post.");
                                    }
                                    break;
                                default:
                                    {
                                        await bot.SendTextMessageAsync(message.Chat, "Istruzione non rilevata");
                                    }
                                    break;
                            }
                        }
                        break;
                    case "Chat":
                        {
                            switch (entità)
                            {
                                case "Creazione":
                                    {
                                        await bot.SendTextMessageAsync(message.Chat, "Per avviare una nuova chat è necessario recarsi nel profilo utente della persona con cui si vuole chattare e cliccare il bottone 'messaggio' ");
                                    }
                                    break;
                                case "EliminazioneMessaggio":
                                    {
                                        await bot.SendTextMessageAsync(message.Chat, "Per eliminare un messaggio basta clicclare l'icona 'cestino' posizionata a lato di ogni messaggio  ");
                                    }
                                    break;
                                case "ElimininazioneChat":
                                    {
                                        await bot.SendTextMessageAsync(message.Chat, "Per eliminare una chat con un utente è necessario recarsi nel profilo dell'utente di cui si vuole eliminare la chat  e cliccare il pulsante elimina che si trova al posto del tasto 'messaggio'  ");
                                    }
                                    break;
                                default:
                                    {
                                        await bot.SendTextMessageAsync(message.Chat, "La chat ti permette di comunicare privatamente con altri utenti");
                                    }
                                    break;
                            }
                        }
                        break;
                    case "Storie":
                        {
                            switch (entità)
                            {
                                case "CreazioneStoria":
                                    {
                                        await bot.SendTextMessageAsync(message.Chat, "Nella pagina principale è presente un tasto che ti permetterà di creare direttamente una nuova storia");
                                    }
                                    break;
                                case "EliminazioneStoria":
                                    {
                                        await bot.SendTextMessageAsync(message.Chat, "non è possibile eliminare nessuna storia precedentemente caricata");
                                    }
                                    break;
                            }
                        }
                        break;
                }
            }
            else
            {
                await bot.SendTextMessageAsync(message.Chat, "Nessun argomento rilevato");
            }
        }
    }

    public static async Task HandleErrorAsync(ITelegramBotClient bot, Exception exception, CancellationToken cancellationToken)
    {
        // Некоторые действия
    }
}