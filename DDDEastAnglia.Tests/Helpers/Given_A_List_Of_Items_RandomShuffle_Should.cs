using DDDEastAnglia.Helpers;
using NUnit.Framework;
using System.Collections.Generic;

namespace DDDEastAnglia.Tests.Helpers
{
    [TestFixture]
    public class Given_A_List_Of_Items_RandomShuffle_Should
    {
        [TestCase(new[] { 1, 2, 3 })]
        [TestCase(new[] { 1, 2, 3, 4, 5, 6 })]
        public void Leave_The_Same_Number_Of_Items_In_The_List(IList<int> inputData)
        {
            int startingCount = inputData.Count;
            inputData.RandomShuffle();

            Assert.That(startingCount, Is.EqualTo(inputData.Count));
        }

        [TestCase(new[] { 1, 2, 3 })]
        [TestCase(new[] { 1, 2, 3, 4, 5, 6 })]
        public void Contain_All_The_Same_Items(IList<int> inputData)
        {
            int[] initialData = new int[inputData.Count];
            inputData.CopyTo(initialData, 0);

            inputData.RandomShuffle();

            foreach (var initialItem in initialData)
            {
                Assert.That(inputData, Contains.Item(initialItem));
            }
        }
    }
}