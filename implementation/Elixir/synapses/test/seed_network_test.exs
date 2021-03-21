defmodule SeedNetworkTest do
  use ExUnit.Case
  alias Synapses.{NeuralNetwork}

  def layers(), do: [4, 6, 5, 3]

  def neuralNetwork(), do: NeuralNetwork.initWithSeed(1000, layers())

  def inputValues(), do: [1.0, 0.5625, 0.511111, 0.47619]

  def prediction(), do: NeuralNetwork.prediction(neuralNetwork(), inputValues())

  def learningRate(), do: 0.99

  def expectedOutput(), do: [0.2, 0.8, 0.01]

  def fitNetwork(), do: NeuralNetwork.fit(neuralNetwork(), learningRate(), inputValues(), expectedOutput())

  test "neural network to json" do
    neuralNetworkJson = "[[{\"activationF\":\"sigmoid\",\"weights\":[0.97591192029471424,0.608726848593679616,-0.10449463866742392,0.86215539315831552,0.508145836217687808]},{\"activationF\":\"sigmoid\",\"weights\":[0.42884311071260608,-0.928013620271803264,-0.841588171984059648,0.6369844981782016,0.493257044371126848]},{\"activationF\":\"sigmoid\",\"weights\":[-0.250953134676056512,0.486245119456245824,0.373046333986740224,0.48124515069088416,0.437726902073087424]},{\"activationF\":\"sigmoid\",\"weights\":[0.34062889128564544,-0.075654855768376944,0.12336723424429552,-0.590345384807812224,-0.025738667762999156]},{\"activationF\":\"sigmoid\",\"weights\":[0.097662715981162496,0.102552052937866064,0.763052863780054656,0.864186513158113408,0.852234019355143168]},{\"activationF\":\"sigmoid\",\"weights\":[0.500883936382927808,-0.815786202686251648,-0.069786319095865856,0.052542488749475872,-0.42187283962216096]}],[{\"activationF\":\"sigmoid\",\"weights\":[-0.545387987433667776,0.193848784045706816,-0.894539921764575488,0.017658813141432584,-0.99557544998996224,0.78955757947190272,-0.135504802947656656]},{\"activationF\":\"sigmoid\",\"weights\":[0.7449544487116448,0.820276742068607488,-0.100644851204899856,-0.433682731359977792,-0.458571748080177216,-0.18585897382787264,0.7339677148976192]},{\"activationF\":\"sigmoid\",\"weights\":[-0.695678943892840576,0.84676260345272,0.755033002416591488,-0.147621102357376128,0.94595696395035776,0.242011207645256736,0.093684348045486752]},{\"activationF\":\"sigmoid\",\"weights\":[0.16876442707872384,-0.216468406120710048,-0.25336739896256112,0.416066890622018432,0.025323671178192784,0.517898431689628928,0.160560495194321792]},{\"activationF\":\"sigmoid\",\"weights\":[-0.295402891098786496,0.234971109993689984,0.175273896680819168,-0.616672251978913152,0.759511997533433216,0.619803202444554112,0.183035240691822976]}],[{\"activationF\":\"sigmoid\",\"weights\":[0.882362387086611968,-0.371599210719792064,-0.245399966611133504,0.03076713228196848,0.656814133282412672,-0.27558689300370864]},{\"activationF\":\"sigmoid\",\"weights\":[0.63357058507698304,-0.048198159104340776,-0.49323248769687392,-0.684966179534859392,-0.357854336273865088,0.918464864900381056]},{\"activationF\":\"sigmoid\",\"weights\":[0.593111751544443904,-0.006652794363385395,0.867715902167896192,-0.934252979757118848,0.657668916181701376,0.15566771739603104]}]]"
    assert NeuralNetwork.toJson(neuralNetwork()) == neuralNetworkJson
  end

  test "neural network of/to json" do
    neuralNetworkJson = NeuralNetwork.toJson(neuralNetwork())
    recreatedNeuralNetworkJson = neuralNetworkJson
                                 |> NeuralNetwork.ofJson
                                 |> NeuralNetwork.toJson
    assert recreatedNeuralNetworkJson == neuralNetworkJson
  end

  test "neural network prediction" do
    assert prediction() == [0.7018483008852783, 0.5232699523175631, 0.746950953587391]
  end

  test "neural network normal errors" do
    assert NeuralNetwork.errors(neuralNetwork(), learningRate(), inputValues(), expectedOutput()) == [
             0.07624623311148832,
             0.042888506125212174,
             0.0389702884518459,
             0.036307693745359616
           ]
  end

  test "neural network zero errors" do
    assert NeuralNetwork.errors(neuralNetwork(), learningRate(), inputValues(), prediction()) == [0, 0, 0, 0]
  end

  test "fit neural network prediction" do
    assert NeuralNetwork.prediction(fitNetwork(), inputValues()) == [
             0.6335205999385805,
             0.5756314596704061,
             0.6599122411687741
           ]
  end

end
