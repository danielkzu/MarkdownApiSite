namespace Markdown.ApiSite
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using ClariusLabs.Doq;
    using Xunit;

    public class Misc
    {
        [Fact]
        public void when_loading_silverlight_assembly_then_succeeds()
        {
            var file = new FileInfo(@"..\..\..\Demo\ClariusLabs.DemoSilverlight\bin\ClariusLabs.DemoSilverlight.dll");
            Assert.True(File.Exists(file.FullName));

            var assembly = Assembly.LoadFrom(file.FullName);

            Assert.Equal(4, assembly.GetExportedTypes().Length);
            Assert.True(assembly
                .GetExportedTypes()
                .First(t => t.Name == "SampleExtensions")
                .GetMethods()
                .First()
                .GetCustomAttributes(true)
                .Any(attr => attr.GetType().FullName == "System.Runtime.CompilerServices.ExtensionAttribute"));
        }

        [Fact]
        public void when_loading_microframework_assembly_then_succeeds()
        {
            var file = new FileInfo(@"..\..\..\Demo\ClariusLabs.DemoMicroFramework\bin\ClariusLabs.DemoMicroFramework.dll");
            Assert.True(File.Exists(file.FullName));

            var assembly = Assembly.LoadFrom(file.FullName);

            Assert.Equal(4, assembly.GetExportedTypes().Length);
            Assert.True(assembly
                .GetExportedTypes()
                .First(t => t.Name == "SampleExtensions")
                .GetMethods()
                .First()
                .GetCustomAttributes(true)
                .Any(attr => attr.GetType().FullName == "System.Runtime.CompilerServices.ExtensionAttribute"));
        }

        [Fact]
        public void when_reading_doc_then_can_build_model()
        {
            var members = Reader.ReadMembers("ClariusLabs.DemoProject.xml");

            foreach (var member in members)
            {
                member.Accept(new SampleVisitor());
            }
        }

        public class SampleVisitor : Visitor
        {
            public override void VisitMember(Member member)
            {
                Console.WriteLine();
                Console.WriteLine(new string('-', 50));
                Console.WriteLine("# " + member.Id);
                base.VisitMember(member);
            }

            public override void VisitSummary(Summary summary)
            {
                Console.WriteLine();
                Console.WriteLine("## Summary");
                base.VisitSummary(summary);
            }

            public override void VisitRemarks(Remarks remarks)
            {
                Console.WriteLine();
                Console.WriteLine("## Remarks");
                base.VisitRemarks(remarks);
            }

            public override void VisitExample(Example example)
            {
                Console.WriteLine();
                Console.WriteLine("### Example");
                base.VisitExample(example);
            }

            public override void VisitC(C code)
            {
                Console.Write(" `");
                Console.Write(code.Content);
                Console.Write("` ");

                base.VisitC(code);
            }

            public override void VisitCode(Code code)
            {
                Console.WriteLine();
                Console.WriteLine();

                foreach (var line in code.Content.Split(new[] { Environment.NewLine }, StringSplitOptions.None))
                {
                    Console.Write("    ");
                    Console.WriteLine(line);
                }

                Console.WriteLine();
                base.VisitCode(code);
            }

            public override void VisitText(Text text)
            {
                Console.Write(text.Content);
                base.VisitText(text);
            }

            public override void VisitPara(Para para)
            {
                Console.WriteLine();
                Console.WriteLine();
                base.VisitPara(para);
                Console.WriteLine();
                Console.WriteLine();
            }

            public override void VisitSee(See see)
            {
                var cref = NormalizeLink(see.Cref);
                Console.Write(" [{0}]({1}) ", cref.Substring(2), cref);
            }

            public override void VisitSeeAlso(SeeAlso seeAlso)
            {
                var cref = NormalizeLink(seeAlso.Cref);
                Console.WriteLine("[{0}]({1})", cref.Substring(2), cref);
            }

            private string NormalizeLink(string cref)
            {
                return cref.Replace(":", "-").Replace("(", "-").Replace(")", "");
            }
        }
    }
}