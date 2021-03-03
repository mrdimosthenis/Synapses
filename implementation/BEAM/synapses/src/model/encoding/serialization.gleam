import gleam_zlists.{ZList}

pub type Attribute {
  DiscreteAttribute(key: String, values: ZList(String))
  ContinuousAttribute(key: String, min: Float, max: Float)
}

pub type AttributeSerialized {
  DiscreteAttributeSerialized(key: String, values: List(String))
  ContinuousAttributeSerialized(key: String, min: Float, max: Float)
}

pub type FSharpAttributeSerialized {
  FSharpAttributeSerialized(case_: String, fields: List(AttributeSerialized))
}

pub type Preprocessor =
  ZList(Attribute)

pub type PreprocessorSerialized =
  List(FSharpAttributeSerialized)
