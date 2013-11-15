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
            console.GetReference("log").SetValue(context.CreateHostFunction((scope, self, args) =>
                                                                                {
                                                                                    Console.Out.WriteLine(
                                                                                        string.Join<IJsValue>("", args));
                                                                                    return JsUndefined.Instance;
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
                    var result = context.Execute(line);
                    Console.Out.WriteLine("> {0}", result);
                }
                catch(Exception e)
                {
                    Console.Out.WriteLine("{0}: {1}",e.GetType().Name, e.Message);
                }
            }
        }
    }
}
