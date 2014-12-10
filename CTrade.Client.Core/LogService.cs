﻿using NLog;
using System;

namespace CTrade.Client.Core
{
    public interface ILogger
    {
        void Info(string message);
        void Error(string message);
        void Error(string message, Exception exception);
    }
    public class LogService : ILogger
    {
        private static readonly Logger _logger;
        static LogService()
        {
            _logger = LogManager.GetLogger("Default");
        }

        public void Error(string message, Exception exception)
        {
            _logger.Error(message, exception);
        }

        public void Info(string message)
        {
            _logger.Info(message);
        }


        public void Error(string message)
        {
            _logger.Error(message);
        }
    }
}