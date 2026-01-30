using System;

namespace MirralLoggerSystem.Runtime.Model
{
    //自定义分类
    [Flags]
    public enum LogCategory
    {
        None = 0,
        SaveAndLoad = 1 << 0,
        Network = 1 << 1,
        Animation = 1 << 2,
        All = SaveAndLoad | Network | Animation
    }
}