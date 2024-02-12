using System.Diagnostics;
using System.Net;
using FcmTester.Models;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Mvc;

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
            if (!ModelState.IsValid)
            {
                model.Result = new ResultModel(HttpStatusCode.BadRequest, string.Join("; ", ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage))));
            }
            else
            {
                model.Result = await SendMessageAsync(model);
            }
        }

        return View(AppendTimeToMessage(model));
    }

    private PushNotificationModel AppendTimeToMessage(PushNotificationModel model)
    {
        if (!string.IsNullOrEmpty(model.Result?.Message))
        {
            model.Result=model.Result with { Message = $"[{DateTime.UtcNow} UTC]:" + model.Result.Message };
        }
        return model;
    }

    private static async Task<ResultModel> SendMessageAsync(PushNotificationModel model)
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
                    { "click_action", model.ClickAction??"" }
                },
                Token = model.DeviceToken
            };
            var messageId = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            return new ResultModel(HttpStatusCode.OK, $"Success with messageId:{messageId}");
        }
        catch (Exception e)
        {
            return new ResultModel(HttpStatusCode.BadRequest, $"Failed to send the message:{e.Message}");
        }
        finally
        {
            FirebaseApp.DefaultInstance?.Delete();
        }
    }
}