package synapses.model

object Mathematics {

  def dotProduct(xs: LazyList[Double], ys: LazyList[Double]): Double =
    xs.zip(ys)
      .map { case (x, y) => x * y }
      .sum

  def euclideanDistance(xs: LazyList[Double], ys: LazyList[Double]): Double = {
    val s = xs
      .zip(ys)
      .map { case (x, y) => (x - y) * (x - y) }
      .sum
    Math.sqrt(s)
  }

  def rootMeanSquareError(yHatsWithYs: LazyList[(LazyList[Double], LazyList[Double])])
  : Double = {
    val (n, s) = yHatsWithYs
      .map { case (y_hat, y) =>
        val d = euclideanDistance(y_hat, y)
        d * d
      }
      .foldLeft((0, 0.0)) { case ((accN, accS), sd) =>
        (accN + 1, accS + sd)
      }
    Math.sqrt(s / n)
  }

}
