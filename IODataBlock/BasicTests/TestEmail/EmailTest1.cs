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
            VerifyRcptTo();
            //Article2();
        }



        // https://github.com/MichaelEvanchik/EmailVerifyDeliverable/blob/master/Verify.cs

        #region Article 1

        //should be renamed to VerifyRCPTO, havent made a VerfyVRFY since most domains do not allow this
        public void VerifyVRFY(DataSet semails)
        {
            string email = "";
            int keyfield = 0;
            string sSuffix = "";
            string domain = "";
            string user = "";
            string suffix = "";
            int i = 0;

            DataTable retDT = new DataTable();
            retDT = semails.Tables[0];
            foreach (DataRow sEmail in retDT.Rows)
            {
                while (true)
                {

                    email = sEmail[1].ToString().Replace(" ", "").ToLower();
                    domain = email.Substring(email.IndexOf("@") + 1);
                    keyfield = Convert.ToInt32(sEmail[0].ToString());
                    if (domain == "gmail.com")
                    {
                        System.Threading.Thread.Sleep(20);
                        try
                        {
                            string response = "";
                            string message = "";
                            TcpClient client = new TcpClient();
                            client.Connect("173.194.70.27", 25);
                            response = Response(client);
                            message = "HELO me.com\r\n";
                            Write(message, client);
                            response = Response(client);
                            message = "MAIL FROM:<xgcmcbain@hotmail.com>\r\n";
                            Write(message, client);
                            response = Response(client);
                            message = "RCPT TO:<" + email + ">\r\n";
                            Write(message, client);
                            response = Response(client);
                            if (response.Substring(0, 3) == "250")
                            {
                                message = "quit\r\n";
                                Write(message, client);
                                client.Close();
                                //DbExecute("update " + slTableName + " set " + slReasonFieldName + " = 18, " + slSuprreesionIDFieldName + " = 12 where " + slKeyField + " = " + keyfield, slConnectionType);
                                break;
                            }
                            else
                            {
                                //DbExecute("update " + slTableName + " set " + slReasonFieldName + " = 102, " + slSuprreesionIDFieldName + " = 0 where " + slKeyField + " = " + keyfield, slConnectionType);
                                break;
                            }
                        }
                        catch
                        {
                            //DbExecute("update " + slTableName + " set " + slReasonFieldName + " = 24, " + slSuprreesionIDFieldName + ", " + slNewEmailFieldName + " = 'invalid'= 0 where " + slKeyField + " = " + keyfield, slConnectionType);
                        }
                    }

                }
            }
        }



        public void VerifyRcptTo()
        {
            try
            {
                var email = "atemnorod@cloudroute.com";
                string response = "";
                string message = "";
                TcpClient client = new TcpClient();
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


        public void Write(string message, TcpClient c)
        {
            byte[] WriteBuffer = new byte[1024];
            WriteBuffer = Encoding.ASCII.GetBytes(message);
            var stream = c.GetStream();
            stream.Write(WriteBuffer, 0, WriteBuffer.Length);
        }

        public string Response(TcpClient c)
        {
            //System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            var serverbuff = new byte[1024];
            var stream = c.GetStream();
            var count = stream.Read(serverbuff, 0, 1024);
            if (count == 0)
            {
                return "";
            }
            return Encoding.ASCII.GetString(serverbuff, 0, count);
        } 

        #endregion



        public void Article2()
        {
            var email = "atemnorod@cloudroute.com";
            TcpClient tClient = new TcpClient("cloudroute-com.mail.protection.outlook.com", 25);
            string CRLF = "\r\n";
            byte[] dataBuffer;
            string ResponseString;
            NetworkStream netStream = tClient.GetStream();
            StreamReader reader = new StreamReader(netStream);
            ResponseString = reader.ReadLine();
            /* Perform HELO to SMTP Server and get Response */
            dataBuffer = BytesFromString("HELO cloudroute.com" + CRLF);
            netStream.Write(dataBuffer, 0, dataBuffer.Length);
            ResponseString = reader.ReadLine();
            dataBuffer = BytesFromString("MAIL FROM:<jkosh@cloudroute.com>" + CRLF);
            netStream.Write(dataBuffer, 0, dataBuffer.Length);
            ResponseString = reader.ReadLine();
            /* Read Response of the RCPT TO Message to know from google if it exist or not */
            dataBuffer = BytesFromString("RCPT TO:<" + email + ">" + CRLF);
            netStream.Write(dataBuffer, 0, dataBuffer.Length);
            ResponseString = reader.ReadLine();
            if (GetResponseCode(ResponseString) == 550)
            {
                Console.Write("Mai Address Does not Exist !<br/><br/>");
                Console.Write("<B><font color='red'>Original Error from Smtp Server :</font></b>" + ResponseString);
            }
            /* QUITE CONNECTION */
            dataBuffer = BytesFromString("QUIT" + CRLF);
            netStream.Write(dataBuffer, 0, dataBuffer.Length);
            tClient.Close();

        }

        private byte[] BytesFromString(string str)
        {
            return Encoding.ASCII.GetBytes(str);
        }
        private int GetResponseCode(string ResponseString)
        {
            return int.Parse(ResponseString.Substring(0, 3));
        }
    }
}
