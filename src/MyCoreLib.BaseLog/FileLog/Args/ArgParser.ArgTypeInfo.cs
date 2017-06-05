using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace MyCoreLib.BaseLog.FileLog.Args
{
    partial class ArgParser
    {
        #region Nested type: ArgTypeInfo

        private class ArgTypeInfo
        {
            private static string[] s_helpSwitchNames = new[]
            {
                "?", "h", "help"
            };

            private Dictionary<string, ArgAttribute> _longNameMap;
            private Dictionary<string, MemberInfo> _memberInfoMap;
            private Dictionary<string, ArgAttribute> _shortNameMap;

            private ArgTypeInfo(Type optionType)
            {
                if (optionType == null)
                {
                    throw new ArgumentNullException("optionType");
                }

                OptionType = optionType;
                _longNameMap = new Dictionary<string, ArgAttribute>();
                _shortNameMap = new Dictionary<string, ArgAttribute>();
                _memberInfoMap = new Dictionary<string, MemberInfo>();
            }

            /// <summary>
            /// Get an array of switch names for requesting usage.
            /// </summary>
            public static string[] HelpSwitchNames
            {
                get { return (string[])s_helpSwitchNames.Clone(); }
            }

            public Type OptionType
            {
                get;
                private set;
            }

            public static ArgTypeInfo Create(Type optionType)
            {
                var t = new ArgTypeInfo(optionType);
                t.Initialize();

                return t;
            }

            public static bool IsHelpSwitch(string name)
            {
                foreach (string s in s_helpSwitchNames)
                {
                    if (string.Compare(name, s) == 0)
                    {
                        return true;
                    }
                }

                return false;
            }

            /// <summary>
            /// Get an array of required switch names.
            /// </summary>
            public string[] GetRequiredSwitches()
            {
                var list = new List<string>(_longNameMap.Count);
                foreach (ArgAttribute arg in _longNameMap.Values)
                {
                    if (arg.Required)
                    {
                        list.Add(arg.Name);
                    }
                }

                return list.ToArray();
            }

            public MemberInfo LookupMember(string name, bool switchIncludesAValue, out ArgAttribute argAttribute)
            {
                // Does the switch name exist? 
                if (!_longNameMap.TryGetValue(name, out argAttribute))
                {
                    // Maybe a short name?
                    if (!_shortNameMap.TryGetValue(name, out argAttribute))
                    {
                        // No members match, we have an invalid argument.
                        throw new InvalidArgException(name);
                    }

                    name = argAttribute.Name;
                }

                ValidateRequiredValue(argAttribute, switchIncludesAValue, name);
                return _memberInfoMap[name];
            }

            /// <summary>
            /// Initialize this instance.
            /// </summary>
            private void Initialize()
            {
                foreach (MemberInfo mi in OptionType.GetMembers(ArgBindingFlags))
                {
                    var argAttribute = mi.GetCustomAttribute<ArgAttribute>();
                    if (argAttribute == null)
                    {
                        continue;
                    }

                    // If this switch name was already defined, throw an exception
                    if (string.IsNullOrEmpty(argAttribute.Name))
                    {
                        argAttribute.Name = mi.Name;
                    }
                    if (_longNameMap.ContainsKey(argAttribute.Name) || _shortNameMap.ContainsKey(argAttribute.Name))
                    {
                        throw new InvalidArgException(
                            argAttribute.Name,
                            string.Format("The '{0}' switch appears more than once in the '{1}' class.", argAttribute.Name, OptionType));
                    }
                    if (IsHelpSwitch(argAttribute.Name))
                    {
                        throw new InvalidArgException(
                            argAttribute.Name,
                            string.Format("The '{0}' switch is an reserved one.", argAttribute.Name));
                    }
                    _longNameMap.Add(argAttribute.Name, argAttribute);
                    _memberInfoMap.Add(argAttribute.Name, mi);

                    // The short switch name should also be unique.
                    string shortName = argAttribute.ShortName;
                    if (!string.IsNullOrEmpty(shortName))
                    {
                        if (_longNameMap.ContainsKey(shortName) || _shortNameMap.ContainsKey(shortName))
                        {
                            throw new InvalidArgException(
                                shortName,
                                string.Format("The '{0}' switch appears more than once in the '{1}' class.", shortName, OptionType));
                        }
                        if (IsHelpSwitch(shortName))
                        {
                            throw new InvalidArgException(
                                argAttribute.Name,
                                string.Format("The '{0}' switch is an reserved one.", shortName));
                        }
                        _shortNameMap.Add(shortName, argAttribute);
                    }
                }
            }

            private static void ValidateRequiredValue(ArgAttribute argAttribute, bool switchIncludesAValue, string name)
            {
                if ((argAttribute.RequiredValue == ArgRequiredValue.Yes) && !switchIncludesAValue)
                {
                    throw new InvalidArgException(
                        name,
                        string.Format("The '{0}' switch requires an argument and none was specified.", name));
                }
                if ((argAttribute.RequiredValue == ArgRequiredValue.No) && switchIncludesAValue)
                {
                    throw new InvalidArgException(
                        name,
                        string.Format("The {0} switch cannot have an argument and one was specified.", name));
                }
            }
        }

        #endregion
    }
}