Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Net
Imports System.Net.Mail
Imports System.IO
Imports System.Xml.Linq

Namespace SocketLabs.OnDemand.Api
	Friend Class Program
		Private Shared Sub Main(args As String())
			Dim smtpHost As String = "YOUR-SMTP-HOST"
			Dim smtpPort As Integer = 2525
			' standard is port 25 but that is blocked by many ISP's
			Dim smtpUserName As String = "YOUR-SMTP-USER-NAME"
			Dim smtpPassword As String = "YOUR-SMTP-API-PASSWORD"

			Dim creds = New NetworkCredential(smtpUserName, smtpPassword)
			Dim auth = creds.GetCredential(smtpHost, smtpPort, "Basic")

			Using msg = New MailMessage()
				Using smtp = New SmtpClient()
					' ** can be set in config **
					' you can skip this set by setting your credentials in the web.config or app.config
					' http://msdn.microsoft.com/en-us/library/w355a94k.aspx
					smtp.Host = smtpHost
					smtp.Port = smtpPort
					smtp.Credentials = auth
					' ** can be set in config **

					msg.From = New MailAddress("ben@contoso.com")
					msg.[To].Add("jane@contoso.com")
					msg.Subject = "Using the SMTP client with SocketLabs."
					msg.Body = "<h1>Hello</h1><p>How are you doing today?</p>"
					msg.IsBodyHtml = True

					smtp.Send(msg)
				End Using
			End Using

			Console.WriteLine("Email Sent")
			Console.Read()
		End Sub
	End Class
End Namespace
