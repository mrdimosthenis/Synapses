package synapses.model.encoding

import io.circe._
import io.circe.generic.auto._
import io.circe.parser
import io.circe.syntax._
import synapses.model.Utilities
import synapses.model.encoding.ContinuousAttribute
import synapses.model.encoding.DiscreteAttribute
import synapses.model.encoding.Serialization._


object Preprocessor {

  def updated(datapoint: Map[String, String])
             (preprocessor: Preprocessor)
  : Preprocessor = preprocessor.map {
    case attr: DiscreteAttribute =>
      DiscreteAttribute.updated(datapoint)(attr)
    case attr: ContinuousAttribute =>
      ContinuousAttribute.updated(datapoint)(attr)
  }

  // public
  def init(keysWithFlags: LazyList[(String, Boolean)],
           dataset: LazyList[Map[String, String]])
  : Preprocessor = {
    val initPreprocessor = keysWithFlags.map {
      case (key, isDiscrete) =>
        if (isDiscrete)
          Serialization.DiscreteAttribute(
            key,
            dataset.take(1).map(_ (key))
          ): Attribute
        else {
          val v = ContinuousAttribute.parse(dataset.head(key))
          Serialization.ContinuousAttribute(
            key,
            v,
            v
          ): Attribute
        }
    }
    Utilities.lazyPreprocessorRealization(
      dataset
        .tail
        .foldLeft(initPreprocessor) { case (acc, x) =>
          updated(x)(acc)
        }
    )
  }

  // public
  def encode(datapoint: Map[String, String])
            (preprocessor: Preprocessor)
  : LazyList[Double] = preprocessor.flatMap {
    case attr: DiscreteAttribute =>
      DiscreteAttribute.encode(datapoint(attr.key))(attr)
    case attr: ContinuousAttribute =>
      ContinuousAttribute.encode(datapoint(attr.key))(attr)
  }

  private def decodeAccF(acc: (LazyList[Double], LazyList[(String, String)]),
                         attr: Attribute)
  : (LazyList[Double], LazyList[(String, String)]) = {
    val (unprocessedFloats, processedKsVs) = acc
    val (key, splitIndex) = attr match {
      case attr: DiscreteAttribute =>
        (attr.key, attr.values.length)
      case attr: ContinuousAttribute =>
        (attr.key, 1)
    }
    val (encodedValues, nextFloats) =
      unprocessedFloats.splitAt(splitIndex)
    val decodedValue = attr match {
      case attr: DiscreteAttribute =>
        DiscreteAttribute.decode(encodedValues)(attr)
      case attr: ContinuousAttribute =>
        ContinuousAttribute.decode(encodedValues)(attr)
    }
    val nextKsVs = LazyList.cons((key, decodedValue), processedKsVs)
    (nextFloats, nextKsVs)
  }

  // public
  def decode(encodedDatapoint: LazyList[Double])
            (preprocessor: Preprocessor)
  : Map[String, String] = preprocessor
    .foldLeft((encodedDatapoint, LazyList.empty[(String, String)]))(decodeAccF)
    ._2
    .toMap

  def serialized(preprocessor: Preprocessor)
  : PreprocessorSerialized = preprocessor
    .map {
      case attr: DiscreteAttribute =>
        DiscreteAttribute.serialized(attr)
      case attr: ContinuousAttribute =>
        ContinuousAttribute.serialized(attr)
    }
    .toList

  private def toFSharp(json: Json): String = {
    val s = json.mapArray {
      _.map { js =>
        val cursor: HCursor = js.hcursor
        val discreteCase = cursor
          .get[Json]("DiscreteAttributeSerialized")
          .toOption
        val continuousCase = cursor
          .get[Json]("ContinuousAttribute")
          .toOption
        val fSharpJson = (discreteCase, continuousCase) match {
          case (Some(discrete), None) =>
            Map[String, Json](
              "Case" -> "SerializableDiscrete".asJson,
              "Fields" -> List(discrete).asJson
            )
          case (None, Some(continuous)) =>
            Map[String, Json](
              "Case" -> "SerializableContinuous".asJson,
              "Fields" -> List(continuous).asJson
            )
          case _ =>
            throw new Exception("non exhausting pattern matching")
        }
        fSharpJson.asJson
      }
    }
    s.noSpaces
  }

  private def fromFSharp(s: String): Json = {
    parser
      .parse(s)
      .toOption
      .get
      .mapArray {
        _.map { attr =>
          val cursor: HCursor = attr.hcursor
          val field = cursor
            .downField("Fields")
            .downArray
            .focus
            .get
          val attrCaseString = cursor
            .get[String]("Case")
            .toOption
            .get
          val serializedOpt = attrCaseString match {
            case "SerializableDiscrete" =>
              field.as[DiscreteAttributeSerialized]
            case "SerializableContinuous" =>
              field.as[ContinuousAttribute]
          }
          serializedOpt
            .toOption
            .get
            .asInstanceOf[AttributeSerialized]
            .asJson
        }
      }
  }

  // public
  def toJson(preprocessor: Preprocessor): String = {
    val serializedJson = serialized(preprocessor).asJson
    toFSharp(serializedJson)
  }

  private def deserialized(preprocessorSerialized: PreprocessorSerialized)
  : Preprocessor = preprocessorSerialized
    .to(LazyList)
    .map {
      case attr: DiscreteAttributeSerialized =>
        DiscreteAttribute.deserialized(attr)
      case attr: ContinuousAttribute =>
        ContinuousAttribute.deserialized(attr)
    }

  // public
  def ofJson(s: String): Preprocessor =
    Utilities.lazyPreprocessorRealization(
      fromFSharp(s)
        .as[PreprocessorSerialized]
        .toOption
        .map(deserialized)
        .get
    )

}
