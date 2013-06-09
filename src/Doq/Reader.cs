namespace ClariusLabs.Doq
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;

    public static class Reader
    {
        public static IEnumerable<Member> ReadMembers(string fileName)
        {
            if (!File.Exists(fileName))
                throw new FileNotFoundException("Could not find documentation file to load.", fileName);

            var doc = XDocument.Load(fileName);

            return doc.Root.Element("members").Elements("member")
                .Select(e => CreateMember(e.Attribute("name").Value, ReadContent(e)));
        }

        // TODO: support multiple assemblies.
        public static IEnumerable<Member> ReadMembers(Assembly assembly)
        {
            var fileName = Path.ChangeExtension(assembly.Location, ".xml");
            if (!File.Exists(fileName))
                throw new FileNotFoundException("Could not find documentation file to load.", fileName);

            var doc = XDocument.Load(fileName);
            var map = new MemberIdMap();
            map.Add(assembly);

            return doc.Root.Element("members").Elements("member")
                .Select(element => CreateMember(element.Attribute("name").Value, ReadContent(element)))
                .Select(member => ReplaceExtensionMethods(member, map))
                .Select(member => ReplaceTypes(member, map))
                .Select(member => SetInfo(member, map));
        }

        private static Member SetInfo(Member member, MemberIdMap map)
        {
            member.Info = map.GetOrDefault(member.ToString());

            return member;
        }

        private static Member ReplaceTypes(Member member, MemberIdMap map)
        {
            if (member.Kind != MemberKinds.Type)
                return member;

            var type = (Type)map.GetOrDefault(member.ToString());
            if (type == null)
                return member;

            var nestingTypeId = "";
            if (type.DeclaringType != null &&
                !string.IsNullOrEmpty((nestingTypeId = map.GetOrDefault(type.DeclaringType))))
                return new NestedType(member.ToString(), nestingTypeId, member.Elements);

            if (type.IsInterface)
                return new Interface(member.ToString(), member.Elements);
            if (type.IsClass)
                return new Class(member.ToString(), member.Elements);
            if (type.IsEnum)
                return new Enum(member.ToString(), member.Elements);
            if (type.IsValueType)
                return new Struct(member.ToString(), member.Elements);

            return member;
        }

        private static Member ReplaceExtensionMethods(Member member, MemberIdMap map)
        {
            if (member.Kind != MemberKinds.Method)
                return member;

            var method = (MethodBase)map.GetOrDefault(member.ToString());
            if (method == null)
                return member;

            if (method.GetCustomAttributes(true).Any(attr => attr.GetType().FullName == "System.Runtime.CompilerServices.ExtensionAttribute"))
            {
                var extendedTypeId = map.GetOrDefault(method.GetParameters()[0].ParameterType);
                if (!string.IsNullOrEmpty(extendedTypeId))
                    return new ExtensionMethod(member.ToString(), extendedTypeId, member.Elements);
            }

            return member;
        }

        private static Member CreateMember(string memberId, IEnumerable<Element> elements)
        {
            switch (memberId[0])
            {
                case 'T':
                    return new TypeDeclaration(memberId, elements);
                case 'F':
                    return new Field(memberId, elements);
                case 'P':
                    return new Property(memberId, elements);
                case 'M':
                    return new Method(memberId, elements);
                case 'E':
                    return new Event(memberId, elements);
                default:
                    return new UnknownMember(memberId, elements);
            }
        }

        private static IEnumerable<Element> ReadContent(XElement element)
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
                                yield return new Summary(ReadContent(elementNode));
                                break;
                            case "remarks":
                                yield return new Remarks(ReadContent(elementNode));
                                break;
                            case "example":
                                yield return new Example(ReadContent(elementNode));
                                break;
                            case "para":
                                yield return new Para(ReadContent(elementNode));
                                break;
                            case "code":
                                yield return new Code(SmartTrimCode(elementNode.Value));
                                break;
                            case "c":
                                yield return new C(elementNode.Value);
                                break;
                            case "see":
                                yield return new See(elementNode.Attribute("cref").Value);
                                break;
                            case "seealso":
                                yield return new SeeAlso(elementNode.Attribute("cref").Value);
                                break;
                            default:
                                break;
                        }
                        break;
                    case System.Xml.XmlNodeType.Text:
                        yield return new Text(SmartTrimText(((XText)node).Value));
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
                .Select(s => s.Trim())
                .ToArray());
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
            if (lines[0].Trim().Length == 0)
                lines.RemoveAt(0);

            if (lines.Count == 0)
                return string.Empty;

            if (lines[lines.Count - 1].Trim().Length == 0)
                lines.RemoveAt(lines.Count - 1);

            if (lines.Count == 0)
                return string.Empty;

            var indent = lines[0].TakeWhile(c => char.IsWhiteSpace(c)).Count();

            return string.Join(" ", lines
                .Select(line => line.Substring(indent))
                .ToArray());
        }
    }
}