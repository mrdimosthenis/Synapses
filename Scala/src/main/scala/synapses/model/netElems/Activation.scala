package synapses.model.netElems

case class Activation(name: String,
                      f: Double => Double,
                      deriv: Double => Double,
                      inverse: Double => Double)

object Activation {

  private def safeSigmoid(x: Double): Double = {
    val boundedPow = Math.min(-x, 500.0)
    1.0 / (1.0 + Math.exp(boundedPow))
  }

  val sigmoid: Activation =
    Activation(
      "sigmoid",
      safeSigmoid,
      x => safeSigmoid(x) * (1.0 - safeSigmoid(x)),
      x => Math.log(x / (1.0 - x))
    )

  val identity: Activation =
    Activation(
      "identity",
      x => x,
      _ => 1.0,
      x => x
    )

  val tanh: Activation =
    Activation(
      "tanh",
      Math.tanh,
      x => 1.0 - Math.tanh(x) * Math.tanh(x),
      x => 0.5 * Math.log((1.0 + x) / (1.0 - x))
    )

  val leakyReLU: Activation =
    Activation(
      "leakyReLU",
      x => if (x < 0.0) {
        0.01 * x
      } else {
        x
      },
      x => if (x < 0.0) {
        0.01
      } else {
        1.0
      },
      x => if (x < 0.0) {
        x / 0.01
      } else {
        x
      }
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
