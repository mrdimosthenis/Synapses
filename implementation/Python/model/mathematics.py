from functional.pipeline import Sequence


def dot_product(xs: Sequence, ys: Sequence) -> float:
    return xs \
        .zip(ys) \
        .map(lambda t: t[0] * t[1]) \
        .sum()
