using FluentEmail.Core;
using FluentEmail.Razor;
using FluentEmail.Smtp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using TheIslandPostManager.Models;

namespace TheIslandPostManager.Services;
internal class EmailService : IEmailService
{
    private readonly IMessageService messageService;

    public EmailService(IMessageService messageService)
    {
        this.messageService = messageService;
    }

    public async Task<bool> SendEmail(Order order)
    {

        try
        {
            var fluentService = App.GetService<IFluentEmail>();

            //Email.DefaultSender = new SmtpSender(smtp);

            var email = await fluentService
                .To(order.Email, order.Name)
                .Subject($"The Island Post Photography Department - {order.Name}")
                .CC(order.CC)
                .UsingTemplateFromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "template.cshtml"), new
                {
                    Link = order.DownloadURL
                })
                .SendAsync();

            if (email.Successful)
            {
                order.IsFinalized = true;
                return true;
            }
        }
        catch (Exception ex)
        {
            messageService.ShowErrorMessage("[EmailService] Error in SendEmail", ex.Message, ex.StackTrace, "");
            messageService.ShowSnackBarMessage("Email Error", "Failed to send email", Wpf.Ui.Controls.ControlAppearance.Danger, Wpf.Ui.Controls.SymbolRegular.ThumbDislike16);
        }

        return false;
    }
}
