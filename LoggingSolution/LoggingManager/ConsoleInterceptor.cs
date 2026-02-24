using System;
using System.IO;
using System.Text;

namespace LoggingManager
{
    public class ConsoleInterceptor : TextWriter
    {
        private readonly TextWriter _original;
        private readonly Action<string> _logAction;

        public ConsoleInterceptor(TextWriter original, Action<string> logAction)
        {
            _original = original;
            _logAction = logAction;
        }

        public override Encoding Encoding => Encoding.UTF8;

        public override void WriteLine(string value)
        {
            _logAction(value);
            _original.WriteLine(value);
        }

        public override void Write(string value)
        {
            _logAction(value);
            _original.Write(value);
        }
    }
}