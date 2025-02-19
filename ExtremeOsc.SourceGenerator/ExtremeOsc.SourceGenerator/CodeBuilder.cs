using System;
using System.Collections.Generic;
using System.Text;

namespace ExtremeOsc.SourceGenerator
{
    internal class CodeBuilder
    {
        private readonly Stack<Scope> scopes = new Stack<Scope>();
        private readonly StringBuilder sb = new StringBuilder();
        private int IndentCount { get => scopes.Count; }

        public class Scope : IDisposable
        {
            private CodeBuilder builder;

            public Scope(CodeBuilder builder)
            {
                this.builder = builder;
                AppendLine("{");
                builder.scopes.Push(this);
            }

            public void AppendLine(string line)
            {
                builder.AppendLine(line);
            }

            public void Dispose()
            {
                builder.scopes.Pop();
                builder.AppendLine("}");
            }
        }

        public void AppendLine(string line)
        {
            sb.AppendLine(new string(' ', IndentCount * 4) + line);
        }

        public void AppendLine()
        {
            sb.AppendLine();
        }

        public Scope BeginScope()
        {
            return new Scope(this);
        }

        public Scope BeginScope(string line)
        {
            AppendLine(line);
            return new Scope(this);
        }

        public override string ToString()
        {
            return sb.ToString();
        }
    }
}
