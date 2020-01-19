using System;
using System.Collections.Generic;
using System.Linq;
using FSharpx.Collections;
using Microsoft.FSharp.Collections;

namespace SynapsesCSharp
{
    public class Statistics
    {
        
        public static double rootMeanSquareError(IEnumerable<(double [], double [])> expectedValuesWithOutputValues)
        {
            LazyList<Tuple<FSharpList<double>, FSharpList<double>>> expWithOuts = LazyList
                .ofSeq(expectedValuesWithOutputValues.Select(exOut =>
                    new Tuple<FSharpList<double>, FSharpList<double>>(
                        ListModule.OfSeq(exOut.Item1),
                        ListModule.OfSeq(exOut.Item2)
                        )
                    )
                );
            return Synapses.Statistics.rootMeanSquareError(expWithOuts);
        }
        
    }
}