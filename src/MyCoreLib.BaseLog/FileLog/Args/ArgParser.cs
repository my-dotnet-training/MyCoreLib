#define ARG_IGNORECASE

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace MyCoreLib.BaseLog.FileLog.Args
{
    public sealed partial class ArgParser
    {
        private const char AltPromptChar = '-';
        private const char PromptChar = '/';
        private const BindingFlags ArgBindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase;

        /// <summary>
        /// Get the usage information gathered from the specified type.
        /// </summary>
        public static string Usage(ICmdArgs cmdArgObj)
        {
            if (cmdArgObj == null)
            {
                throw new ArgumentNullException("cmdArgObj");
            }

            var assembly = Assembly.GetEntryAssembly();
            var cmdLine = new StringBuilder();

            // The name of this tool.
            cmdLine.Append(Path.GetFileNameWithoutExtension(assembly.Location));

            #region collect argument list
            var argList = new SortedList<ArgAttribute, Type>(new ArgAttributeComparer());
            foreach (MemberInfo mi in cmdArgObj.GetType().GetMembers(ArgBindingFlags))
            {
                ArgAttribute argAttribute = mi.GetCustomAttribute<ArgAttribute>();

                // If the member doesn't have a ArgAttribute applied to it, or it's invisible, ignore it
                if (argAttribute != null && argAttribute.Visible)
                {
                    // If no attribute-given name specified, use the member's name
                    if (string.IsNullOrEmpty(argAttribute.Name))
                    {
                        argAttribute.Name = mi.Name;
                    }

                    switch (argAttribute.RequiredValue)
                    {
                        case ArgRequiredValue.No:
                            // This switch must NOT have a value, the switch's type must be bool.
#if DEBUG
                            if (typeof(bool) != GetFieldOrPropertyMemberType(mi))
                            {
                                throw new InvalidOperationException(string.Format("Invalid type for '{0}'.", argAttribute.Name));
                            }
#endif
                            argList.Add(argAttribute, null);
                            break;

                        case ArgRequiredValue.Optional:
                        case ArgRequiredValue.Yes:
                            // This switch MAY have a value.
                            argList.Add(argAttribute, GetFieldOrPropertyMemberType(mi));
                            break;

                        default:
                            throw new InvalidOperationException(string.Format("Invalid RequiredValue '{0}' property for '{1}'.", argAttribute.RequiredValue, argAttribute.Name));
                    }

                }
            }
            #endregion collect argument list

            #region print argument names
            // Iterates all the members
            foreach (var kvp in argList)
            {
                ArgAttribute argAttribute = kvp.Key;
                cmdLine.Append(' ');

                // If the switch isn't required, it's optional: put it in square brackets
                if (!argAttribute.Required)
                {
                    cmdLine.Append('[');
                }
                cmdLine.Append(PromptChar);
                cmdLine.Append(argAttribute.Name);

                switch (argAttribute.RequiredValue)
                {
                    case ArgRequiredValue.No:
                        break;

                    case ArgRequiredValue.Yes:
                    case ArgRequiredValue.Optional:
                        // This switch MAY have a value, the switch's type doesn't have to be bool.
                        if (argAttribute.RequiredValue == ArgRequiredValue.Optional)
                        {
                            cmdLine.Append("[:]");
                        }
                        else
                        {
                            cmdLine.Append(':');
                        }
                        break;

                    default:
                        throw new InvalidOperationException(string.Format("Invalid RequiredValue '{0}' property for '{1}'.", argAttribute.RequiredValue, argAttribute.Name));
                }

                if (!argAttribute.Required)
                {
                    cmdLine.Append(']');
                }
            }
            cmdLine.AppendLine();
            cmdLine.AppendLine();
            #endregion print argument names

            #region print argument description
            // Print argument description then
            var descLineWidth = GetWindowWidth() - 4;
            foreach (var kvp in argList)
            {
                cmdLine.Append(PromptChar);
                cmdLine.Append(kvp.Key.Name);
                cmdLine.Append("  ");
                cmdLine.AppendLine();

                var descText = new StringBuilder();
                descText.Append(kvp.Key.Description);

                Type switchType = kvp.Value;
                if (switchType != null && typeof(Enum).IsAssignableFrom(switchType))
                {
                    if (descText.Length > 0)
                    {
                        descText.Append(' ');
                    }
                    descText.Append("Choose from ");
                    var enumValues = Enum.GetValues(switchType);
                    var firstMember = true;
                    foreach (var enumMember in enumValues)
                    {
                        if (!firstMember)
                        {
                            descText.Append(", ");
                        }
                        firstMember = false;

                        descText.Append('\"');
                        descText.Append(enumMember);
                        descText.Append('\"');
                        
                        // Get the description defined for the enum member
                        //var enumValueDesc = (ArgEnumValueDescriptionAttribute)Attribute.GetCustomAttribute(
                        //    switchType.GetField(enumMember.ToString()),
                        //    typeof(ArgEnumValueDescriptionAttribute),
                        //    false);
                        //if (enumValueDesc != null && string.IsNullOrEmpty(enumValueDesc.Description))
                        //{
                        //    descText.Append('(');
                        //    descText.Append(enumValueDesc.Description);
                        //    descText.Append(')');
                        //}
                    }
                    descText.Append('.');
                }

                if (!string.IsNullOrEmpty(kvp.Key.ShortName))
                {
                    if (descText.Length > 0)
                    {
                        descText.Append(' ');
                    }

                    descText.Append("Short form is \"/");
                    descText.Append(kvp.Key.ShortName);
                    descText.Append("\".");
                }

                var lines = BreakStringIntoLinesOfSpecifiedWidth(descText.ToString(), descLineWidth);
                for (var i = 0; i < lines.Length; i++)
                {
                    cmdLine.Append(new string(' ', 4));
                    cmdLine.Append(lines[i]);
                    cmdLine.AppendLine();
                }
                cmdLine.AppendLine();
            }
            #endregion print argument description

            return cmdLine.ToString();
        }

        public static void Parse(ICmdArgs cmdArgObj, string[] args)
        {
            if (cmdArgObj == null)
            {
                throw new ArgumentNullException("cmdArgObj");
            }
            var argType = ArgTypeInfo.Create(cmdArgObj.GetType());
            var array = argType.GetRequiredSwitches();
            var requiredSwitchNames = new List<string>(array);
            foreach (var arg in args)
            {
                if (string.IsNullOrEmpty(arg))
                {
                    continue;
                }

                if (arg[0] == PromptChar || arg[0] == AltPromptChar)
                {
                    // This argument is a switch, process it.
                    string switchName;
                    var switchValue = string.Empty;

                    var colonPos = arg.IndexOf('=');
                    var switchIncludeAValue = (colonPos != -1);
                    if (switchIncludeAValue)
                    {
                        switchName = arg.Substring(1, colonPos - 1);
                        switchValue = arg.Substring(colonPos + 1);
                    }
                    else
                    {
                        switchName = arg.Substring(1);

                        if (ArgTypeInfo.IsHelpSwitch(switchName))
                        {
                            cmdArgObj.ShowUsage = true;
                            continue;
                        }
                    }

                    // Lookup the switch member
                    ArgAttribute argAttribute;
                    MemberInfo mi = argType.LookupMember(switchName, switchIncludeAValue, out argAttribute);
                    if (argAttribute.RequiredValue == ArgRequiredValue.No)
                    {
                        // This switch must NOT have a value. Therefore the switch's type must be bool.
                        if (GetFieldOrPropertyMemberType(mi) != typeof(bool))
                        {
                            throw new InvalidArgException(switchName, string.Format("The '{0}' switch must be of Boolean type.", mi.Name));
                        }

                        // Since the switch is specified, turn this bool to true
                        SetFieldOrPropertyValue(mi, cmdArgObj, true);
                    }
                    else
                    {
                        // This switch MAY have a value.
                        Type switchType = GetFieldOrPropertyMemberType(mi);
                        if (typeof(Enum).IsAssignableFrom(switchType))
                        {
                            try
                            {
#if ARG_IGNORECASE
                                SetFieldOrPropertyValue(mi, cmdArgObj, Enum.Parse(switchType, switchValue, true));
#else
                                SetFieldOrPropertyValue(mi, cmdArgObj, Enum.Parse(switchType, switchValue, false));
#endif
                            }
                            catch (ArgumentException)
                            {
                                if (!CustomAttributeExtensions.IsDefined(switchType.GetTypeInfo(), typeof(FlagsAttribute), false))
                                {
                                    throw new InvalidArgException(
                                        switchName,
                                        string.Format("The '{0}' switch requires one of the following values: {1}.", switchName, string.Join(", ", Enum.GetNames(switchType))));
                                }
                                throw new InvalidArgException(
                                    switchName,
                                    string.Format("The '{0}' switch requires a combination of the following values: {1}.", switchName, string.Join(", ", Enum.GetNames(switchType))));
                            }
                        }
                        else
                        {
                            SetFieldOrPropertyValue(mi, cmdArgObj, Convert.ChangeType(switchValue, switchType));
                        }

                        if (argAttribute.Required)
                        {
                            // If we found a required switch, remove it from the list of required switches.
                            // The list should be empty when done parsing or some required switches weren't
                            // specified by the user.
                            requiredSwitchNames.Remove(argAttribute.Name);
                        }
                    }
                }
                else
                {
                    // This is a free-form command-line argument.
                    // So pass it to the ICmdArgs objects directly.
                    cmdArgObj.ProcessStandAloneArgument(arg);
                }
            }

            if (!cmdArgObj.ShowUsage)
            {
                // We're done parsing command-line arguments, were any required switches unspecified?
                if (requiredSwitchNames.Count > 0)
                {
                    var names = new string[requiredSwitchNames.Count];
                    requiredSwitchNames.CopyTo(names, 0);

                    var joinedNames = string.Join(", ", names);
                    throw new InvalidArgException(
                        joinedNames,
                        string.Format("The following required switch(es) must be specified: {0}.", joinedNames));
                }

                // Let the user's type perform any desired validation
                cmdArgObj.Validate();
            }
        }

        private static string[] BreakStringIntoLinesOfSpecifiedWidth(string s, int width)
        {
            var text = new StringBuilder(s);
            var lines = new List<string>();
            var delimiters = new[] { ' ', '\t', '-', '.' };

            while (text.Length > 0)
            {
                // Remove any delimiters from the start of line
                while (Array.IndexOf(delimiters, text[0]) == 0)
                {
                    text.Remove(0, 1);
                }

                // Grab the maximum number of chars to take from this line
                var workingLine = text.ToString(0, Math.Min(width, text.Length));
                int lastIndex = -1;

                // Get the index of that last delimiter to prevent a work from being splitted into 2 lines.
                if (workingLine.Length >= width)
                {
                    lastIndex = workingLine.LastIndexOfAny(delimiters);
                }
                if (lastIndex == -1)
                {
                    lastIndex = width - 1;
                }

                int numCharsToTake = Math.Min(lastIndex + 1, workingLine.Length);
                lines.Add(workingLine.Substring(0, numCharsToTake));

                text.Remove(0, numCharsToTake);
            }

            return lines.ToArray();
        }

        private static Type GetFieldOrPropertyMemberType(MemberInfo mi)
        {
            if (mi.MemberType == MemberTypes.Field)
            {
                return ((FieldInfo)mi).FieldType;
            }
            if (mi.MemberType == MemberTypes.Property)
            {
                return ((PropertyInfo)mi).PropertyType;
            }

            throw new InvalidOperationException(string.Format("Member '{0}' must be a field or property.", mi.Name));
        }

        private static void SetFieldOrPropertyValue(MemberInfo mi, ICmdArgs cmdArgObj, object value)
        {
            if (mi.MemberType == MemberTypes.Field)
            {
                ((FieldInfo)mi).SetValue(cmdArgObj, value);
                return;
            }
            if (mi.MemberType == MemberTypes.Property)
            {
                ((PropertyInfo)mi).SetValue(cmdArgObj, value, null);
                return;
            }
            throw new InvalidOperationException(string.Format("Member '{0}' must be a field or property.", mi.Name));
        }

        /// <summary>
        /// Gets the width of the current window in characters.
        /// </summary>
        private static int GetWindowWidth()
        {
            try
            {
                return Console.WindowWidth;
            }
            catch (IOException)
            {
                // Maybe not a console window?
                return int.MaxValue;
            }
        }
    }
}