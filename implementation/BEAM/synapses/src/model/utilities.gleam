import gleam_zlists.{ZList} as zlist

pub fn infinite_indices() -> ZList(Int) {
  zlist.iterate(0, fn(x) { x + 1 })
}
