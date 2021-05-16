using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace HGServer.Utility
{
    static class ListExtension
    {
        class ListAccessor<T>
        {
            public static Func<List<T>, T[]> Getter;

            static ListAccessor()
            {
                var dm = new DynamicMethod("get", MethodAttributes.Static | MethodAttributes.Public, CallingConventions.Standard, typeof(T[]), new Type[] { typeof(List<T>) }, typeof(ListAccessor<T>), true);
                var il = dm.GetILGenerator();
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldfld, typeof(List<T>).GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance)); // Replace argument by field
                il.Emit(OpCodes.Ret);
                Getter = (Func<List<T>, T[]>)dm.CreateDelegate(typeof(Func<List<T>, T[]>));
            }
        }

        public static T[] GetListArray<T>(this List<T> list)
        {
            return ListAccessor<T>.Getter(list);
        }

        public static Span<T> GetListSpan<T>(this List<T> list)
        {
            var array = list.GetListArray();
            return new Span<T>(array);
        }
    }
}
