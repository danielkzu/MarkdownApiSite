#region Apache Licensed
/*
 Copyright 2013 Clarius Consulting, Daniel Cazzulino

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/
#endregion

namespace ClariusLabs.Doq
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    internal class MemberIdMap
    {
        internal readonly Dictionary<string, MemberInfo> idToMemberMap = new Dictionary<string, MemberInfo>();
        internal readonly Dictionary<MemberInfo, string> memberToIdMap = new Dictionary<MemberInfo, string>();
        private readonly StringBuilder sb = new StringBuilder();

        public void Add(Assembly assembly)
        {
            AddRange(assembly.GetTypes());
        }

        public void AddRange(IEnumerable<System.Type> types)
        {
            foreach (var type in types)
            {
                Add(type);
            }
        }

        public void Add(System.Type type)
        {
            var members = type.GetMembers(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);

            foreach (var member in members)
            {
                Add(type, member);
            }
        }

        public MemberInfo GetOrDefault(string memberId)
        {
            MemberInfo result = null;
            idToMemberMap.TryGetValue(memberId, out result);
            return result;
        }

        public string GetOrDefault(MemberInfo member)
        {
            string result = null;
            memberToIdMap.TryGetValue(member, out result);
            return result;
        }

        private void Add(System.Type type, MemberInfo member)
        {
            System.Type nestedType = null;

            sb.Length = 0;

            switch (member.MemberType)
            {
                case MemberTypes.Constructor:
                    sb.Append("M:");
                    Append((ConstructorInfo)member);
                    break;
                case MemberTypes.Event:
                    sb.Append("E:");
                    Append((EventInfo)member);
                    break;
                case MemberTypes.Field:
                    sb.Append("F:");
                    Append((FieldInfo)member);
                    break;
                case MemberTypes.Method:
                    sb.Append("M:");
                    Append((MethodInfo)member);
                    break;
                case MemberTypes.NestedType:
                    nestedType = (System.Type)member;
                    sb.Append("T:");
                    AppendNestedType(nestedType);
                    break;
                case MemberTypes.Property:
                    sb.Append("P:");
                    Append((PropertyInfo)member);
                    break;
            }

            if (sb.Length > 0)
            {
                idToMemberMap[sb.ToString()] = member;
                memberToIdMap[member] = sb.ToString();
            }

            if (nestedType != null)
            {
                Add(nestedType);
            }
        }

        private void Append(PropertyInfo property)
        {
            AppendType(sb, property.DeclaringType);
            sb.Append('.').Append(property.Name);
        }

        private void Append(MethodInfo method)
        {
            if (method.IsSpecialName || method.IsPrivate)
            {
                sb.Length = 0;
                return;
            }

            AppendType(sb, method.DeclaringType);
            sb.Append('.').Append(method.Name);
            Append(method.GetParameters());
        }

        private void Append(ParameterInfo[] parameters)
        {
            if (parameters.Length == 0)
            {
                return;
            }
            sb.Append('(');
            for (int i = 0; i < parameters.Length; i++)
            {
                if (i > 0)
                {
                    sb.Append(',');
                }
                var p = parameters[i];
                AppendType(sb, p.ParameterType);
            }
            sb.Append(')');
        }

        private void Append(FieldInfo field)
        {
            if (field.IsPrivate)
            {
                sb.Length = 0;
                return;
            }

            AppendType(sb, field.DeclaringType);
            sb.Append('.').Append(field.Name);
        }

        private void Append(EventInfo @event)
        {
            AppendType(sb, @event.DeclaringType);
            sb.Append('.').Append(@event.Name);
        }

        private void Append(ConstructorInfo constructor)
        {
            AppendType(sb, constructor.DeclaringType);
            sb.Append('.').Append("#ctor");
            Append(constructor.GetParameters());
        }

        private void AppendNestedType(System.Type type)
        {
            if (type.IsNestedPrivate)
            {
                sb.Length = 0;
                return;
            }

            AppendType(sb, type);
        }

        private void AppendType(StringBuilder sb, System.Type type)
        {
            if (type.DeclaringType != null)
            {
                AppendType(sb, type.DeclaringType);
                sb.Append('.');
            }
            else if (!string.IsNullOrEmpty(type.Namespace))
            {
                sb.Append(type.Namespace);
                sb.Append('.');
            }
            sb.Append(type.Name);
            if (type.IsGenericType && !type.IsGenericTypeDefinition)
            {
                // Remove "`1" suffix from type name
                while (char.IsDigit(sb[sb.Length - 1]))
                    sb.Length--;
                sb.Length--;
                {
                    var args = type.GetGenericArguments();
                    sb.Append('{');
                    for (int i = 0; i < args.Length; i++)
                    {
                        if (i > 0)
                        {
                            sb.Append(',');
                        }
                        AppendType(sb, args[i]);
                    }
                    sb.Append('}');
                }
            }

            // Always ensure the owning type is added for 
            // any member id in use.s
            var typeId = "T:" + sb.ToString().Substring(2);
            idToMemberMap[typeId] = type;
            memberToIdMap[type] = typeId;
        }
    }
}