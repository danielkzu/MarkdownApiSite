namespace Markdown.ApiSite
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class DocMember : DocContainer
    {
        public DocMember(string memberId, IEnumerable<DocElement> elements)
            : base(elements)
        {
            switch (memberId[0])
            {
                case 'T':
                    this.Type = MemberType.Type;
                    break;
                case 'F':
                    this.Type = MemberType.Field;
                    break;
                case 'P':
                    this.Type = MemberType.Property;
                    break;
                case 'M':
                    this.Type = MemberType.Method;
                    break;
                case 'E':
                    this.Type = MemberType.Event;
                    break;
                default:
                    this.Type = MemberType.Unknown;
                    break;
            }

            this.Id = memberId.Substring(2);
        }

        public string Id { get; private set; }
        public MemberType Type { get; private set; }

        public override string ToString()
        {
            return this.Type + ": " + this.Id;
        }

        protected override void OnAccept(DocVisitor visitor)
        {
            visitor.VisitMember(this);
        }
    }
}