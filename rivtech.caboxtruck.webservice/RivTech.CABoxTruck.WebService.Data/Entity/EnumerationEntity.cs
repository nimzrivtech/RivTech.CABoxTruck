using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

/// <summary>
/// https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/microservice-ddd-cqrs-patterns/enumeration-classes-over-enum-types
/// </summary>

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public abstract class Enumeration<TKey> : BaseEntity<TKey>, IDescription, IIsActive
    {
        [Key]
        public virtual TKey Id { get; set; }
        public virtual string Description { get; set; }
        public virtual bool IsActive { get; set; }

        protected Enumeration()
        {
            this.IsActive = true;
        }

        protected Enumeration(TKey id, string description)
        {
            Id = id;
            Description = description;
            IsActive = true;
        }

        public override string ToString()
        {
            return Description;
        }

        private static readonly Dictionary<string, IEnumerable<object>> ListDictionary = new Dictionary<string, IEnumerable<object>>();

        public override bool Equals(object obj)
        {
            var otherValue = obj as Enumeration<TKey>;
            if (otherValue == null)
            {
                return false;
            }
            var typeMatch = GetType() == obj.GetType();
            var valueMatch = Id.Equals(otherValue.Id);
            return typeMatch && valueMatch;
        }

        public static T GetEnumerationById<T>(object id) where T : Enumeration<TKey>, new()
        {
            return id == null ? null : GetAll<T>().SingleOrDefault(x => x.Id.Equals(id));
        }

        public static List<T> GetAll<T>() where T : Enumeration<TKey>, new()
        {
            var type = typeof(T);
            var typeName = type.Name;

            if (ListDictionary.ContainsKey(typeName))
            {
                return (ListDictionary.SingleOrDefault(x => x.Key.Equals(typeName)).Value as List<T>);
            }

            var fields = type.GetTypeInfo().GetMembers(BindingFlags.Public |
                                                       BindingFlags.Static |
                                                       BindingFlags.DeclaredOnly);
            var list = new List<T>();
            foreach (var info in fields)
            {
                var instance = new T();
                var locatedValue = GetValue(info, instance) as T;
                if (locatedValue != null)
                {
                    list.Add(locatedValue);
                }
            }

            ListDictionary.Add(typeName, list);

            return list;
        }

        private static object GetValue(MemberInfo memberInfo, object forObject)
        {
            return memberInfo.MemberType switch
            {
                MemberTypes.Field => ((FieldInfo) memberInfo).GetValue(forObject),
                MemberTypes.Property => ((PropertyInfo) memberInfo).GetValue(forObject),
                _ => null
            };
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
