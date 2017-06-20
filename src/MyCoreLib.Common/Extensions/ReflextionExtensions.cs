using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MyCoreLib.Common.Helper
{
    public static class ReflextionExtensions
    {
        #region member info
        public static MemberInfo[] GetMemberInfo<TEntity>(this TEntity entity, string memberName, MemberTypes type = MemberTypes.All, BindingFlags bindingAttr = BindingFlags.Default | BindingFlags.Instance)
        {
            TypeInfo _info = entity.GetType().GetTypeInfo();
            return _info.GetMember(memberName, type, bindingAttr);
        }
        public static MemberInfo[] GetMemberInfos<TEntity>(this TEntity entity, BindingFlags bindingAttr = BindingFlags.Default | BindingFlags.Instance)
        {
            TypeInfo _info = entity.GetType().GetTypeInfo();
            return _info.GetMembers(bindingAttr);
        }
        #endregion

        #region method info
        public static MethodInfo GetMethodInfo<TEntity>(this TEntity entity, string methodName, BindingFlags bindingAttr = BindingFlags.Default | BindingFlags.Instance)
        {
            TypeInfo _info = entity.GetType().GetTypeInfo();
            return _info.GetMethod(methodName, bindingAttr);
        }
        public static MethodInfo GetMethodInfo<TEntity>(this TEntity entity, string methodName, Type[] types, ParameterModifier[] modifiers = null)
        {
            TypeInfo _info = entity.GetType().GetTypeInfo();
            return _info.GetMethod(methodName, types, modifiers);
        }

        public static MethodInfo[] GetMethodInfos<TEntity>(this TEntity entity, BindingFlags bindingAttr = BindingFlags.Default | BindingFlags.Instance)
        {
            TypeInfo _info = entity.GetType().GetTypeInfo();
            return _info.GetMethods(bindingAttr);
        }
        #endregion

        #region field info 
        public static FieldInfo GetFieldInfo<TEntity>(this TEntity entity, string name, BindingFlags bindingAttr = BindingFlags.Default | BindingFlags.Instance)
        {
            TypeInfo _info = entity.GetType().GetTypeInfo();
            return _info.GetField(name, bindingAttr);
        }
        public static FieldInfo[] GetFieldInfos<TEntity>(this TEntity entity, BindingFlags bindingAttr = BindingFlags.Default | BindingFlags.Instance)
        {
            TypeInfo _info = entity.GetType().GetTypeInfo();
            return _info.GetFields(bindingAttr);
        }
        #endregion

        #region property info 

        public static PropertyInfo[] GetPropertieInfos<TEntity>(this TEntity entity, BindingFlags bindingAttr = BindingFlags.Default | BindingFlags.Instance)
        {
            TypeInfo _info = entity.GetType().GetTypeInfo();
            return _info.GetProperties(bindingAttr);
        }
        public static PropertyInfo GetPropertyInfo<TEntity>(this TEntity entity, string name, Type[] types = null)
        {
            TypeInfo _info = entity.GetType().GetTypeInfo();
            return _info.GetProperty(name, types);
        }
        public static PropertyInfo GetPropertyInfo<TEntity>(this TEntity entity, string name, Type returnType, Type[] types = null, ParameterModifier[] modifiers = null)
        {
            TypeInfo _info = entity.GetType().GetTypeInfo();
            return _info.GetProperty(name, returnType, types, modifiers);
        }
        public static PropertyInfo GetPropertyInfo<TEntity>(this TEntity entity, string name, BindingFlags bindingAttr = BindingFlags.Default | BindingFlags.Instance)
        {
            TypeInfo _info = entity.GetType().GetTypeInfo();
            return _info.GetProperty(name, bindingAttr);
        }

        #endregion
    }
}
