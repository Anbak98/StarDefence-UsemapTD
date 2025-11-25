using STARTD.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace STARTD.Core
{
    public class BaseScene<T> : SingletonBehaviour<BaseScene<T>> where T : BaseScene<T>
    {
        public static new T Singleton => (T)SingletonBehaviour<BaseScene<T>>.Singleton;
    }
}
