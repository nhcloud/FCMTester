using System.ComponentModel.DataAnnotations;
using System.Net;

namespace FcmTester.Models;
public class PushNotificationModel
{
    [Display(Name = "Google FCM Token from JSON Key file. [Required]")]
    [Required(ErrorMessage = "Google FCM Token from JSON Key file is required.")]
    public string FcmToken { get; set; }

    [Display(Name = "Targeted device token. [Required]")]
    [Required(ErrorMessage = "Targeted device token is required.")]
    public string DeviceToken { get; set; }

    [Display(Name = "Notification title. [Required]")]
    [Required(ErrorMessage = "Title is required.")]
    public string Title { get; set; }

    [Display(Name = "Notification body. [Required]")]
    [Required(ErrorMessage = "Body is required.")]
    public string Body { get; set; }

    [Display(Name = "Notification image url. [optional]")]
    public string? ImageUrl { get; set; }

    [Display(Name = "Notification click action. [optional]")]
    public string? ClickAction { get; set; }

    public ResultModel? Result { get; set; }
}

public record ResultModel(HttpStatusCode StatusCode, string Message);
