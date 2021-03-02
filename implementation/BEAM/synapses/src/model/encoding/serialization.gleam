import gleam_zlists.{ZList}

pub type DiscreteAttribute {
  DiscreteAttribute(key: String, values: ZList(String))
}

pub type ContinuousAttribute {
  ContinuousAttribute(key: String, min: Float, max: Float)
}

pub type Attribute {
  DiscrAttr(DiscreteAttribute)
  ContinAttr(ContinuousAttribute)
}

pub type Preprocessor =
  ZList(Attribute)

pub type DiscreteAttributeSerialized {
  DiscreteAttributeSerialized(key: String, values: List(String))
}

pub type ContinuousAttributeSerialized {
  ContinuousAttributeSerialized(key: String, min: Float, max: Float)
}

pub type AttributeSerialized {
  DiscrAttrSerialized(DiscreteAttributeSerialized)
  ContinAttrSerialized(ContinuousAttributeSerialized)
}

pub type PreprocessorSerialized =
  List(AttributeSerialized)
