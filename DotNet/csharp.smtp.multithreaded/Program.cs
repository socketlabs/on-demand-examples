using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Quiksoft.EasyMail.SMTP;

/*
* This sample uses the Quiksoft EasyMail SMTP Object which you can find at: http://www.quiksoft.com/emdotnet/
*/

namespace Bulk_Sender
{
    class Program
    {
        static int threadsOpen = 0;
        static void Main(string[] args)
        {
            //leave the Quiksoft license.key assignment if you want to use EasyMail .Net Edition in trial mode.  Otherwise uncomment and put in your code.
            //Quiksoft.EasyMail.SMTP.License.Key = "Your license key goes here";
            
            DateTime dtStart = DateTime.Now;
                        
            /* fire off 4 threads
             * each thread can send multiple messages
            */
            for (int x = 0; x < 4; x++)
            {
                Interlocked.Increment(ref threadsOpen);
                System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(SendThread));
                t.Start();
            }

            while (threadsOpen > 0)
                System.Threading.Thread.Sleep(500);
            
            
            TimeSpan ts = DateTime.Now - dtStart;
            Console.WriteLine("Finished in: " + ts);
            Console.ReadLine();
        }

        
        public static void SendThread()
        {
            //create SMTP object
            SMTP objSMTP = new SMTP();
            objSMTP.SMTPServers.Add("smtp.socketlabs.com", 25, 60, SMTPAuthMode.AuthLogin, "your_smtp_user", "your_smtp_password");

            /*
             * this sample just sends one message per thread/connection but in the real world you should send about
             * 50-100 messages per connection. You will have to add your database retrieval and loop management here
             * 
             * i.e. grab 50 records from db, connect, loop through and send all 50 and then disconnect
             * repeat as long as there are more records to process in database
            */

            
            //establish connection and keep it open for all messages we send
            objSMTP.Connect();


            EmailMessage objMessage = new EmailMessage();
            objMessage.From = new Address("sender@domain.com", "Sender Name");
            objMessage.Recipients.Add("recipient@domain.com", "Recipient Name", RecipientType.To);
            objMessage.Subject = "Subject...";
            objMessage.BodyParts.Add("Hi ##FIRSTNAME##, Thank you for your interest in ##PRODUCT##.", BodyPartFormat.Plain, BodyPartEncoding.QuotedPrintable);
            Dictionary<string, string> tokens = new Dictionary<string, string>();
            tokens["##FIRSTNAME##"] = "John";
            tokens["##PRODUCT##"] = "SocketLabs Email On-Demand";
            objMessage.BodyParts[0].Body = BulkReplace(objMessage.BodyParts[0].Body, tokens);

            objSMTP.Send(objMessage);

            //close connection after all messages have been sent
            objSMTP.Disconnect();



            Interlocked.Decrement(ref threadsOpen);
        }

        static string BulkReplace(string txt, Dictionary<string, string> tokens)
        {
            foreach (string token in tokens.Keys)
            {
                txt = txt.Replace(token, tokens[token]);
            }
            return txt;
        }

    }
}
