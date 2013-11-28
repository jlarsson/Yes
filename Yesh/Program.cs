using System;
using Yes;
using Yes.Interpreter.Model;

namespace Yesh
{
    class Program
    {
        const string InitScript = @"
var test = {};
test.run = function (){
    var m = 100;
    for (var i = 1; i < 1000000; ++i){
        m /= i;
    }
};
";
        static void Main(string[] _)
        {
            
            var context = new Context();

            var console = context.CreateObject();
            console.GetReference("log").SetValue(console, context.CreateHostFunction((scope, self, args) =>
                                                                                {
                                                                                    Console.Out.WriteLine(
                                                                                        string.Join<IJsValue>("", args));
                                                                                    return JsUndefined.Value;
                                                                                }));
            context.Environment.CreateReference("console", console);


            Console.Out.WriteLine("Yesh - Yes Javascript Shell");
            context.Execute(InitScript);
            while(true)
            {
                Console.Out.Write(":");
                Console.Out.Flush();
                var line = Console.In.ReadLine();
                if (line == null)
                {
                    break;
                }

                line = line.Trim();
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                try
                {
                    var start = DateTime.Now;
                    var result = context.Execute(line);
                    var elapsed = DateTime.Now - start;
                    Console.Out.WriteLine("[{0}] {1}", elapsed, result);
                }
                catch(Exception e)
                {
                    Console.Out.WriteLine("{0}: {1}",e.GetType().Name, e.Message);
                }
            }
        }
    }
}
