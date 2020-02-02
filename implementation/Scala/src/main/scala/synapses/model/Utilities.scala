package synapses.model

import synapses.model.encoding.Preprocessor
import synapses.model.encoding.Serialization.Preprocessor
import synapses.model.netElems.Network
import synapses.model.netElems.Network.Network

object Utilities {

  def lazyNetworkRealization(network: Network): Network = {
    Network.serialized(network)
    network
  }

  def lazyPreprocessorRealization(preprocessor: Preprocessor): Preprocessor = {
    Preprocessor.serialized(preprocessor)
    preprocessor
  }

}
