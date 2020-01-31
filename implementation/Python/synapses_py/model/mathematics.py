import math

from functional.pipeline import Sequence


def dot_product(xs: Sequence, ys: Sequence) -> float:
    return xs \
        .zip(ys) \
        .map(lambda t: t[0] * t[1]) \
        .sum()


def euclidean_distance(xs: Sequence, ys: Sequence) -> float:
    s = xs \
        .zip(ys) \
        .map(lambda t: (t[0] - t[1]) * (t[0] - t[1])) \
        .sum()
    return math.sqrt(s)


def root_mean_square_error_d(y_hat: Sequence, y: Sequence) -> float:
    d = euclidean_distance(y_hat, y)
    return d * d


def root_mean_square_error(y_hats_with_ys: Sequence) -> float:
    (n, s) = y_hats_with_ys \
        .map(lambda t: root_mean_square_error_d(t[0], t[1])) \
        .fold_left((0, 0.0), lambda acc, x: (acc[0] + 1, acc[1] + x))
    return math.sqrt(s / n)
