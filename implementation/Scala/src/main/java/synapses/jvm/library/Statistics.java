package synapses.jvm.library;

import synapses.JavaInterface.Statistics$;

import java.util.stream.Stream;

public class Statistics {

    public static double rootMeanSquareError(Stream<double[][]> expectedValuesWithOutputValues){
        return Statistics$.MODULE$.rootMeanSquareError(expectedValuesWithOutputValues);
    }

}
