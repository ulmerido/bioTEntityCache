using bioTCache.Entitys;
using bioTCache.Firebase;
using bioTCache.Repositorys;
using bioTCache.Tests;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace bioTCache
{
    public class Program
    {
        static void Main(string[] args)
        {
            new EntityCache_Example().Start();
        }
    }
}
