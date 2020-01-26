from typing import List

from dataclasses import dataclass
from functional.pipeline import Sequence


@dataclass(frozen=True)
class Attribute:
    key: str


@dataclass(frozen=True)
class DiscreteAttribute(Attribute):
    key: str
    values: Sequence


@dataclass(frozen=True)
class ContinuousAttribute(Attribute):
    key: str
    min: float
    max: float


Preprocessor = Sequence


##


@dataclass(frozen=True)
class AttributeSerialized:
    key: str


@dataclass(frozen=True)
class DiscreteAttributeSerialized(AttributeSerialized):
    key: str
    values: List[str]


@dataclass(frozen=True)
class ContinuousAttributeSerialized(AttributeSerialized):
    key: str
    min: float
    max: float
