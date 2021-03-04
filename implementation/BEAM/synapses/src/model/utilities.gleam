import gleam_zlists.{ZList} as zlist

pub fn lazy_max_int(zls: ZList(Int)) -> Int {
  let Ok(tuple(hd, tl)) = zlist.uncons(zls)
  zlist.reduce(
    tl,
    hd,
    fn(x, acc) {
      case x > acc {
        True -> x
        False -> acc
      }
    },
  )
}

pub fn lazy_max_float(zls: ZList(Float)) -> Float {
  let Ok(tuple(hd, tl)) = zlist.uncons(zls)
  zlist.reduce(
    tl,
    hd,
    fn(x, acc) {
      case x >. acc {
        True -> x
        False -> acc
      }
    },
  )
}

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
