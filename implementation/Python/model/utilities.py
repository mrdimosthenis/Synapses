from typing import Tuple

from functional import seq
from functional.pipeline import Sequence


def lazy_unzip(ls: Sequence) -> Tuple[Sequence,Sequence]:
    (reversed_xs, reversed_ys) = seq([])\
        .fold_left(
        (seq([]), seq([])),
        lambda acc_t, t:(
            seq([t[0]]) + acc_t[0],
            seq([t[1]]) + acc_t[1]))
    xs = reversed_xs.reverse()
    ys = reversed_ys.reverse()
    return xs, ys
