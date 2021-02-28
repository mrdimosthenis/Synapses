import gleam_zlists.{ZList} as zlist

pub fn lazy_unzip(zls: ZList(tuple(a, b))) -> tuple(ZList(a), ZList(b)) {
  zls
  |> zlist.reverse
  |> zlist.reduce(
    tuple(zlist.new(), zlist.new()),
    fn(it, acc) {
      let tuple(x, y) = it
      let tuple(acc_xs, acc_ys) = acc
      tuple(zlist.cons(acc_xs, x), zlist.cons(acc_ys, y))
    },
  )
}
