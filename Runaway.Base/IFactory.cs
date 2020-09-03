using System;
using System.Collections.Generic;
using System.Text;

namespace Runaway.Base
{
    /// <summary>
    /// object factory interface
    /// </summary>
    /// <typeparam name="T">object type</typeparam>
    interface IFactory<T> 
        where T : new()
    {
        /// <summary>
        /// Initialization factory
        /// </summary>
        void Initialize();

        /// <summary>
        /// Create new object
        /// </summary>
        /// <returns>Created object</returns>
        T CreateObject();

        /// <summary>
        /// Return object
        /// </summary>
        /// <param name="returnObject">Return object</param>
        void ReturnObject(T returnObject);

        /// <summary>
        /// Close factory
        /// </summary>
        void Close();
    }
}
