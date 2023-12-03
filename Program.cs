using Firebase.Database;

namespace TaskReviewServer
{
    internal class Application
    {
        static readonly int TIME_MINUTE = 1000 * 60; // ミリ秒指定
        const string LAUNCH_CMD = "launch";
        const string HIDE_OPTION = "--hide";
        const string TIME_OPTION = "-t";
        const string EXIT_CMD = "exit";
        private static async Task Main(string[] args)
        {
            var firebase = await FirebaseManager.GetClient();
            Console.Clear();
            while(true)
            {
                var command = getCommandValue(getCommand());
                if (!command.Item1)
                {
                    Console.WriteLine("コマンドが無効です");

                    continue;
                }
                if (command.Item2 == 0) break;
                Launch(firebase, command.Item2, command.Item3);
            }
        }

        private static void Launch(FirebaseClient firebase, int minute, bool isHide)
        {
            Console.Clear();
            Timer timer = new Timer(
                async o => await FirebaseManager.ChangePriority(firebase, isHide), null, 0, TIME_MINUTE * minute);
            Console.ReadLine();
            timer.Dispose();
        }
        // isValidCmd, minute, isHide
        private static (bool, int, bool) getCommandValue(string command)
        {
            var isHide = false;
            var minute = 1;
            var commandArray = command.Split(" ");
            if(commandArray.Length == 0) return (false, minute, isHide);
            if(commandArray.Length == 1)
            {
                if (commandArray[0] == LAUNCH_CMD) return (true, minute, isHide);
                if (commandArray[0] == EXIT_CMD) return (true, 0, isHide);
            }
            if (commandArray[0] != LAUNCH_CMD) return (false, minute, isHide);
            if(commandArray.Length == 2)
            {
                if (commandArray[1] == HIDE_OPTION) return (true, minute, true);
            }
            if(commandArray.Length == 3)
            {
                if (commandArray[1] == TIME_OPTION)
                {
                    if (int.TryParse(commandArray[2], out minute))
                    {
                        if(0 < minute && minute < 14400) return (true, minute, isHide);
                    }
                }
            }
            if(commandArray.Length == 4)
            {
                if(commandArray[1] == TIME_OPTION)
                {
                    if (int.TryParse(commandArray[2], out minute))
                    {
                        if (0 < minute && minute < 14400)
                        {
                            if (commandArray[3] == HIDE_OPTION) return (true, minute, true);
                            Console.WriteLine("オプション引数 <minute> が無効か大きすぎます");
                        }
                    }
                }
                if (commandArray[1] == HIDE_OPTION)
                {
                    if (commandArray[2] == TIME_OPTION)
                    {
                        if (int.TryParse(commandArray[3], out minute))
                        {
                            if (0 < minute && minute < 14400) return (true, minute, true);
                        }
                        else Console.WriteLine("オプション引数 <minute> が無効か大きすぎます");
                    }
                }
            }
            return (false, minute, isHide);
        }

        private static string getCommand()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("使いかた:");
            Console.WriteLine($"{LAUNCH_CMD} < option >: 集計開始");
            Console.WriteLine($"{EXIT_CMD}: 終了");
            Console.WriteLine($"< option >: {TIME_OPTION} < minute >: < minute >分ごとに集計");
            Console.WriteLine($"            {HIDE_OPTION}       : 集計情報を非表示");
            Console.ResetColor();
            Console.Write(">");
            return Console.ReadLine() ?? string.Empty;
        }
    }
}