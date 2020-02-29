package synapses.model

import scalatags.Text.all._
import scalatags.Text.svgTags
import scalatags.Text.svgTags._
import scalatags.Text.svgAttrs._
import synapses.model.netElems.Layer.Layer
import synapses.model.netElems.Network.Network

object Draw {

  private val pixels = 400.0

  private val circleVerticalDistance = pixels * 0.02
  private val circleHorizontalDistance = pixels * 0.15

  private val circleRadius = pixels * 0.03
  private val circleStrokeWidth = pixels / 150

  private val lineStrokeWidth = pixels / 300

  private val circleFill = "white"

  private val inputCircleStroke = "brown"
  private val biasCircleStroke = "black"

  private val sigmoidCircleStroke = "blue"
  private val identityCircleStroke = "orange"
  private val tanhCircleStroke = "yellow"
  private val leakyReLUCircleStroke = "pink"

  private val positiveLineStroke = "lawngreen"
  private val negativeLineStroke = "palevioletred"

  private def activationNameToStroke(activName: String): String =
    activName match {
      case "sigmoid" => sigmoidCircleStroke
      case "identity" => identityCircleStroke
      case "tanh" => tanhCircleStroke
      case "leakyReLU" => leakyReLUCircleStroke
    }

  private def layerWidth(numOfCircles: Int)
  : Double =
    circleVerticalDistance + numOfCircles *
      (2 * circleRadius + circleVerticalDistance)

  private def circleCX(chainOrder: Int)
  : Double =
    circleHorizontalDistance +
      chainOrder * circleHorizontalDistance

  private def circleCY(maxChainCircles: Int,
                       numOfChainCircles: Int,
                       circleOrder: Int)
  : Double = {
    val currentLayerWidth = layerWidth(numOfChainCircles)
    val maxLayerWidth = layerWidth(maxChainCircles)
    val layerY = (maxLayerWidth - currentLayerWidth) / 2
    layerY + (circleOrder + 1) *
      (2 * circleRadius + circleVerticalDistance)
  }

  private def circleSVG(x: Double,
                        y: Double,
                        stroke_val: String)
  : svgTags.ConcreteHtmlTag[String] =
    circle(
      cx := x,
      cy := y,
      r := circleRadius,
      stroke := stroke_val,
      strokeWidth := circleStrokeWidth,
      fill := circleFill
    )

  private def inputCirclesSVGs(maxChainCircles: Int,
                               inputCircles: Int)
  : LazyList[svgTags.ConcreteHtmlTag[String]] =
    LazyList
      .range(0, inputCircles)
      .map { i =>
        val x = circleCX(0)
        val y = circleCY(maxChainCircles, inputCircles, i)
        val stroke_val =
          if (i == 0) biasCircleStroke
          else inputCircleStroke
        circleSVG(x, y, stroke_val)
      }

  private def outputCirclesSVGs(maxChainCircles: Int,
                                outputChainOrder: Int,
                                outputActivations: LazyList[String])
  : LazyList[svgTags.ConcreteHtmlTag[String]] =
    outputActivations
      .zipWithIndex
      .map { case (activ, i) =>
        val x = circleCX(outputChainOrder)
        val y = circleCY(maxChainCircles, outputActivations.length, i)
        val stroke_val = activationNameToStroke(activ)
        circleSVG(x, y, stroke_val)
      }

  private def hiddenCirclesSVGs(maxChainCircles: Int,
                                hiddenChainOrder: Int,
                                hiddenActivations: LazyList[String])
  : LazyList[svgTags.ConcreteHtmlTag[String]] =
    hiddenActivations
      .map(Option(_))
      .prepended(None)
      .zipWithIndex
      .map { case (activ, i) =>
        val x = circleCX(hiddenChainOrder)
        val y = circleCY(
          maxChainCircles,
          hiddenActivations.length + 1,
          i
        )
        val stroke_val = activ match {
          case None => biasCircleStroke
          case Some(act) => activationNameToStroke(act)
        }
        circleSVG(x, y, stroke_val)
      }

