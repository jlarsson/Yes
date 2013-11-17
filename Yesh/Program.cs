using System;
using Yes;
using Yes.Interpreter.Model;

namespace Yesh
{
    class Program
    {
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
