﻿using System;
using System.IO;
using System.Linq;

namespace Gherkin.AstGenerator
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: Gherkin.AstGenerator.exe test-feature-file.feature");
                return 100;
            }

            var startTime = Environment.TickCount;
            foreach (var featureFilePath in args)
            {
                try
                {
                    var astText = AstGenerator.GenerateAst(featureFilePath);
                    var grlFilePath = Path.Combine(Path.GetDirectoryName(featureFilePath), Path.GetFileNameWithoutExtension(featureFilePath), ".z151");

                    Console.WriteLine(astText);
                }
                catch (Exception ex)
                {
                    // Ideally we'd use Console.Error here, but we can't because
                    // 2> doesn't seem to work properly - at least not on Mono on OS X.
                    Console.WriteLine(ex.Message);
                    return 1;
                }
            }
            var endTime = Environment.TickCount;
            if (Environment.GetEnvironmentVariable("GHERKIN_PERF") != null)
            {
                Console.Error.WriteLine(endTime - startTime);
            }
            return 0;
        }
    }
}
