using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuilderExample
{
    /// <summary>
    /// Example class that is used as the concrete type for the RecordBuilder class
    /// </summary>
    public class IceCream
    {
        public string Name { get; set; }
        public int Scoops { get; set; }
    }

    /// <summary>
    /// Example of implementing the RecordBuilder class
    /// </summary>
    public class IceCreamMachine
    {
        public void Run()
        {
            IceCream vanilla = RecordBuilder<IceCream>.Create().WithProperty(new { Name = "Name", Value = "Vanilla" }).WithProperty(new { Name = "Scoops", Value = 2 }).Build();
            IceCream banannaChip = RecordBuilder<IceCream>.Create().WithProperty(new { Name = "Name", Value = "BanannaChip" }).WithProperty(new { Name = "Scoops", Value = 1 }).Build();

        }
    }
    /// <summary>
    /// Generic Builder Class allows chaining of the same object to build up a generic list of actions to apply to a particular object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RecordBuilder<T>
    {
        private readonly List<Action<T>> _builderActions;

        public RecordBuilder()
        {
            _builderActions = new List<Action<T>>();
        }

        /// <summary>
        /// Allow initalization of the new builder object
        /// </summary>
        /// <returns></returns>
        public static RecordBuilder<T> Create()
        {
            return new RecordBuilder<T>();
        }

        /// <summary>
        /// Allowing to pass through a dynamic object that contains a Name/Value, could possibly use a more defined class like Key/Value Pairs
        /// </summary>
        /// <param name="propertyToChange"></param>
        /// <returns></returns>
        public RecordBuilder<T> WithProperty(dynamic propertyToChange)
        {
            //Build up the new Action setting the value of the property on a generic object
            _builderActions.Add(x =>
            {
                var property = x.GetType().GetProperty(propertyToChange.Name);
                if (property != null)
                {
                    property.SetValue(x, propertyToChange.Value);
                }
            });

            return this;
        }

        /// <summary>
        /// Finally create a new instance of the type and apply each action against the newly created object
        /// </summary>
        /// <returns></returns>
        public T Build()
        {
            var obj = (T)Activator.CreateInstance(typeof(T));
            _builderActions.ForEach(x => x(obj));
            return obj;
        }
    }
}
