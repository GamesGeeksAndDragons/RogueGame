using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using log4net;
using log4net.Core;
using Xunit.Abstractions;

namespace AssetsTests.Fakes
{
    public class FakeLogger : ILog
    {
        private readonly ITestOutputHelper _output;

        public FakeLogger(ITestOutputHelper output)
        {
            _output = output;
        }

        public ILogger Logger { get; }
        public void Debug(object message)
        {
            _output.WriteLine($"Debug: {message}");
        }

        private string FormatException(Exception exception)
        {
            return $"Exception [{exception.GetType().Name}], Message [{exception.Message}]";
        }

        public void Debug(object message, Exception exception)
        {
            Debug(message);
            Debug(FormatException(exception));
        }

        private string Format(string format, params object[] args)
        {
            return string.Format(format, args);
        }

        public void DebugFormat(string format, params object[] args)
        {
            Debug(Format(format, args));
        }

        public void DebugFormat(string format, object arg0)
        {
            Debug(Format(format, arg0));
        }

        public void DebugFormat(string format, object arg0, object arg1)
        {
            Debug(Format(format, arg0, arg1));
        }

        public void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            Debug(Format(format, arg0, arg1, arg2));
        }

        public void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Info(object message)
        {
            _output.WriteLine($"Info: {message}");
        }

        public void Info(object message, Exception exception)
        {
            Info(message);
            Info(FormatException(exception));
        }

        public void InfoFormat(string format, params object[] args)
        {
            Info(Format(format, args));
        }

        public void InfoFormat(string format, object arg0)
        {
            Info(Format(format, arg0));
        }

        public void InfoFormat(string format, object arg0, object arg1)
        {
            Info(Format(format, arg0, arg1));
        }

        public void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            Info(Format(format, arg0, arg1, arg2));
        }

        public void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Warn(object message)
        {
            _output.WriteLine($"Warn: {message}");
        }

        public void Warn(object message, Exception exception)
        {
            Warn(message);
            Warn(FormatException(exception));
        }

        public void WarnFormat(string format, params object[] args)
        {
            Warn(Format(format, args));
        }

        public void WarnFormat(string format, object arg0)
        {
            Warn(Format(format, arg0));
        }

        public void WarnFormat(string format, object arg0, object arg1)
        {
            Warn(Format(format, arg0, arg1));
        }

        public void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            Warn(Format(format, arg0, arg1, arg2));
        }

        public void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Error(object message)
        {
            _output.WriteLine($"Error: {message}");
        }

        public void Error(object message, Exception exception)
        {
            Error(message);
            Error(FormatException(exception));
        }

        public void ErrorFormat(string format, params object[] args)
        {
            Error(Format(format, args));
        }

        public void ErrorFormat(string format, object arg0)
        {
            Error(Format(format, arg0));
        }

        public void ErrorFormat(string format, object arg0, object arg1)
        {
            Error(Format(format, arg0, arg1));
        }

        public void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            Error(Format(format, arg0, arg1, arg2));
        }

        public void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Fatal(object message)
        {
            _output.WriteLine($"Fatal: {message}");
        }

        public void Fatal(object message, Exception exception)
        {
            Fatal(message);
            Fatal(FormatException(exception));
        }

        public void FatalFormat(string format, params object[] args)
        {
            Fatal(Format(format, args));
        }

        public void FatalFormat(string format, object arg0)
        {
            Fatal(Format(format, arg0));
        }

        public void FatalFormat(string format, object arg0, object arg1)
        {
            Fatal(Format(format, arg0, arg1));
        }

        public void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            Fatal(Format(format, arg0, arg1, arg2));
        }

        public void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public bool IsDebugEnabled => true;
        public bool IsInfoEnabled => true;
        public bool IsWarnEnabled => true;
        public bool IsErrorEnabled => true;
        public bool IsFatalEnabled => true;
    }
}
