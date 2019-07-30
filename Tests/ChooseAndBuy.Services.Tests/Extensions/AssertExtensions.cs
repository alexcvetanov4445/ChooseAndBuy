﻿namespace ChooseAndBuy.Services.Tests.Extensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using Xunit;

    public static class AssertExtensions
    {
        public static void EmptyWithMessage(IEnumerable collection, string message)
        {
            var count = 0;

            foreach (var item in collection)
            {
                count++;
            }

            Assert.True(count == 0, message);
        }

        public static void EqualCountWithMessage(int first, int second, string message)
        {
            Assert.True(first == second, message);
        }

        public static void NotNullWithMessage(object obj, string message)
        {
            Assert.True(obj != null, message);
        }
    }
}
