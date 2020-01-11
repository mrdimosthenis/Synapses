import org.junit.Test;

import synapses.Library.*;

import java.util.Arrays;
import java.util.stream.Stream;

import static org.junit.Assert.assertEquals;

public class StatisticsTest {

    double[][][] expectedWithOutputValuesArr = {
            {{0.0, 0.0, 1.0}, {0.0, 0.0, 1.0}},
            {{0.0, 0.0, 1.0}, {0.0, 1.0, 1.0}}
    };

    Stream<double[][]> expectedWithOutputValuesStream = Arrays.stream(expectedWithOutputValuesArr);

    @Test
    public void neuralNetworkPrediction() {
        assertEquals(
                0.7071067811865476,
                Statistics$.MODULE$.rootMeanSquareError(expectedWithOutputValuesStream),
                0.0);
    }

}
