import gleam/pair
import gleam_zlists.{ZList} as zlist

pub fn dot_product(left: ZList(Float), right: ZList(Float)) -> Float {
  zlist.zip(left, right)
  |> zlist.map(fn(x) {
    let tuple(a, b) = x
    a *. b
  })
  |> zlist.reduce(0.0, fn(x, acc) { x +. acc })
}
