namespace Markdown.ApiSite
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    public static class DocReader
    {
        public static IEnumerable<DocMember> ReadMembers(string fileName)
        {
            var doc = XDocument.Load(fileName);

            return doc.Root.Element("members").Elements("member")
                .Select(e => new DocMember(e.Attribute("name").Value, ReadContent(e)));
        }

        private static IEnumerable<DocElement> ReadContent(XElement element)
        {
            foreach (var node in element.Nodes())
            {
                switch (node.NodeType)
                {
                    case System.Xml.XmlNodeType.Element:
                        var elementNode = (XElement)node;
                        switch (elementNode.Name.LocalName)
                        {
                            case "summary":
                                yield return new DocSummary(ReadContent(elementNode));
                                break;
                            case "remarks":
                                yield return new DocRemarks(ReadContent(elementNode));
                                break;
                            case "example":
                                yield return new DocExample(ReadContent(elementNode));
                                break;
                            case "para":
                                yield return new DocPara(ReadContent(elementNode));
                                break;
                            case "code":
                                yield return new DocCode(SmartTrimCode(elementNode.Value));
                                break;
                            case "c":
                                yield return new DocC(elementNode.Value);
                                break;
                            case "see":
                                yield return new DocSee(elementNode.Attribute("cref").Value);
                                break;
                            case "seealso":
                                yield return new DocSeeAlso(elementNode.Attribute("cref").Value);
                                break;
                            default:
                                break;
                        }
                        break;
                    case System.Xml.XmlNodeType.Text:
                        yield return new DocText(SmartTrimText(((XText)node).Value));
                        break;
                    default:
                        break;
                }

            }
        }

        private static string SmartTrimText(string text)
        {
            // New lines in doc comments should be removed since 
            // that's the purpose of <para>. We should remove new lines, 
            // trim the results and join the strings with a simple whitespace.
            return string.Join(" ", text
                .Split(new[] { Environment.NewLine, "\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim()));
        }

        private static string SmartTrimCode(string code)
        {
            // In code blocks, the indent of the first line of code determines the base 
            // indent for all the lines, which we should remove. New lines should remain 
            // as-is, as well as any further spacing.
            var lines = code.Split(new[] { Environment.NewLine, "\n" }, StringSplitOptions.None).ToList();

            if (lines.Count == 0)
                return string.Empty;

            // Remove leading and trailing empty lines which are used for wrapping in the doc XML.
            if (string.IsNullOrWhiteSpace(lines[0]))
                lines.RemoveAt(0);

            if (lines.Count == 0)
                return string.Empty;

            if (string.IsNullOrWhiteSpace(lines[lines.Count - 1]))
                lines.RemoveAt(lines.Count - 1);

            if (lines.Count == 0)
                return string.Empty;

            var indent = lines[0].TakeWhile(c => char.IsWhiteSpace(c)).Count();

            return string.Join(" ", lines
                .Select(line => line.Substring(indent)));
        }
    }

    public class DocVisitor
    {
        public virtual void VisitMember(DocMember member)
        {
        }

        public virtual void VisitSummary(DocSummary summary)
        {
        }

        public virtual void VisitRemarks(DocRemarks remarks)
        {
        }

        public virtual void VisitPara(DocPara para)
        {
        }

        public virtual void VisitCode(DocCode code)
        {
        }

        public virtual void VisitC(DocC code)
        {
        }

        public virtual void VisitText(DocText text)
        {
        }

        public virtual void VisitExample(DocExample example)
        {
        }

        public virtual void VisitSee(DocSee see)
        {
        }

        public virtual void VisitSeeAlso(DocSeeAlso seeAlso)
        {
        }
    }

    public abstract class DocElement
    {
        public abstract void Accept(DocVisitor visitor);
    }

    public abstract class DocContainer : DocElement
    {
        public DocContainer(IEnumerable<DocElement> elements)
        {
            this.Elements = elements;
        }

        public IEnumerable<DocElement> Elements { get; private set; }

        public override void Accept(DocVisitor visitor)
        {
            OnAccept(visitor);
            foreach (var element in Elements)
            {
                element.Accept(visitor);
            }
        }

        protected abstract void OnAccept(DocVisitor visitor);
    }

    public class DocSummary : DocContainer
    {
        public DocSummary(IEnumerable<DocElement> elements)
            : base(elements)
        {
        }

        protected override void OnAccept(DocVisitor visitor)
        {
            visitor.VisitSummary(this);
        }
    }

    public class DocRemarks : DocContainer
    {
        public DocRemarks(IEnumerable<DocElement> elements)
            : base(elements)
        {
        }

        protected override void OnAccept(DocVisitor visitor)
        {
            visitor.VisitRemarks(this);
        }
    }

    public class DocPara : DocContainer
    {
        public DocPara(IEnumerable<DocElement> elements)
            : base(elements)
        {
        }

        protected override void OnAccept(DocVisitor visitor)
        {
            visitor.VisitPara(this);
        }
    }

    public class DocExample : DocContainer
    {
        public DocExample(IEnumerable<DocElement> elements)
            : base(elements)
        {
        }

        protected override void OnAccept(DocVisitor visitor)
        {
            visitor.VisitExample(this);
        }
    }

    public class DocCode : DocElement
    {
        public DocCode(string code)
        {
            this.Code = code;
        }

        public override void Accept(DocVisitor visitor)
        {
            visitor.VisitCode(this);
        }

        public string Code { get; private set; }
    }

    public class DocC : DocElement
    {
        public DocC(string code)
        {
            this.Code = code;
        }

        public override void Accept(DocVisitor visitor)
        {
            visitor.VisitC(this);
        }

        public string Code { get; private set; }
    }

    public class DocText : DocElement
    {
        public DocText(string text)
        {
            this.Text = text;
        }

        public override void Accept(DocVisitor visitor)
        {
            visitor.VisitText(this);
        }

        public string Text { get; private set; }
    }

    public class DocSee : DocElement
    {
        public DocSee(string cref)
        {
            this.Cref = cref;
        }

        public override void Accept(DocVisitor visitor)
        {
            visitor.VisitSee(this);
        }

        public string Cref { get; private set; }
    }

    public class DocSeeAlso : DocElement
    {
        public DocSeeAlso(string cref)
        {
            this.Cref = cref;
        }

        public override void Accept(DocVisitor visitor)
        {
            visitor.VisitSeeAlso(this);
        }

        public string Cref { get; private set; }
    }
}