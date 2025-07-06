using BinarySerializerLibrary.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Base
{
    public static class MethodAccessDelegateCompiler
    {
        public class DelegateWrapper<Type1, Type2>
        {
            public Action<object, object> GetActionWrapper(Delegate actionDelegate)
            {
                return (value1, value2) => { ((Action<Type1, Type2>)actionDelegate)?.Invoke((Type1)value1, (Type2)value2); };
            }

            public Func<object, object?> GetFunctionWrapper(Delegate functionDelegate)
            {
                return (value1) => { return ((Func<Type1, Type2>)functionDelegate).Invoke((Type1)value1); };
            }
        }

        public static Action<object, object?> CreatePropertySetterDelegate(Type ownerType, MethodInfo? methodInfo)
        {
            var wrappedDelegate = _GetWrappedDelegate(ownerType, methodInfo, "GetActionWrapper");

            if (wrappedDelegate is not null)
            {
                var convertedDelegate = wrappedDelegate as Action<object, object?>;

                if (convertedDelegate is not null)
                {
                    return convertedDelegate;
                }
            }

            throw new ReflectionMethodAccessException("Ошибка создания делегата метода установки значения", ownerType, methodInfo);
        }

        public static Func<object, object?> CreatePropertyGetterDelegate(Type ownerType, MethodInfo? methodInfo)
        {
            var wrappedDelegate = _GetWrappedDelegate(ownerType, methodInfo, "GetFunctionWrapper");

            if (wrappedDelegate is not null)
            {
                var convertedDelegate = wrappedDelegate as Func<object, object?>;

                if (convertedDelegate is not null)
                {
                    return convertedDelegate;
                }
            }

            throw new ReflectionMethodAccessException("Ошибка создания делегата метода чтения значения", ownerType, methodInfo);
        }

        private static Delegate? _GetWrappedDelegate(Type ownerType, MethodInfo? methodInfo, string wrappingMethodName)
        {
            if (methodInfo is null)
            {
                throw new ReflectionMethodAccessException("Отсутствует информация о методе для создания делегата", ownerType, methodInfo);
            }

            var methoDelegate = _GetMethodDelegate(ownerType, methodInfo);

            if (methoDelegate is not null)
            {
                var concreteDelegateWrapperType = Type.GetType("BinarySerializerLibrary.Base.MethodAccessDelegateCompiler+DelegateWrapper`2")?
                    .MakeGenericType(methoDelegate.GetType().GetGenericArguments());

                if (concreteDelegateWrapperType is not null)
                {
                    var concreteDelegateWrapperTypeInstance = Activator.CreateInstance(concreteDelegateWrapperType);

                    return concreteDelegateWrapperType.GetMethod(wrappingMethodName)?.Invoke(concreteDelegateWrapperTypeInstance, new object[] { methoDelegate }) as Delegate;
                }
                else
                {
                    throw new ReflectionMethodAccessException("Невозможно создать тип-обертку для вызова метода в виде делегата", ownerType, methodInfo);
                }
            }
            else
            {
                throw new ReflectionMethodAccessException("Невозможно создать делегат из метода", ownerType, methodInfo);
            }
        }

        private static Delegate? _GetMethodDelegate(Type ownerType, MethodInfo methodInfo)
        {
            try
            {
                var instance = Expression.Parameter(ownerType);

                var methodArguments = methodInfo.GetParameters().Select((p) => Expression.Parameter(p.ParameterType)).ToList();

                var call = Expression.Call(instance, methodInfo, methodArguments);

                methodArguments.Insert(0, instance);

                return Expression.Lambda(call, methodArguments).Compile();
            }
            catch
            {
                return null;
            }
        }
    }
}
