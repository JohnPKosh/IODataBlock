﻿using System;
using System.Collections.Generic;

namespace BasicTests.Data
{
    public class DnInsert
    {
        public string DomainName { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public static class DnInsertBuilder
    {
        public static List<DnInsert> GetSample1()
        {
            var rv = new List<DnInsert>();
            for (int i = 0; i < 100; i++)
            {
                rv.Add(new DnInsert()
                {
                    DomainName = "cloudroute.com",
                    Status = 1,
                    CreatedDate = DateTime.Now
                });
            }
            return rv;
        }
    }
}