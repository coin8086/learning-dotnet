﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTestSample
{
    public class ClassFixture : IClassFixture<FixtureData>
    {
        private FixtureData _data;

        public ClassFixture(FixtureData data)
        {
            _data = data;
        }

        //One and only one between Test1 and Test2 should pass, and the other should fail. 
        //The run order decides which one should pass.
        //See more in https://xunit.net/docs/shared-context#class-fixture

        [Fact]
        public void Test1()
        {
            Assert.Equal(0, _data.Num);
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(0, _data.Num);
        }
    }

    public class FixtureData
    {
        private int _num = 0;

        public int Num
        {
            get { return _num++; }
        }
    }
}
