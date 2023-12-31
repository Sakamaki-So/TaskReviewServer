﻿using Firebase.Auth.Providers;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using FirebaseData;
using System.Globalization;

namespace TaskReviewServer
{
    internal class FirebaseManager
    {
        const string AUTH_DOMAIN = "taskview-f65fb.firebaseapp.com";
        const string FIREBASE_URL = "https://taskview-f65fb-default-rtdb.firebaseio.com/";
        const string TASK_KEY = "tasks";
        const string EVALUATION_KEY = "evaluations";
        const string PRIORITY_KEY = "priority";
        readonly static string colorBar = new('%', 40);
        readonly static string colorBar2 = new('/', 40);

        internal static async Task<FirebaseClient> GetClient()
        {
            var config = new FirebaseAuthConfig
            {
                ApiKey = GetValue("APIキーを入力してください: "),
                AuthDomain = AUTH_DOMAIN,
                Providers = new FirebaseAuthProvider[]
                {
                    new EmailProvider()
                }
            };
            var auth = new FirebaseAuthClient(config);
            var userCredential = await auth.SignInWithEmailAndPasswordAsync(
                GetValue("メールアドレスを入力してください："), GetValue("パスワードを入力してください："));
            var firebase = new FirebaseClient(FIREBASE_URL,
                new FirebaseOptions
                {
                    AuthTokenAsyncFactory = () => userCredential.User.GetIdTokenAsync()
                });
            return firebase;
        }
        private static string GetValue(string message)
        {
            Console.Write(message);
            return Console.ReadLine() ?? string.Empty;
        }

        internal static async System.Threading.Tasks.Task ChangePriority(FirebaseClient firebase, bool isHide)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("TODOリストのデータをFirebaseから取得しています");
            if (isHide == false) Console.WriteLine($"\n{colorBar2}");
            Console.ResetColor();
            var tasks = await firebase.Child(TASK_KEY).OnceAsync<FirebaseData.Task>();
            foreach (var task in tasks)
            {
                if (isHide == false)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(colorBar);
                    Console.ResetColor();
                    task.Object.Print(task.Key);
                }
                var estimatedTime = task.Object.estimatedTime;
                var date = GetDateFromJavaFormat(task.Object.deadline ?? "");
                var priority = await CalcPriorityFromEvaluation(firebase, task.Key, date, estimatedTime, isHide);
                await firebase.Child(TASK_KEY).Child(task.Key).Child(PRIORITY_KEY).PutAsync(priority);
                if (isHide == false)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{colorBar}\n\n\n");
                    Console.ResetColor();
                }
            }
            if (isHide == false)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(colorBar2);
                Console.ResetColor();
            }
        }
        private static async Task<double> CalcPriorityFromEvaluation(FirebaseClient firebase, string key, DateTime date, int estimatedTime, bool isHide)
        {
            double scoreSum = 0;
            double score = 0;
            int evalNum = 0;
            var evals = await firebase
                .Child(EVALUATION_KEY)
                .OnceAsync<Evaluation>();
            if (isHide == false)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n<評価データ>");
                Console.ResetColor();
            }
            foreach (var eval in evals)
            {
                if (eval.Object.firebaseKey != key) continue;
                if (isHide == false) eval.Object.Print(eval.Key);
                evalNum++;
                if (eval.Object.isFinished == false) scoreSum += 4.0;
                else scoreSum += eval.Object.evaluation switch
                {
                    0 => 3.0,
                    1 => 2.5,
                    2 => 2.0,
                    3 => 1.0,
                    4 => 0.5,
                    5 => -0.5,
                    _ => 1.0
                };
            }
            score = evalNum == 0 ? 0 : scoreSum / evalNum;
            score += Math.Clamp(estimatedTime / 3000, 0.0, 0.5);
            score += Math.Clamp(0.5 - (DateTime.Now - date).TotalDays / 28, 0.0, 0.5); 
            return Math.Clamp(score, 0.0, 2.0);
        }

        private static DateTime GetDateFromJavaFormat(string javaDate)
        {
            try
            {
                DateTime date = DateTime.ParseExact(javaDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                return date.Date;
            }
            catch (Exception) {
                return DateTime.Now.Date;
            }
        }
    }
}
