using System.Collections.Generic;
using SynapsesCSharp;
using Xunit;

namespace CSharpTest.test
{
    public class StatisticsTest
    {
        static IEnumerable<(double[], double[])> expectedWithOutputValues =
            new List<(double[], double[])>()
            {
                (new double[] {0.0, 0.0, 1.0}, new double[] {0.0, 0.0, 1.0}),
                (new double[] {0.0, 0.0, 1.0}, new double[] {0.0, 1.0, 1.0})
            };

        [Fact]
        public void rootMeanSquareError()
        {
            Assert.Equal(
                0.7071067811865476,
                Statistics.rootMeanSquareError(expectedWithOutputValues)
            );
        }
    }
}