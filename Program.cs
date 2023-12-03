using Firebase.Database;

namespace TaskReviewServer
{
    internal class Application
    {
        static int TIME = 1000 * 60; // ミリ秒指定
        private static async Task Main(string[] args)
        {
            var firebase = await FirebaseManager.GetClient();
            Console.Clear();
            Launch(firebase);
        }

        private static void Launch(FirebaseClient firebase)
        {
            Timer timer = new Timer(
                async o => await FirebaseManager.ChangePriority(firebase), null, 0, TIME);
            Console.ReadLine();
            timer.Dispose();
        }
    }
}