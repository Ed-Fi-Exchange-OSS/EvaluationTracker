// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.Json;

namespace eppeta.webapi.Infrastructure;

public class MailSender
{


    public MailSender()
    {
    }
    public void sendMailForgotPassword(MailSettings mailSettings, string userName, string email, string forgotPasswordUrl)
    {
        string subject = "Reset Password";
        string[] html = File.ReadAllLines("./Infrastructure/Templates/forgotPasswordMail.html");
        StringBuilder sb = new StringBuilder();
        foreach (string line in html)
        {
            sb.AppendLine(line); // Append each line to the StringBuilder object
        }
        sb.Replace("{0}", userName);
        sb.Replace("{1}", forgotPasswordUrl);
        string body = sb.ToString(); //

        sendMail(mailSettings, email, subject, body);
    }
    public void sendMail(MailSettings mailSettings, string email, string subject, string body)
    {
        try
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(mailSettings.From);
            mailMessage.To.Add(email);
            mailMessage.Subject = subject;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = true;

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = mailSettings.Host;
            smtpClient.Port = mailSettings.Port;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(mailSettings.Username, mailSettings.Password);
            smtpClient.EnableSsl = mailSettings.EnableSsl;

            smtpClient.Send(mailMessage);
            Console.WriteLine("Email Sent Successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
}
