using System;
using System.Collections.Generic;
using System.Linq;
using FSharpx.Collections;
using Microsoft.FSharp.Collections;
using Synapses;
using Synapses.Model.Encoding;

namespace SynapsesCSharp
{
    public class DataPreprocessor
    {
        readonly LazyList<Preprocessor.Attribute> contents;

        public DataPreprocessor(LazyList<Preprocessor.Attribute> _contents)
        {
            contents = _contents;
        }

        public static DataPreprocessor init(
            (string, bool)[] keysWithDiscreteFlags,
            IEnumerable<Dictionary<string, string>> datapoints)
        {
            FSharpList<Tuple<string, bool>> ksFls = ListModule
                .OfSeq(keysWithDiscreteFlags.Select(x =>
                    new Tuple<string, bool>(x.Item1, x.Item2))
                );
            LazyList<FSharpMap<string, string>> data = LazyList
                .ofSeq(datapoints
                    .Select(x =>
                        x.Aggregate(
                            new FSharpMap<string, string>(Enumerable.Empty<Tuple<string, string>>()),
                            (acc, kv) => acc.Add(kv.Key, kv.Value)
                        )
                    )
                );
            LazyList<Preprocessor.Attribute> _contents = DataPreprocessorModule.init(ksFls, data);
            return new DataPreprocessor(_contents);
        }

        public static double[] encodedDatapoint(
            DataPreprocessor dataPreprocessor,
            Dictionary<String, String> datapoint)
        {
            FSharpMap<string, string> point = datapoint.Aggregate(
                new FSharpMap<string, string>(Enumerable.Empty<Tuple<string, string>>()),
                (acc, kv) => acc.Add(kv.Key, kv.Value)
            );
            return DataPreprocessorModule
                .encodedDatapoint(dataPreprocessor.contents, point)
                .ToArray();
        }

        public static Dictionary<String, String> decodedDatapoint(
            DataPreprocessor dataPreprocessor,
            double[] encodedValues)
        {
            FSharpList<double> evs = ListModule.OfSeq(encodedValues);
            return DataPreprocessorModule
                .decodedDatapoint(dataPreprocessor.contents, evs)
                .ToDictionary(x => x.Key, x => x.Value);
        }

        public static String toJson(DataPreprocessor dataPreprocessor)
        {
            return DataPreprocessorModule.toJson(dataPreprocessor.contents);
        }

        public static DataPreprocessor ofJson(String json)
        {
            LazyList<Preprocessor.Attribute> _contents = DataPreprocessorModule.ofJson(json);
            return new DataPreprocessor(_contents);
        }
    }
}