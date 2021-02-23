import synapses
import gleam/should

pub fn hello_world_test() {
  synapses.hello_world()
  |> should.equal("Hello, from synapses!")
}
