using Firebase.Auth.Providers;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using FirebaseData;

namespace TaskReviewServer
{
    internal class FirebaseManager
    {
        const string AUTH_DOMAIN = "taskview-f65fb.firebaseapp.com";
        const string TASK_KEY = "tasks";
        const string EVALUATION_KEY = "evaluations";
        const string PRIORITY_KEY = "priority";
        readonly static string colorBar = new('%', 40);

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
            var firebase = new FirebaseClient(
                "https://taskview-f65fb-default-rtdb.firebaseio.com/",
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

        internal static async System.Threading.Tasks.Task ChangePriority(FirebaseClient firebase)
        {
            Console.WriteLine("TODOリストのデータをFirebaseから取得しています");
            var tasks = await firebase.Child(TASK_KEY).OnceAsync<FirebaseData.Task>();
            foreach (var task in tasks)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(colorBar);
                Console.ResetColor();
                task.Object.Print(task.Key);
                var priority = await CalcPriorityFromEvaluation(firebase, task.Key);
                await firebase.Child(TASK_KEY).Child(task.Key).Child(PRIORITY_KEY).PutAsync(priority);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{colorBar}\n\n\n");
                Console.ResetColor();
            }
        }
        private static async Task<double> CalcPriorityFromEvaluation(FirebaseClient firebase, string key)
        {
            double score = 0;
            var evals = await firebase
                .Child(EVALUATION_KEY)
                //.OrderBy(FIREBASE_KEY_NAME_KEY)
                .OnceAsync<Evaluation>();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n<評価データ>");
            Console.ResetColor();
            foreach (var eval in evals)
            {
                if (eval.Object.firebaseKey == key) continue;
                eval.Object.Print(eval.Key);
                if (eval.Object.isFinished != false) score += 2.5;
                else score += eval.Object.evaluation switch
                {
                    0 => 2.0,
                    1 => 1.5,
                    2 => 1.0,
                    3 => 0.5,
                    4 => 0.0,
                    5 => -0.5,
                    _ => 0.0
                };
            }
            return Math.Clamp(score / evals.Count, 0.0, 2.0);
        }
    }
}
