using System;
namespace GS
{
    public class ConstantData
    {
        //This is a dummy Score
        public static int userCoinCount = 100;
        public const string shareDialogTitle = "Amazing Example",
            shareDialogMsg = "This is a Superb Owesome Game! Check this Out.",
            inviteDialogTitle = "Amazing Example",
            inviteDialogMsg = "Let's Play this Great Fun Game!";
        public static Uri fbShareURI = new Uri("http://u3d.as/aRQ"),
                fbSharePicURI = new Uri("http://i.imgur.com/fPs7tnx.png");


        public static string invMessage = "Let's Play this Fun game!",
        invTitle = "Super Awesome Example by Game Slyce",
        openGraphObjURL = "https://www.curioerp.com/gameslyce/plugins/fbpro/sampleog.html",
        achURL = "https://www.curioerp.com/gameslyce/plugins/fbpro/achievement.html";
    }
}
