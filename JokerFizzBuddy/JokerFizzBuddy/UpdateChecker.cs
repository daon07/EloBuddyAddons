using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;

namespace JokerFizzBuddy
{
    public class UpdateChecker
    {
        public static void CheckForUpdates()
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    using (var webClient = new WebClient())
                    {
                        var rawVersion = webClient.DownloadString("https://raw.githubusercontent.com/JokerArtLoL/EloBuddyAddons/master/JokerFizzBuddy/JokerFizzBuddy/Properties/AssemblyInfo.cs");
                        var match = new Regex(@"\[assembly\: AssemblyVersion\(""(\d{1,})\.(\d{1,})\.(\d{1,})\.(\d{1,})""\)\]").Match(rawVersion);

                        if (match.Success)
                        {
                            var gitVersion = new System.Version(string.Format("{0}.{1}.{2}.{3}", match.Groups[1], match.Groups[2], match.Groups[3], match.Groups[4]));

                            Chat.Print("<font color='#15C3AC'>Joker Fizz - The Tidal Trickster: </font>" + "<font color='#C0C0C0'>Thanks for using Joker Fizz <3!" + "</font>");

                            if (gitVersion != typeof(Program).Assembly.GetName().Version)
                                Chat.Print("<font color='#15C3AC'>Joker Fizz - The Tidal Trickster:</font> <font color='#FF0000'>" + "OUTDATED - Please update to version: " + gitVersion + "</font>");
                            else
                                Chat.Print("<font color='#15C3AC'>Joker Fizz - The Tidal Trickster:</font> <font color='#00FF00'>" + "UPDATED - Version: " + gitVersion + "</font>");
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            });
        }
    }
}
