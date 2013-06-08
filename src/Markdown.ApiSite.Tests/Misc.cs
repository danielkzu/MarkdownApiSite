namespace Markdown.ApiSite
{
    using System;
    using System.Linq;
    using Xunit;

    public class Misc
    {
        [Fact]
        public void when_reading_doc_then_can_build_model()
        {
            var members = DocReader.ReadMembers("Markdown.ApiSite.Tests.xml");

            foreach (var member in members)
            {
                member.Accept(new SampleVisitor());
            }
        }

        public class SampleVisitor : DocVisitor
        {
            private bool newLineNext;

            public override void VisitMember(DocMember member)
            {
                Console.WriteLine();
                Console.WriteLine(new string('-', 50));
                Console.Write("# " + member.Id);
                newLineNext = true;
            }

            public override void VisitSummary(DocSummary summary)
            {
                Console.WriteLine();
                Console.Write("# Summary: ");
                newLineNext = true;
            }

            public override void VisitRemarks(DocRemarks remarks)
            {
                Console.WriteLine();
                Console.WriteLine("## Remarks: ");
                newLineNext = true;
            }

            public override void VisitExample(DocExample example)
            {
                Console.WriteLine();
                Console.Write("Example: ");
                newLineNext = true;
            }

            public override void VisitC(DocC code)
            {
                Console.Write(" `");
                Console.Write(code.Code);
                Console.Write("` ");
            }

            public override void VisitCode(DocCode code)
            {
                if (newLineNext)
                    newLineNext = false;

                Console.WriteLine();
                Console.WriteLine();
                foreach (var line in code.Code.Split(new[] { Environment.NewLine }, StringSplitOptions.None))
                {
                    Console.Write("    ");
                    Console.WriteLine(line);
                }
                Console.WriteLine();
                newLineNext = false;
            }

            public override void VisitText(DocText text)
            {
                if (newLineNext)
                {
                    Console.WriteLine();
                    newLineNext = false;
                }
                Console.Write(text.Text);
            }

            public override void VisitPara(DocPara para)
            {
                if (newLineNext)
                    newLineNext = false;

                Console.WriteLine();
                Console.WriteLine();
            }

            public override void VisitSee(DocSee see)
            {
                Console.Write(" (" + see.Cref + ") ");
            }
        }
    }
}