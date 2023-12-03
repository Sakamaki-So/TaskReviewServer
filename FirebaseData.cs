using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirebaseData
{
    public class Task
    {
        public string? deadline { get; set;}
        public int estimatedTime { get; set;}
        public double priority { get; set;}
        public string? subject { get; set;}
        public string? title { get; set;}

        
        internal void Print(string key)
        {
            Console.WriteLine($"キー：{key}");
            Console.WriteLine($"タイトル：{title}");
            Console.WriteLine($"科目：{subject}");
            Console.WriteLine($"期限：{deadline}");
            Console.WriteLine($"予想時間(分)：{estimatedTime}");
            Console.WriteLine($"優先度：{priority}");
        }
    }

    public class Evaluation
    {
        static readonly string colorBar = new('=', 40);
        public double evaluation { get; set;}
        public string? firebaseKey { get; set;}
        public bool isFinished { get; set;}
        internal void Print(string key)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(colorBar);
            Console.ResetColor();
            Console.WriteLine($"キー：{key}");
            Console.WriteLine($"対応するタスクのキー：{firebaseKey}");
            Console.WriteLine($"評価：{evaluation}");
            var isFinishedStr = isFinished ? "はい" : "いいえ";
            Console.WriteLine($"終了したかどうか：{isFinishedStr}");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(colorBar);
            Console.ResetColor();
        }
    }
}
