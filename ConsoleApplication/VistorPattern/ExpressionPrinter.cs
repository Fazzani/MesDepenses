using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication.VistorPattern
{
    class ExpressionPrinter
    {
        private readonly IndentedTextWriter output;

        public ExpressionPrinter(TextWriter output)
        {
            this.output = new IndentedTextWriter(output);
        }

        public void Print(Expression exp)
        {
            dynamic d = exp;
            this.Visit(d);
        }

        private void Visit(Expression exp)
        {
            output.WriteLine("Expression");
        }

        private void Visit(BinaryExpression exp)
        {
            output.WriteLine("BinaryExpression: {0}", exp.NodeType);
            this.VisitSubExpression(exp.Left);
            this.VisitSubExpression(exp.Right);
        }

        private void Visit(ConditionalExpression exp)
        {
            output.WriteLine("ConditionalExpression");
            this.VisitSubExpression(exp.Test);
            this.VisitSubExpression(exp.IfTrue);
            this.VisitSubExpression(exp.IfFalse);
        }

        private void Visit(ConstantExpression exp)
        {
            output.WriteLine("ConstantExpression: {0}", exp.Value);
        }

        private void Visit(MethodCallExpression exp)
        {
            output.WriteLine("MethodCall: {0}", exp.Method.Name);
        }

        private void Visit(ParameterExpression exp)
        {
            output.WriteLine("ParameterExpression: {0}", exp.Name);
        }

        public void Visit(UnaryExpression exp)
        {
            output.WriteLine("UnaryExpression: {0}", exp.NodeType);
            this.VisitSubExpression(exp.Operand);
        }

        private void VisitSubExpression(Expression exp)
        {
            output.Indent++;
            dynamic d = exp;
            this.Visit(d);
            output.Indent--;
        }
    }
}
