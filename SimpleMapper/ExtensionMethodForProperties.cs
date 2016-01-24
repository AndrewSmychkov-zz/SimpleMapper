using System.Reflection;


namespace SimpleMapper
{
    public static class Mapper
    { /// <summary>
      /// fill object
      /// </summary>
      /// <typeparam name="TResult"></typeparam>
      /// <param name="destination"></param>
      /// <param name="source"></param>
      /// <param name="exception">Name of property which do not copy</param>
        public static void FillObject<TResult>(this TResult destination, object source, string[] exception) where TResult : class
        {
            CopyObject(destination, source, exception);
        }

        private static void CopyObject<TResult>(TResult model, object copyesModel, string[] exception)
            where TResult : class
        {
            if (copyesModel == null || model == null)
                return;
            foreach (var p in copyesModel.GetType().GetRuntimeProperties())
            {
                if (!p.CanRead || ContainsString(exception, p.Name))
                    continue;
                var p2 = model.GetType().GetRuntimeProperty(p.Name);
                if (p2 != null && p2.CanWrite && Compare(p, p2))
                {
                    p2.SetValue(model, p.GetValue(copyesModel));
                }
            }
        }

        private static bool ContainsString(string[] arraystring, string str)
        {
            for (int i = 0; i < arraystring.Length; i++)
            {
                if (arraystring[i] == str)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// fill object
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="destination"></param>
        /// <param name="source"></param>
        public static void FillObject<TResult>(this TResult destination, object source) where TResult : class
        {
            CopyObject(destination, source, new[] { "" });
        }

        private static bool Compare(PropertyInfo p, PropertyInfo p2)
        {
            return p.PropertyType == p2.PropertyType;
        }

        /// <summary>
        /// Create and fill new object
        /// </summary>
        /// <typeparam name="T">type of object</typeparam>
        /// <param name="source">Source array</param>
        /// <returns></returns>
        public static T CreateObject<T>(this object source) where T : class, new()
        {
            return source.CreateObject<T>(new[] { "" });
        }
        /// <summary>
        /// Create a new array and fill each object
        /// </summary>
        /// <typeparam name="T">type of object</typeparam>
        /// <param name="source">Source array</param>
        /// <param name="exception">Name of property which do not copy</param>
        /// <returns></returns>
        public static T CreateObject<T>(this object model, string[] exception) where T : class, new()
        {
            if (model == null)
                return null;

            var creatingOfModel = new T();

            creatingOfModel.FillObject(model, exception);

            return creatingOfModel;
        }
        /// <summary>
        /// Create and fill new object
        /// </summary>
        /// <typeparam name="T">type of object</typeparam>
        /// <param name="source">Source array</param>
        /// <returns>new array</returns>
        public static T[] CreateObjectList<T>(this object[] source) where T : class, new()
        {
            return source.CreateObjectList<T>(new[] { "" });
        }
        /// <summary>
        /// Create a new array and fill each object
        /// </summary>
        /// <typeparam name="T">type of object</typeparam>
        /// <param name="source">Source array</param>
        /// <param name="exception">Name of property which do not copy</param>
        /// <returns></returns>
        public static T[] CreateObjectList<T>(this object[] source, string[] exception) where T : class, new()
        {
            var len = source.Length;
            var list = new T[len];
            for (int i = 0; i < len; i++)
            {
                list[i] = source[i].CreateObject<T>(exception);
            }
            return list;
        }
    }
}
