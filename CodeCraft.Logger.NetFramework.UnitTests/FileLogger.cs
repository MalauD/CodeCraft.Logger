﻿using System;

using System.IO;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeCraft.Logger.NetFramework.UnitTests
{
    [TestClass]
    public class FileLogger
    {
        [TestInitialize]
        public void Initialize()
        {
            var logoutput = @"E:\Log.txt";
            if (File.Exists(logoutput))
                File.Delete(logoutput);
        }
        
        public void Cleanup()
        {
            try
            {
                var logoutput = @"E:\Log.txt";
                Assert.IsTrue(File.Exists(logoutput));
                Assert.AreEqual(81, File.ReadAllLines(logoutput).Length);
            }
            catch (IOException ex)
            {
                Thread.Sleep(5500);
           
            }


        }

        [TestMethod]
        public void FileLoggerTest()
        {
            /*using (*/
            using(var fileLogger = new Logger.FileLogger(@"E:\Log.txt") )
            {
                fileLogger.Error("Tests");
                for (int i = 0; i < 80; i++)
                {
                    fileLogger.Warn($"{i}");
                }
            }
             
        }

        [TestMethod]
        public void FileLoggerTestWitoutUsing()
        {
            /*using (*/
            var fileLogger = new Logger.FileLogger(@"E:\Log.txt");
            {
                fileLogger.Error("Tests");
                for (int i = 0; i < 80; i++)
                {
                    fileLogger.Warn($"{i}");
                }
            }

        }
    }
}
