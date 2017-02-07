using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MyCoreLib.Data.Entity
{
    internal class ExtendedAttributesCompiler
    {
        private Type _type;
        private HashSet<string> _refAssemblies;
        private static ConcurrentDictionary<Type, IExtendedAttributesFactory> s_cache = new ConcurrentDictionary<Type, IExtendedAttributesFactory>();

        internal ExtendedAttributesCompiler(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");
            this._type = type;
            this._refAssemblies = new HashSet<string>();
        }

        //public static IExtendedAttributesFactory GetAttributesFactory(Type type)
        //{
        //    if (type == null) throw new ArgumentNullException("type");
        //    IExtendedAttributesFactory factory;
        //    if (!s_cache.TryGetValue(type, out factory))
        //    {
        //        ExtendedAttributesCompiler compiler = new ExtendedAttributesCompiler(type);
        //        factory = compiler.CompileFactory();
        //        s_cache.TryAdd(type, factory);
        //    }

        //    return factory;
        //}

        //internal IExtendedAttributesFactory CompileFactory()
        //{
        //    string code = GenerateCode();
        //    Assembly assembly = CompileAssembly(code, _refAssemblies.ToArray());

        //    string FactoryType = string.Format("Weipan.Entity.GeneratedAssembly.{0}AttributesFactory", _type.Name);
        //    Type type = assembly.GetType(FactoryType);
        //    if (type == null)
        //    {
        //        throw new InvalidOperationException(string.Format("Unable to get an instance for IExtendedAttributesFactory for {0}.", _type));
        //    }

        //    return (IExtendedAttributesFactory)Activator.CreateInstance(type);
        //}

        //private string GenerateCode()
        //{
        //    _refAssemblies.Add(typeof(IExtendedAttributesFactory).Assembly.Location);
        //    _refAssemblies.Add(typeof(JsonConvert).Assembly.Location);
        //    _refAssemblies.Add(_type.Assembly.Location);

        //    // There might be '+' in type full name once the class is an inner one within some other class.
        //    string typeFullName = _type.FullName.Replace('+', '.');
        //    AddImport(_type);

        //    using (StringWriter sw = new StringWriter())
        //    {
        //        IndentedTextWriter tw = new IndentedTextWriter(sw);
        //        tw.WriteLine("namespace Weipan.Entity.GeneratedAssembly");
        //        tw.WriteLine("{");
        //        tw.Indent++;
        //        {
        //            // Generate the attribute class
        //            tw.WriteLine("public class {0}Attributes : {1}", _type.Name, typeof(IExtendedAttributes).FullName);
        //            tw.WriteLine("{");
        //            tw.Indent++;
        //            {
        //                var list = new List<PropertyDescriptor>();
        //                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(_type);
        //                if (properties != null && properties.Count > 0)
        //                {
        //                    foreach (PropertyDescriptor p in properties)
        //                    {
        //                        if (p.Attributes == null)
        //                        {
        //                            continue;
        //                        }

        //                        foreach (Attribute att in p.Attributes)
        //                        {
        //                            if (att.GetType() == typeof(ExtendedAttributeAttribute))
        //                            {
        //                                ExtendedAttributeAttribute extendedAtt = (ExtendedAttributeAttribute)att;
        //                                list.Add(p);
        //                                break;
        //                            }
        //                        }
        //                    }
        //                }

        //                // Generate a list of properties
        //                foreach (PropertyDescriptor p in list)
        //                {
        //                    tw.Write("public {0} {1} ", FormatTypeName(p.PropertyType), p.Name);
        //                    tw.WriteLine("{ get; set; }");
        //                    AddImport(p.PropertyType);
        //                }

        //                // Generate the default constructors
        //                tw.WriteLine();
        //                tw.Write("public {0}Attributes()", _type.Name);
        //                tw.WriteLine("{ }");

        //                // Generate a constructor which takes the entity as the parameter
        //                tw.WriteLine();
        //                tw.WriteLine("public {0}Attributes({1} entity)", _type.Name, typeFullName);
        //                tw.WriteLine("{");
        //                tw.Indent++;
        //                foreach (PropertyDescriptor p in list)
        //                {
        //                    tw.WriteLine("this.{0} = entity.{0};", p.Name);
        //                }
        //                tw.Indent--;
        //                tw.WriteLine("}");

        //                // Generate method FillEntity
        //                tw.WriteLine();
        //                tw.WriteLine("public void FillEntity({0} entity)", typeFullName);
        //                tw.WriteLine("{");
        //                tw.Indent++;
        //                foreach (PropertyDescriptor p in list)
        //                {
        //                    tw.WriteLine("entity.{0} = this.{0};", p.Name);
        //                }
        //                tw.Indent--;
        //                tw.WriteLine("}");
        //            }
        //            tw.Indent--;
        //            tw.WriteLine("}");

        //            // Then to generate the factory class.
        //            #region factory code
        //            tw.WriteLine("public class {0}AttributesFactory : {1}", _type.Name, typeof(IExtendedAttributesFactory).FullName);
        //            tw.WriteLine("{");
        //            tw.Indent++;
        //            {
        //                // DeserializeJson method
        //                tw.WriteLine("public {0} DeserializeJson(string json, Newtonsoft.Json.JsonSerializerSettings settings)", typeof(IExtendedAttributes).FullName);
        //                tw.WriteLine("{");
        //                tw.Indent++;
        //                tw.WriteLine("return Newtonsoft.Json.JsonConvert.DeserializeObject<{0}Attributes>(json, settings);", _type.Name);
        //                tw.Indent--;
        //                tw.WriteLine("}");

        //                // CreateAttributes method
        //                tw.WriteLine("public {0} CreateAttributes({1} entity)", typeof(IExtendedAttributes).FullName, typeof(ExtendableEntity).FullName);
        //                tw.WriteLine("{");
        //                tw.Indent++;
        //                tw.WriteLine("return new {0}Attributes(({1})entity);", _type.Name, typeFullName);
        //                tw.Indent--;
        //                tw.WriteLine("}");

        //                // FillEntity method
        //                tw.WriteLine("public void FillEntity({1} entity, {0} attrs)", typeof(IExtendedAttributes).FullName, typeof(ExtendableEntity).FullName);
        //                tw.WriteLine("{");
        //                tw.Indent++;
        //                tw.WriteLine("(({0}Attributes)attrs).FillEntity(({1})entity);", _type.Name, typeFullName);
        //                tw.Indent--;
        //                tw.WriteLine("}");
        //            }
        //            tw.Indent--;
        //            tw.WriteLine("}");
        //            #endregion
        //        }
        //        tw.Indent--;
        //        tw.WriteLine("}");

        //        return sw.ToString();
        //    }
        //}

        //internal static string FormatTypeName(Type type)
        //{
        //    if (!type.IsGenericType)
        //        return type.FullName.Replace('+', '.');


        //    StringBuilder sb = new StringBuilder();
        //    sb.Append(type.Namespace);
        //    sb.Append('.');

        //    int pos = type.Name.IndexOf('`');
        //    sb.Append(type.Name.Substring(0, pos));
        //    sb.Append('<');
        //    sb.Append(FormatTypeName(type.GenericTypeArguments[0]));
        //    for (int i = 1; i < type.GenericTypeArguments.Length; i++)
        //    {
        //        sb.Append(',');
        //        sb.Append(FormatTypeName(type.GenericTypeArguments[i]));
        //    }
        //    sb.Append('>');

        //    return sb.ToString();
        //}

        //private Assembly CompileAssembly(string code, string[] assemblies)
        //{
        //    using (CodeDomProvider provider = new CSharpCodeProvider())
        //    {
        //        CompilerParameters codeDomParameters = new CompilerParameters { GenerateInMemory = true };
        //        if (assemblies != null && assemblies.Length > 0)
        //        {
        //            codeDomParameters.ReferencedAssemblies.AddRange(assemblies);
        //        }

        //        CompilerResults results = provider.CompileAssemblyFromSource(codeDomParameters, code);
        //        if (results.Errors.Count > 0)
        //        {
        //            StringWriter writer = new StringWriter();
        //            writer.WriteLine("Failed to generate attribute class for {0}.", _type);
        //            foreach (CompilerError err in results.Errors)
        //            {
        //                // if ( !err.IsWarning ) // TODO: Do we need to skip warinings?
        //                writer.WriteLine(err);
        //            }

        //            throw new InvalidOperationException(writer.ToString());
        //        }

        //        return results.CompiledAssembly;
        //    }
        //}

        //private void AddImport(Type type)
        //{
        //    if (!IsKnownType(type))
        //    {
        //        _refAssemblies.Add(type.Assembly.Location);
        //    }
        //}

        //private static bool IsKnownType(Type type)
        //{
        //    if (type == typeof(object))
        //    {
        //        return true;
        //    }
        //    if (type.IsEnum)
        //    {
        //        return false;
        //    }
        //    switch (Type.GetTypeCode(type))
        //    {
        //        case TypeCode.Boolean:
        //            return true;

        //        case TypeCode.Char:
        //            return true;

        //        case TypeCode.SByte:
        //            return true;

        //        case TypeCode.Byte:
        //            return true;

        //        case TypeCode.Int16:
        //            return true;

        //        case TypeCode.UInt16:
        //            return true;

        //        case TypeCode.Int32:
        //            return true;

        //        case TypeCode.UInt32:
        //            return true;

        //        case TypeCode.Int64:
        //            return true;

        //        case TypeCode.UInt64:
        //            return true;

        //        case TypeCode.Single:
        //            return true;

        //        case TypeCode.Double:
        //            return true;

        //        case TypeCode.Decimal:
        //            return true;

        //        case TypeCode.DateTime:
        //            return true;

        //        case TypeCode.String:
        //            return true;
        //    }
        //    return (type == typeof(byte[])) || (type == typeof(Guid));
        //}
    }
}
