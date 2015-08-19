﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox4.Models.Content
{
    public class TargetPath : ITargetPath
    {
        public string AreaName { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Section { get; set; }
        public string ContentId { get; set; }
    }
}