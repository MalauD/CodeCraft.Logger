﻿using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using CodeCraft.Logger.ProducerConsumer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeCraft.Logger.NetFramework.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        ConsoleLogger ConsoleLogger = new ConsoleLogger();
        [TestMethod]
        public void ConsoleLoggerWithoutDispose()
        {
            var t1 = new Thread(new ParameterizedThreadStart(TraceEvery300msLogs));
            t1.Start(ConsoleLogger);
            t1.Join();

            //  ConsoleLogger.Dispose();

        }

        [TestMethod]
        public void ConsoleLoggerWithDispose()
        {
            var logger = new ConsoleLogger();
            {
                var t1 = new Thread(new ParameterizedThreadStart(TraceEvery300msLogs));
                t1.Start(ConsoleLogger);
                t1.Join();
            }
            ConsoleLogger.Dispose();

        }
        [TestMethod]
        public void ConsoleLoggerWithUsing()
        {
            using (var logger = new ConsoleLogger())
            {
                var t1 = new Thread(new ParameterizedThreadStart(TraceEvery300msLogs));
                t1.Start(ConsoleLogger);
                t1.Join();
            }

        }

        private void TraceEvery300msLogs(object logger) => TraceEvery300msLogs(((ILogger)logger).Trace);
        private void TraceEvery300msLogs(ILogger logger) => TraceEvery300msLogs(logger.Trace);
        private void TraceEvery300msLogs(LogLevel log)
        {
            for (int i = 0; i < 3; i++)
            {
                log(i.ToString());
                Thread.Sleep(300);
            }
        }

        [TestMethod]
        public void ConsoleLoggerWithDisposeCancellation()
        {
            var t1 = new Thread(new ThreadStart(TraceLogs));
            var t2 = new Thread(new ThreadStart(InfoLogs));
            var t3 = new Thread(new ThreadStart(DebugLogs));

            t1.Start();
            t2.Start();
            t3.Start();
            t1.Join();
            t3.Join();
            t2.Join();

            ConsoleLogger.Dispose();
        }

        [TestMethod]
        public void ConsoleLoggerWithoutDisposeCancellation()
        {
            var t1 = new Thread(new ThreadStart(TraceLogs));
            var t2 = new Thread(new ThreadStart(InfoLogs));
            var t3 = new Thread(new ThreadStart(DebugLogs));

            t1.Start();
            t2.Start();
            t3.Start();

 
            t1.Join();
            t3.Join();
            t2.Join();
            Thread.Sleep(2000);
        }


        delegate void LogLevel(string log);

        private void TraceLogs() => Logs(ConsoleLogger.Trace);
        private void InfoLogs() => Logs2(ConsoleLogger.Info);
        private void DebugLogs() => Logs3(ConsoleLogger.Debug);

        private void Logs(LogLevel Log)
        {
            Thread.Sleep(1000);
            for (int i = 0; i < 200; i++)
                Log(i.ToString());
            //
            for (int i = 1000; i < 1200; i++)
                Log(i.ToString());
            Debug.WriteLine("1#################################");
        }
        private void Logs2(LogLevel Log)
        {
            for (int i = 0; i < 200; i++)
            {
                Log(i.ToString());//Thread.Sleep(5);
            }
            Debug.WriteLine("2#################################");
        }

        private void Logs3(LogLevel Log)
        {
            for (int i = 0; i < 200; i++)
            {
                Log(i.ToString());
                //Thread.Sleep(2);
            }
            Debug.WriteLine("3#################################");
        }
    }
}
