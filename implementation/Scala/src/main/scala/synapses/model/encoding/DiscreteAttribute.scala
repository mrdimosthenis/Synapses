package synapses.model.encoding

import synapses.model.encoding.Serialization._

object DiscreteAttribute {

  def updated(datapoint: Map[String, String])
             (discreteAttribute: DiscreteAttribute)
  : DiscreteAttribute = {
    val exists = discreteAttribute
      .values
      .contains(datapoint(discreteAttribute.key))
    val updatedValues =
      if (exists) discreteAttribute.values
      else LazyList.cons(
        datapoint(discreteAttribute.key), // TODO: fetch `datapoint(discreteAttribute.key)` only once
        discreteAttribute.values
      )
    discreteAttribute.copy(values = updatedValues)
  }

  def encode(value: String)
            (discreteAttribute: DiscreteAttribute)
  : LazyList[Double] =
    discreteAttribute
      .values
      .map(s => if (s == value) 1.0 else 0.0)

  private def decodeAccF(acc: (String, Double),
                 x: (String, Double))
  : (String, Double) =
    if (x._2 > acc._2) x
    else acc

  def decode(encodedValues: LazyList[Double])
            (discreteAttribute: DiscreteAttribute)
  : String = discreteAttribute
    .values
    .zip(encodedValues)
    .reduceLeft(decodeAccF)
    ._1

  def serialized(discreteAttribute: DiscreteAttribute)
  : DiscreteAttributeSerialized = DiscreteAttributeSerialized(
    discreteAttribute.key,
    discreteAttribute.values.toList
  )

  def deserialized(discreteAttributeSerialized: DiscreteAttributeSerialized)
  : DiscreteAttribute = Serialization.DiscreteAttribute(
    discreteAttributeSerialized.key,
    discreteAttributeSerialized.values.to(LazyList)
  )

}