  private def layerCirclesSVGs(maxChainCircles: Int,
                               layerOrder: Int,
                               numOfLayers: Int,
                               layer: Layer)
  : LazyList[svgTags.ConcreteHtmlTag[String]] = {
    val isLastLayer = layerOrder == numOfLayers - 1
    val activations = layer.map(_.activationF.name)
    val inputCircles =
      if (layerOrder == 0) inputCirclesSVGs(
        maxChainCircles,
        layer.head.weights.length
      )
      else LazyList()
    val hiddenCircles =
      if (isLastLayer) LazyList()
      else hiddenCirclesSVGs(
        maxChainCircles,
        layerOrder + 1,
        activations
      )
    val outputCircles =
      if (isLastLayer)
        outputCirclesSVGs(
          maxChainCircles,
          layerOrder + 1,
          activations
        )
      else LazyList()
    inputCircles ++ hiddenCircles ++ outputCircles
  }

  private def lineSVG(maxChainCircles: Int,
                      baseChainOrder: Int,
                      numOfCirclesInBaseChain: Int,
                      numOfCirclesInTargetChain: Int,
                      baseCircleOrder: Int,
                      targetCircleOrder: Int,
                      weight: Double,
                      maxAbsWeight: Double)
  : svgTags.ConcreteHtmlTag[String] = {
    val alpha = Math.abs(weight) / maxAbsWeight
    val x1_val = circleCX(baseChainOrder)
    val y1_val = circleCY(
      maxChainCircles,
      numOfCirclesInBaseChain,
      baseCircleOrder
    )
    val x2_val = circleCX(baseChainOrder + 1)
    val y2_val = circleCY(
      maxChainCircles,
      numOfCirclesInTargetChain,
      targetCircleOrder
    )
    val stroke_val =
      if (weight > 0) positiveLineStroke
      else negativeLineStroke
    line(
      strokeOpacity := alpha,
      x1 := x1_val,
      y1 := y1_val,
      x2 := x2_val,
      y2 := y2_val,
      stroke := stroke_val,
      strokeWidth := lineStrokeWidth
    )
  }

  private def neuronLinesSVGs(maxChainCircles: Int,
                              layerSize: Int,
                              layerOrder: Int,
                              numOfLayers: Int,
                              neuronOrderInLayer: Int,
                              maxAbsWeight: Double,
                              weights: LazyList[Double])
  : LazyList[svgTags.ConcreteHtmlTag[String]] = {
    val isOutputLayer =
      layerOrder == numOfLayers - 1
    val numOfCirclesInBaseChain =
      weights.length
    val numOfCirclesInTargetChain =
      if (isOutputLayer) layerSize
      else layerSize + 1
    val targetCircleOrder =
      if (isOutputLayer) neuronOrderInLayer
      else neuronOrderInLayer + 1
    weights
      .zipWithIndex
      .map { case (w, i) =>
        lineSVG(
          maxChainCircles,
          layerOrder,
          numOfCirclesInBaseChain,
          numOfCirclesInTargetChain,
          i,
          targetCircleOrder,
          w,
          maxAbsWeight
        )
      }
  }

  private def layerLinesSVGs(maxChainCircles: Int,
                             layerOrder: Int,
                             numOfLayers: Int,
                             maxAbsWeight: Double,
                             layer: Layer)
  : LazyList[svgTags.ConcreteHtmlTag[String]] =
    layer
      .zipWithIndex
      .flatMap { case (neuron, neuronOrderInLayer) =>
        neuronLinesSVGs(
          maxChainCircles,
          layer.length,
          layerOrder,
          numOfLayers,
          neuronOrderInLayer,
          maxAbsWeight,
          neuron.weights
        )
      }

  def networkSVG(network: Network)
  : svgTags.ConcreteHtmlTag[String] = {
    val maxChainCircles =
      network
        .zipWithIndex
        .map { case (layer, i) =>
          if (i == network.length - 1)
            layer.length + 1
          else layer.length
        }
        .max
    val numOfLayers = network.length
    val absWeight = for {
      layer <- network
      neuron <- layer
      weight <- neuron.weights
    } yield Math.abs(weight)
    val maxAbsWeight = absWeight.max
    val circlesSVGs = network
      .zipWithIndex
      .flatMap { case (layer, i) =>
        layerCirclesSVGs(
          maxChainCircles,
          i,
          numOfLayers,
          layer
        )
      }
    val linesSVGs = network
      .zipWithIndex
      .flatMap { case (layer, i) =>
        layerLinesSVGs(
          maxChainCircles,
          i,
          numOfLayers,
          maxAbsWeight,
          layer
        )
      }
    val w = circleCX(numOfLayers + 1)
    val h = circleCY(
      maxChainCircles,
      maxChainCircles,
      maxChainCircles
    )
    val netSVGs = linesSVGs ++ circlesSVGs
    svg(widthA := w, heightA := h)(netSVGs: _*)
  }

}