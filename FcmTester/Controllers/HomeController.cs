using FcmTester.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using FirebaseAdmin.Messaging;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace FcmTester.Controllers;

public class HomeController(ILogger<HomeController> logger) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public async Task<ActionResult> Index(PushNotificationModel? model)
    {
        model ??= new PushNotificationModel();
        if (HttpContext.Request.Method == "POST")
        {
            model.Result = await SendMessageAsync(model);
        }

        return View(model);
    }

    private static async Task<string> SendMessageAsync(PushNotificationModel model)
    {
        try
        {
            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromJson(model.FcmToken)
            });
            var message = new Message
            {
                Notification = new Notification
                {
                    Title = model.Title,
                    Body = model.Body,
                    ImageUrl = model.ImageUrl
                },
                Data = new Dictionary<string, string>
                {
                    { "click_action", model.ClickAction }
                },
                Token = model.DeviceToken
            };


            var messageId = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            return $"Message sent: {messageId}";
        }
        catch (Exception e)
        {
            return $"Failed to send the message: {e.Message}";
        }
        finally
        {
            FirebaseApp.DefaultInstance.Delete();
        }
    }
}