using System;
using System.Collections.Generic;
using System.Text;

namespace rpswitch.Remote
{
    public enum MethodCode
    {
        /// <summary>
        /// 关闭远程设备
        /// </summary>
        ShotdownDevice = 0,
        /// <summary>
        /// 远程注销，主程序退出
        /// </summary>
        LogoutDevice = 1,
        /// <summary>
        /// 操作远程开关
        /// </summary>
        GpioSwitch = 2
    }
}