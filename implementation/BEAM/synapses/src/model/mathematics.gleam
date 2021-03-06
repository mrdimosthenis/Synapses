import gleam/float
import gleam/int
import gleam/pair
import gleam_zlists.{ZList} as zlist

pub fn dot_product(left: ZList(Float), right: ZList(Float)) -> Float {
  zlist.zip(left, right)
  |> zlist.map(fn(x) {
    let tuple(a, b) = x
    a *. b
  })
  |> zlist.sum
}

fn euclidean_distance(xs: ZList(Float), ys: ZList(Float)) -> Float {
  let Ok(res) =
    xs
    |> zlist.zip(ys)
    |> zlist.map(fn(t) {
      let tuple(x, y) = t
      let diff = x -. y
      diff *. diff
    })
    |> zlist.sum
    |> float.square_root
  res
}

pub fn root_mean_square_error(
  y_hats_with_ys: ZList(tuple(ZList(Float), ZList(Float))),
) -> Float {
  let tuple(n, s) =
    y_hats_with_ys
    |> zlist.map(fn(t) {
      let tuple(y_hat, y) = t
      let d = euclidean_distance(y_hat, y)
      d *. d
    })
    |> zlist.reduce(
      tuple(0, 0.0),
      fn(x, acc) {
        let tuple(acc_n, acc_s) = acc
        tuple(acc_n + 1, acc_s +. x)
      },
    )
  let avg = s /. int.to_float(n)
  let Ok(res) = float.square_root(avg)
  res
}
