package synapses.model.encoding

import synapses.model.encoding.Serialization._

object ContinuousAttribute {

  def parse(s: String): Double =
    s.trim.toDouble

  def updated(datapoint: Map[String, String])
             (continuousAttribute: ContinuousAttribute)
  : ContinuousAttribute = {
    val v = parse(datapoint(continuousAttribute.key))
    continuousAttribute.copy(
      min = Math.min(v, continuousAttribute.min),
      max = Math.max(v, continuousAttribute.max)
    )
  }

  def encode(value: String)
            (continuousAttribute: ContinuousAttribute)
  : LazyList[Double] =
    if (continuousAttribute.min == continuousAttribute.max)
      LazyList(0.5)
    else LazyList(
      (parse(value) - continuousAttribute.min) /
        (continuousAttribute.max - continuousAttribute.min)
    )

  def decode(encodedValues: LazyList[Double])
            (continuousAttribute: ContinuousAttribute)
  : String =
    if (continuousAttribute.min == continuousAttribute.max)
      "%f".format(continuousAttribute.min)
    else {
      val v = encodedValues.head *
        (continuousAttribute.max - continuousAttribute.min) +
        continuousAttribute.min
      "%f".format(v)
    }

  def serialized(continuousAttribute: ContinuousAttribute)
  : ContinuousAttribute = continuousAttribute

  def deserialized(continuousAttributeSerialized: ContinuousAttribute)
  : ContinuousAttribute = continuousAttributeSerialized

}
