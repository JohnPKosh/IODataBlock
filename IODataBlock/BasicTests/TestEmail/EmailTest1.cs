using System;
using System.Data;
using System.IO;
using System.Net.Sockets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace BasicTests.TestEmail
{
    [TestClass]
    public class EmailTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //VerifyRcptTo();
            //Article2();

            VerifyEmailWithRcptTo("sales@consiliencesoftware.com", "gshepov@cloudroute.com", "ASPMX.L.GOOGLE.COM", "173.203.187.2", 25);
        }



        // https://github.com/MichaelEvanchik/EmailVerifyDeliverable/blob/master/Verify.cs

        #region Article 1

        public void VerifyRcptTo()
        {
            try
            {
                var email = "atemnorod@cloudroute.com";
                var response = "";
                var message = "";
                var client = new TcpClient();
                client.Connect("cloudroute-com.mail.protection.outlook.com", 25);
                response = Response(client);
                message = "HELO gmail.com\r\n";
                Write(message, client);
                response = Response(client);
                message = "MAIL FROM:<jkosh@gmail.com>\r\n";
                Write(message, client);
                response = Response(client);
                message = "RCPT TO:<" + email + ">\r\n";
                Write(message, client);
                response = Response(client);
                Console.WriteLine(response);
                if (response.Substring(0, 3) == "250")
                {
                    message = "quit\r\n";
                    Write(message, client);
                    client.Close();
                    //DbExecute("update " + slTableName + " set " + slReasonFieldName + " = 18, " + slSuprreesionIDFieldName + " = 12 where " + slKeyField + " = " + keyfield, slConnectionType);

                }
                else
                {
                    //DbExecute("update " + slTableName + " set " + slReasonFieldName + " = 102, " + slSuprreesionIDFieldName + " = 0 where " + slKeyField + " = " + keyfield, slConnectionType);
                    Console.WriteLine(response);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //DbExecute("update " + slTableName + " set " + slReasonFieldName + " = 24, " + slSuprreesionIDFieldName + ", " + slNewEmailFieldName + " = 'invalid'= 0 where " + slKeyField + " = " + keyfield, slConnectionType);
            }
        }


        public void VerifyEmailWithRcptTo(string toEmail, string fromEmail, string helo, string hostName, int port = 25)
        {
            try
            {

                var response = "";
                var message = "";
                var client = new TcpClient();
                client.Connect(hostName, 25);
                response = Response(client);
                Write($"EHLO {helo}\r\n", client);
                response = Response(client);
                Write($"MAIL FROM:<{fromEmail}>\r\n", client);
                response = Response(client);
                Write($"RCPT TO:<{toEmail}>\r\n", client);
                response = Response(client);
                Console.WriteLine(response);
                if (response.Substring(0, 3) == "250")
                {
                    Write("quit\r\n", client);
                    client.Close();
                }
                else
                {
                    Console.WriteLine(response);
                    client.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                
            }
        }


        public void Write(string message, TcpClient c)
        {
            var buffer = Encoding.ASCII.GetBytes(message);
            var stream = c.GetStream();
            stream.Write(buffer, 0, buffer.Length);
        }

        public string Response(TcpClient c)
        {
            var serverbuff = new byte[1024];
            var stream = c.GetStream();
            stream.ReadTimeout = 10000;
            var memStream = new MemoryStream();
            var bytesread = stream.Read(serverbuff, 0, serverbuff.Length);
            while (bytesread > 0)
            {
                memStream.Write(serverbuff, 0, bytesread);
                if (!stream.DataAvailable)
                {
                    break;
                }
                bytesread = stream.Read(serverbuff, 0, serverbuff.Length);
            }
            return Encoding.ASCII.GetString(memStream.ToArray());
        } 

        #endregion



        private int GetResponseCode(string ResponseString)
        {
            return int.Parse(ResponseString.Substring(0, 3));
        }
    }
}
