package synapses.model.netElems

case class Activation(name: String,
                      f: Double => Double,
                      deriv: Double => Double,
                      inverse: Double => Double,
                      minMaxInVals: (Double, Double))

object Activation {

  def restrictedInput(activation: Activation, x: Double): Double =
    Math.max(
      Math.min(
        x,
        activation.minMaxInVals._2
      ),
      activation.minMaxInVals._1
    )

  def restrictedOutput(activation: Activation, y: Double): Double =
    Math.max(
      Math.min(
        y,
        activation.f(activation.minMaxInVals._2)
      ),
      activation.f(activation.minMaxInVals._1)
    )

  private def sigmoidF(x: Double): Double = {
    1.0 / (1.0 + Math.exp(-x))
  }

  val sigmoid: Activation =
    Activation(
      "sigmoid",
      sigmoidF,
      d => sigmoidF(d) * (1.0 - sigmoidF(d)),
      y => Math.log(y / (1.0 - y)),
      (-700.0, 20.0)
    )

  val identity: Activation =
    Activation(
      "identity",
      x => x,
      _ => 1.0,
      y => y,
      (-1.7976931348623157E308, 1.7976931348623157E308)
    )

  val tanh: Activation =
    Activation(
      "tanh",
      Math.tanh,
      d => 1.0 - Math.tanh(d) * Math.tanh(d),
      y => 0.5 * Math.log((1.0 + y) / (1.0 - y)),
      (-10.0, 10.0)
    )

  val leakyReLU: Activation =
    Activation(
      "leakyReLU",
      x => if (x < 0.0) {
        0.01 * x
      } else {
        x
      },
      d => if (d < 0.0) {
        0.01
      } else {
        1.0
      },
      y => if (y < 0.0) {
        y / 0.01
      } else {
        y
      },
      (-1.7976931348623157E308, 1.7976931348623157E308)
    )

  type ActivationSerialized = String

  def serialized(activation: Activation)
  : ActivationSerialized = activation.name

  def deserialized(activationSerialized: ActivationSerialized)
  : Activation =
    activationSerialized match {
      case "sigmoid" => Activation.sigmoid
      case "identity" => Activation.identity
      case "tanh" => Activation.tanh
      case "leakyReLU" => Activation.leakyReLU
    }

}
