﻿using System;
using CodeCraft.Logger.ProducerConsumer;

namespace CodeCraft.Logger
{ 
    public class ConsoleLogger : BaseLogger<ConsoleLogProducerConsumer>
    {
        protected override void WriteLog(string log) => Console.WriteLine(log);
    }
}
