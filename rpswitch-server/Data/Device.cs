using System;
using System.Collections.Generic;
namespace rpswitch.Data
{
    public class Device
    {
        public string tag { get; set; }
        public DateTime time { get; set; }
        public Guid id { get; set; }
        public sysinfo sys { get; set; }

    }
    public class sysinfo
    {
        public Dictionary<string, string> ip { get; set; }
        public float memory { get; set; }
        public float cpu { get; set; }
        public decimal uptime { get; set; }
    }
}