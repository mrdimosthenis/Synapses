package synapses.model.encoding

object Serialization {

  sealed trait Attribute

  final case class DiscreteAttribute(key: String,
                                     values: LazyList[String])
    extends Attribute

  case class ContinuousAttribute(key: String,
                                 min: Double,
                                 max: Double)
    extends Attribute with AttributeSerialized

  type Preprocessor = LazyList[Attribute]

  //

  sealed trait AttributeSerialized

  final case class DiscreteAttributeSerialized(key: String,
                                               values: List[String])
    extends AttributeSerialized

  type PreprocessorSerialized = List[AttributeSerialized]

}
