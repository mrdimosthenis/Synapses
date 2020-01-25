from typing import Tuple, TypeVar

from functional import seq
from functional.pipeline import Sequence

T = TypeVar('T')


def lazy_cons(x: T, xs: Sequence) -> Sequence:
    return seq([x]) + xs


def lazy_unzip(ls: Sequence) -> Tuple[Sequence, Sequence]:
    (reversed_xs, reversed_ys) = ls \
        .fold_left(
        (seq([]), seq([])),
        lambda acc_t, t: (
            seq([t[0]]) + acc_t[0],
            seq([t[1]]) + acc_t[1]))
    xs = reversed_xs.reverse()
    ys = reversed_ys.reverse()
    return xs, ys
