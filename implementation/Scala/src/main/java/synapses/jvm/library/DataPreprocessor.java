package synapses.jvm.library;

import scala.collection.immutable.LazyList;
import synapses.JavaInterface.DataPreprocessor$;

import java.util.Map;
import java.util.stream.Stream;

public class DataPreprocessor {

    LazyList contents;

    public DataPreprocessor(LazyList _contents) {
        contents = _contents;
    }

    public static DataPreprocessor init(
            Object[][] keysWithDiscreteFlags,
            Stream<Map<String,String>> datapoints) {
        LazyList _contents = DataPreprocessor$.MODULE$.init(keysWithDiscreteFlags, datapoints);
        return new DataPreprocessor(_contents);
    }

    public static double[] encodedDatapoint(
            DataPreprocessor dataPreprocessor,
            Map<String,String> datapoint) {
        return DataPreprocessor$.MODULE$.encodedDatapoint(dataPreprocessor.contents, datapoint);
    }

    public static Map<String,String> decodedDatapoint(
            DataPreprocessor dataPreprocessor,
            double[] encodedValues) {
        return DataPreprocessor$.MODULE$.decodedDatapoint(dataPreprocessor.contents, encodedValues);
    }

    public static String toJson(DataPreprocessor dataPreprocessor){
        return DataPreprocessor$.MODULE$.toJson(dataPreprocessor.contents);
    }

    public static DataPreprocessor ofJson(String json){
        LazyList _contents = DataPreprocessor$.MODULE$.ofJson(json);
        return new DataPreprocessor(_contents);
    }

}
