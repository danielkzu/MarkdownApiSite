namespace ClariusLabs.Doq
{
    public class Visitor
    {
        public virtual void VisitType(TypeDeclaration type)
        {
            VisitMember(type);
        }

        public virtual void VisitNestedType(NestedType type)
        {
            VisitType(type);
        }

        public virtual void VisitInterface(Interface type)
        {
            VisitType(type);
        }
        
        public virtual void VisitClass(Class type)
        {
            VisitType(type);
        }

        public virtual void VisitStruct(Struct type)
        {
            VisitType(type);
        }

        public virtual void VisitEnum(Enum type)
        {
            VisitType(type);
        }

        public virtual void VisitField(Field field)
        {
            VisitMember(field);
        }

        public virtual void VisitProperty(Property property)
        {
            VisitMember(property);
        }

        public virtual void VisitEvent(Event @event)
        {
            VisitMember(@event);
        }

        public virtual void VisitMethod(Method method)
        {
            VisitMember(method);
        }

        public virtual void VisitExtensionMethod(ExtensionMethod method)
        {
            VisitMember(method);
        }

        public virtual void VisitMember(Member member)
        {
            VisitElements(member);
        }

        public virtual void VisitSummary(Summary summary)
        {
            VisitElements(summary);
        }

        public virtual void VisitRemarks(Remarks remarks)
        {
            VisitElements(remarks);
        }

        public virtual void VisitPara(Para para)
        {
            VisitElements(para);
        }

        public virtual void VisitCode(Code code)
        {
        }

        public virtual void VisitC(C code)
        {
        }

        public virtual void VisitText(Text text)
        {
        }

        public virtual void VisitExample(Example example)
        {
            VisitElements(example);
        }

        public virtual void VisitSee(See see)
        {
        }

        public virtual void VisitSeeAlso(SeeAlso seeAlso)
        {
        }

        public virtual void VisitUnknownMember(UnknownMember member)
        {
            VisitElements(member);
        }

        protected virtual void VisitElements(Container container)
        {
            foreach (var element in container.Elements)
            {
                element.Accept(this);
            }
        }
    }
}