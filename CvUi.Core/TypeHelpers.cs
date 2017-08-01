using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CvUi.Core
{
    public static class TypeHelpers
    {
        /// <summary>
        /// Cast the object to the destired type.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public static object Convert(object value, Type destinationType)
        {
            if (value == null)
            {
                throw new InvalidOperationException("value cannot be null");
            }
            // TODO: This is probably slow in a tight loop, but I'm too lazy to write TypeConverters for everything.
            // see https://stackoverflow.com/questions/1398796/casting-with-reflection
            var dataParam = Expression.Parameter(typeof(object), "data");
            var body = Expression.Block(Expression.Convert(Expression.Convert(dataParam, value.GetType()), destinationType));
            var run = Expression.Lambda(body, dataParam).Compile();
            var ret = run.DynamicInvoke(value);
            return ret;
        }
    }
}
