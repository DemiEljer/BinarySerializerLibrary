using BinarySerializerLibrary.Base;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.ObjectSerializationRecipes.TypesWrapperDelegates
{
    public static class ListTypeWrapperDelegateCollection
    {
        public class ListTypeWrapperDelegates
        {
            public Action<object, object?> AddElementDelegate { get; }

            public ListTypeWrapperDelegates(Type listType)
            {
                AddElementDelegate = MethodAccessDelegateCompiler.CreatePropertySetterDelegate(listType, listType.GetMethod("Add"));
            }
        }

        private static ConcurrentDictionary<Type, ListTypeWrapperDelegates> _WrappedTypes { get; } = new();

        private static ListTypeWrapperDelegates _GetWrapper(Type? listType)
        {
            if (listType is null)
            {
                throw new ArgumentNullException(nameof(listType));
            }

            return _WrappedTypes[listType];
        }

        private static ListTypeWrapperDelegates _GetWrapper(object listObject) => _GetWrapper(listObject?.GetType());

        public static void CookType(Type listType)
        {
            if (!_WrappedTypes.ContainsKey(listType))
            {
                _WrappedTypes.TryAdd(listType, new ListTypeWrapperDelegates(listType));
            }
        }

        public static void InvokeAddMethod(object listObject, object? value) => _GetWrapper(listObject).AddElementDelegate(listObject, value);
    }
}
