using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;


namespace Mmcoy.Framework
{
    public class MFDynamicProperty
    {
        private Func<object, object> m_getter;

        private Action<object, object> m_setter;


        public MFDynamicProperty(Type type, string propertyName)
            : this(type.GetProperty(propertyName))
        { }

        public MFDynamicProperty(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                return;

            var instance = Expression.Parameter(typeof(object), "instance");

            var instanceCast = Expression.Convert(instance, propertyInfo.ReflectedType);

            var propertyAccess = Expression.Property(instanceCast, propertyInfo);

            var castPropertyValue = Expression.Convert(propertyAccess, typeof(object));

            var getValueLambda = Expression.Lambda<Func<object, object>>(castPropertyValue, instance);//生成Get的Lambda表达式

            var valParam = Expression.Parameter(typeof(object), "value");

            var valParamCast = Expression.Convert(valParam, propertyInfo.PropertyType);

            var propertySeter = Expression.Call(instanceCast, propertyInfo.GetSetMethod(), valParamCast);

            var setValueLambda = Expression.Lambda<Action<object, object>>(propertySeter, instance, valParam);//生成Set的Lambda表达式

            this.m_getter = getValueLambda.Compile();
            this.m_setter = setValueLambda.Compile();
            this.Property = propertyInfo;
        }

        public PropertyInfo Property { get; set; }

        public object GetValue(object o)
        {
            return this.m_getter(o);
        }

        public void SetValue(object o, object value)
        {
            this.m_setter(o, value);
        }
    }
}