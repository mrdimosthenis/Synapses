from typing import Tuple, TypeVar
import json

from functional import seq
from functional.pipeline import Sequence
import dataclasses

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


def lazy_split_at(n: int,
                  ls: Sequence
                  ) -> (Sequence, Sequence):
    return ls.take(n), ls.drop(n)


def lazy_realization(ls: Sequence) -> Sequence:
    # this is odd but it gives different results from cached
    ls.__str__()
    return ls


class EnhancedJSONEncoder(json.JSONEncoder):
    def default(self, o):
        if dataclasses.is_dataclass(o):
            return dataclasses.asdict(o)
        return super().default(o)
