using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FcmTester.Models;
public class PushNotificationModel
{
    public string FcmToken { get; set; }
    public string DeviceToken { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    [Display(Name = "Image Url (optional)")]
    public string ImageUrl { get; set; }
    [Display(Name = "Click Action (optional)")]
    public string ClickAction { get; set; }
    public string Result { get; set; }
}