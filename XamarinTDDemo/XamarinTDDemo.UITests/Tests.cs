﻿using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace XamarinTDDemo.UITests
{
    [TestFixture(Platform.Android)]
    public class Tests
    {
        IApp app;
        Platform platform;

        public Tests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
        }

        [Test]
        public void AppLaunches()
        {
            app.Screenshot("First screen.");
        }
        [Test]
        public void CalculatorFormLoads()
        {
            app.WaitForElement(c => c.Class("PageRenderer"));
            Assert.Pass();
        }
        [Test]
        public void CalculatorHasStartTimeControl()
        {
            app.WaitForElement(c => c.Class("PageRenderer"));
            var startTimeControls = app.Query(c=>c.Marked("StartTimeControl"));
            Assert.IsTrue(startTimeControls!=null && startTimeControls.Count()>0);
        }
    }
}

