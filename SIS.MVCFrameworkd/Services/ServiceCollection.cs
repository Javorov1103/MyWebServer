namespace SIS.MVCFrameworkd.Services
{
    using SIS.MVCFrameworkd.Services.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ServiceCollection : IServiceCollection
    {
        private IDictionary<Type, Type> dependencyContainer;

        public ServiceCollection()
        {
            this.dependencyContainer = new Dictionary<Type, Type>();
        }

        public void AddService<TSource, TDestination>()
        {
            dependencyContainer[typeof(TSource)] = typeof(TDestination);
        }

        public T CreateInstance<T>()
        {
            return (T)CreateInstance(typeof(T));
        }

        public object CreateInstance(Type type)
        {

            if (dependencyContainer.ContainsKey(type))
            {
                type = dependencyContainer[type];
            }

            if (type.IsInterface || type.IsAbstract)
            {
                throw new Exception($"Type {type.FullName} cannot be instanciated");
            }

            var constructor = type.GetConstructors().OrderBy(x=>x.GetParameters().Length).First();

            var constructorParams = constructor.GetParameters();

            List<object> constructorParamObjects = new List<object>();
            foreach (var param in constructorParams)
            {
                var paramObject = this.CreateInstance(param.ParameterType);
                constructorParamObjects.Add(paramObject);
            }

            var  obj = constructor.Invoke(constructorParamObjects.ToArray());

            return obj;
        }
    }
}
