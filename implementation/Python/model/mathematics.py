from functional.pipeline import Sequence
from fn import _


def dot_product(xs: Sequence, ys: Sequence) -> float:
    return xs \
        .zip(ys) \
        .map(_ * _) \
        .sum()
