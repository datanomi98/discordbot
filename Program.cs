using DiscordSharp;
using System;
using System.Net;
using System.Threading;
using System.Net.Mail;
using System.IO;
using Newtonsoft.Json;

namespace Examples.SmptExamples.Async
{
    namespace discord_bot
    {
        class Program
        {
          

            public static bool isbot = true;
            static void Main(string[] args)
            {



                DiscordClient clientti = new DiscordClient("apikey", isbot);
                clientti.ClientPrivateInformation.Email = "sposti";
                clientti.ClientPrivateInformation.Password = "password";


                clientti.Connected += (sender, e) =>
                {
                    Console.WriteLine("user connected " + e.User.Username);

                    clientti.UpdateCurrentGame("kek");
                };

                clientti.PrivateMessageReceived += (sender, e) =>
                {
                    var fromAddress = new MailAddress("botmail", "BOT");
                    var toAddress = new MailAddress("", "");
                    const string fromPassword = "gmail thing";
                    const string subject = "uusi viesti discordista";
                    string body = "katso uusi viesti " + e.Author.Username;

                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                    };
                    using (var message = new MailMessage(fromAddress, toAddress)
                    {
                        Subject = subject,
                        Body = body
                    })
                    {
                        smtp.Send(message);
                    }




                };

                clientti.PrivateMessageReceived += (sender, e) =>
                {
                    string msg = e.Message.ToString();

                    if (e.Message == "!msg")
                    {

                        e.Author.SendMessage(msg);


                    }
                   
                    if (msg.StartsWith("!"))
                    {
                        // var tag = msg.Substring(0, msg.IndexOf(' ') == -1 ?
                        var tag = msg.Substring(0, msg.IndexOf(' ') == -1 ? msg.Length : msg.IndexOf(' '));
                        tag = tag.Replace("!", "");
                        var cmd = msg.Substring(msg.IndexOf(' ') == -1 ? msg.Length : msg.IndexOf(' ') + 1);
                        if (tag == "test")
                        {
                            if (cmd == "")
                            {
                                e.Author.SendMessage("test");
                            }
                            else
                            {
                                e.Author.SendMessage("testi " + cmd);
                            }

                        }
                        if (tag == "kek")
                        {
                            if (cmd == "")
                            {
                                e.Author.SendMessage("kekkimusmaksimus");
                            }
                            else
                            {
                                e.Author.SendMessage("test " + cmd);
                            }
                        }

                    }

                    if (e.Message.Contains("join"))
                    {
                        if (!isbot)
                        {
                            string inviteID = e.Message.Substring(e.Message.LastIndexOf('/') + 1);
                            clientti.AcceptInvite(inviteID);
                            e.Author.SendMessage("joined your discord channel");
                            Console.WriteLine("got join request from " + inviteID);
                        }
                    };



                };

               

                clientti.MessageReceived += (sender, e) =>

                {
                    var fromAddress = new MailAddress("bot mail", "BOT");
                    var toAddress = new MailAddress("", "");
                    const string fromPassword = "gmail thing";
                    const string subject = "uusi viesti discordista";
                    string body = "katso uusi viesti" + e.Author.Username;

                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                    };
                    using (var message = new MailMessage(fromAddress, toAddress)
                    {
                        Subject = subject,
                        Body = body
                    })
                    {
                        smtp.Send(message);
                    }

                    if(e.MessageText == "!rules")
                    {
                        e.Channel.SendMessage("channel rules:");

                    }
                    //overwatch profile info with json file parsering
                    while(e.MessageText.StartsWith("!ow "))
                    {
                        string mytext = e.MessageText;
                        string newline = mytext.Substring(mytext.IndexOf(' ') + 1);

                        string url = "https://api.lootbox.eu/pc/eu/" + newline.Replace("#", "-") + "/profile";
                        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);


                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();


                        StreamReader sr = new StreamReader(response.GetResponseStream());

                        string res;
                        string html = sr.ReadToEnd();
                        sr.Close();
                        response.Close();
                        res = html;
                        dynamic dyn = JsonConvert.DeserializeObject(res);



                        string error = dyn.statusCode;
                        if(error == "404")
                        {
                            e.Channel.SendMessage("errori.");
                            goto Lopetus;
                            
                        }
                        string name = dyn.data.username;
                        string level = dyn.data.level;



                        string message ="```\n" + "nimi:" + name + " \nleveli: " + level + "\n``` ";
                        e.Channel.SendMessage(message);

                        break;
                    }
                    Lopetus: { }
                    

                    //string msg = e.Message.ToString();
                    //if (msg.Contains(" "))
                    //{

                    //   var tag = msg.Substring(0, msg.IndexOf(' ') == -1 ? msg.Length : msg.IndexOf(' '));
                    //    tag = tag.Replace("!", "");
                    //    var cmd = msg.Substring(msg.IndexOf(' ') == -1 ? msg.Length : msg.IndexOf(' ') + 1);
                    //    if (tag == "test")
                    //    {
                    //        if (cmd == "")
                    //        {
                    //            e.Author.SendMessage("test");
                    //        }
                    //        else
                    //        {
                    //            e.Author.SendMessage("testi " + cmd);
                    //        }

                    //    }
                    //    if (tag == "kek")
                    //    {
                    //        if (cmd == "")
                    //        {
                    //            e.Author.SendMessage("kekkimusmaksimus");
                    //        }
                    //        else
                    //        {
                    //            e.Author.SendMessage("test " + cmd);
                    //        }
                    //    }

                    //}


                    //if (e.MessageText == "!admin")
                    //{
                    //    bool isadmin = false;
                    //    List<DiscordRole> roles = e.Author.Roles;
                    //    foreach (DiscordRole role in roles)
                    //    {
                    //        if (role.Permissions.HasPermission(DiscordSpecialPermissions.ManageServer))
                    //        {
                    //            isadmin = true;
                    //        }
                    //    }
                    //    if (isadmin)
                    //    {
                    //        e.Channel.SendMessage("Yes, you are! :D");
                    //        Console.WriteLine(isadmin);
                    //    }
                    //    else
                    //    {
                    //        e.Channel.SendMessage("No, you aren't :c");
                    //        Console.WriteLine(isadmin);
                    //    }


                    //}
                };

                try {
                    //login request
                    Console.WriteLine("sending login request");
                    clientti.SendLoginRequest();
                    Console.WriteLine("connection clien in separate thread");
                    Thread connect = new Thread(clientti.Connect);
                    connect.Start();
                    Console.WriteLine("client connected");
                } catch (Exception e)
                {
                    Console.WriteLine("something went wrong\n" + e.Message + "\nPress any key to close this window.");

                }
                Console.ReadKey();//if user press the key bot will shut down
                Environment.Exit(0);//make sure all threads are closed

            }

           
        
        }
    }
}
