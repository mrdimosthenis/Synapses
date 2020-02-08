import minitest._
import synapses.Library._

object StatisticsTest extends SimpleTestSuite {

  val expectedWithOutputValues =
    LazyList(
      (List(0.0, 0.0, 1.0), List(0.0, 0.0, 1.0)),
      (List(0.0, 0.0, 1.0), List(0.0, 1.0, 1.0))
    )

  test("root mean square error") {
    assertEquals(
      Statistics.rootMeanSquareError(expectedWithOutputValues),
      0.7071067811865476
    )
  }

}
