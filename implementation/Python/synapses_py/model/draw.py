from drawSvg import Circle, Line, Drawing
from functional import seq
from functional.pipeline import Sequence

from synapses_py.model import utilities
from synapses_py.model.net_elems.layer import Layer
from synapses_py.model.net_elems.network import Network

pixels = 400.0

circleVerticalDistance = pixels * 0.02
circleHorizontalDistance = pixels * 0.15

circleRadius = pixels * 0.03
circleStrokeWidth = pixels / 150

lineStrokeWidth = pixels / 300

circleFill = 'white'

inputCircleStroke = 'brown'
biasCircleStroke = 'black'

sigmoidCircleStroke = 'blue'
identityCircleStroke = 'orange'
tanhCircleStroke = 'yellow'
leakyReLUCircleStroke = 'pink'

positiveLineStroke = 'lawngreen'
negativeLineStroke = 'palevioletred'


def activationNameToStroke(activName: str) -> str:
    return {
        'sigmoid': sigmoidCircleStroke,
        'identity': identityCircleStroke,
        'tanh': tanhCircleStroke,
        'leakyReLU': leakyReLUCircleStroke,
    }[activName]


def layerWidth(numOfCircles: int) -> float:
    return circleVerticalDistance + numOfCircles * \
           (2 * circleRadius + circleVerticalDistance)


def circleCX(chainOrder: int) -> float:
    return circleHorizontalDistance + \
           chainOrder * circleHorizontalDistance


def circleCY(maxChainCircles: int,
             numOfChainCircles: int,
             circleOrder: int) -> float:
    currentLayerWidth = layerWidth(numOfChainCircles)
    maxLayerWidth = layerWidth(maxChainCircles)
    layerY = (maxLayerWidth - currentLayerWidth) / 2
    return layerY + (circleOrder + 1) * \
           (2 * circleRadius + circleVerticalDistance)


def circleSVG(x: float,
              y: float,
              stroke_val: str) -> Circle:
    return Circle(
        cx=x,
        cy=y,
        r=circleRadius,
        stroke=stroke_val,
        stroke_width=circleStrokeWidth,
        fill=circleFill
    )


def inputCirclesSVGs(maxChainCircles: int,
                     inputCircles: int) -> Sequence:
    return seq \
        .range(inputCircles) \
        .map(lambda i:
             circleSVG(
                 circleCX(0),
                 circleCY(maxChainCircles, inputCircles, i),
                 biasCircleStroke if i == 0 else inputCircleStroke))


def outputCirclesSVGs(maxChainCircles: int,
                      outputChainOrder: int,
                      outputActivations: Sequence) -> Sequence:
    return outputActivations \
        .zip_with_index() \
        .map(lambda t:
             circleSVG(
                 circleCX(outputChainOrder),
                 circleCY(maxChainCircles, outputActivations.size(), t[1]),
                 activationNameToStroke(t[0])))


def hiddenCirclesSVGs(maxChainCircles: int,
                      hiddenChainOrder: int,
                      hiddenActivations: Sequence) -> Sequence:
    return utilities \
        .lazy_cons(None, hiddenActivations) \
        .zip_with_index() \
        .map(lambda t:
             circleSVG(
                 circleCX(hiddenChainOrder),
                 circleCY(
                     maxChainCircles,
                     hiddenActivations.size() + 1,
                     t[1]
                 ),
                 biasCircleStroke if t[0] is None else activationNameToStroke(t[0])))


def layerCirclesSVGs(maxChainCircles: int,
                     layerOrder: int,
                     numOfLayers: int,
                     layer_val: Layer) -> Sequence:
    isLastLayer = layerOrder == numOfLayers - 1
    activations = layer_val \
        .map(lambda neuron_val: neuron_val.activation_f.name)
    if layerOrder == 0:
        inputCircles = inputCirclesSVGs(
            maxChainCircles,
            layer_val.head().weights.size()
        )
    else:
        inputCircles = seq([])
    if isLastLayer:
        hiddenCircles = seq([])
    else:
        hiddenCircles = hiddenCirclesSVGs(
            maxChainCircles,
            layerOrder + 1,
            activations
        )
    if isLastLayer:
        outputCircles = outputCirclesSVGs(
            maxChainCircles,
            layerOrder + 1,
            activations
        )
    else:
        outputCircles = seq([])
    return inputCircles + hiddenCircles + outputCircles


def lineSVG(maxChainCircles: int,
            baseChainOrder: int,
            numOfCirclesInBaseChain: int,
            numOfCirclesInTargetChain: int,
            baseCircleOrder: int,
            targetCircleOrder: int,
            weight: float,
            maxAbsWeight: float) -> Line:
    alpha = abs(weight) / maxAbsWeight
    x1_val = circleCX(baseChainOrder)
    y1_val = circleCY(
        maxChainCircles,
        numOfCirclesInBaseChain,
        baseCircleOrder
    )
    x2_val = circleCX(baseChainOrder + 1)
    y2_val = circleCY(
        maxChainCircles,
        numOfCirclesInTargetChain,
        targetCircleOrder
    )
    stroke_val = positiveLineStroke if weight > 0 else negativeLineStroke
    return Line(
        stroke_opacity=alpha,
        sx=x1_val,
        sy=y1_val,
        ex=x2_val,
        ey=y2_val,
        stroke=stroke_val,
        stroke_width=lineStrokeWidth
    )


def neuronLinesSVGs(maxChainCircles: int,
                    layerSize: int,
                    layerOrder: int,
                    numOfLayers: int,
                    neuronOrderInLayer: int,
                    maxAbsWeight: float,
                    weights: Sequence) -> Sequence:
    isOutputLayer = \
        layerOrder == numOfLayers - 1
    numOfCirclesInBaseChain = weights.size()
    if isOutputLayer:
        numOfCirclesInTargetChain = layerSize
    else:
        numOfCirclesInTargetChain = layerSize + 1
    if isOutputLayer:
        targetCircleOrder = neuronOrderInLayer
    else:
        targetCircleOrder = neuronOrderInLayer + 1
    return weights \
        .zip_with_index() \
        .map(lambda t:
             lineSVG(
                 maxChainCircles,
                 layerOrder,
                 numOfCirclesInBaseChain,
                 numOfCirclesInTargetChain,
                 t[1],
                 targetCircleOrder,
                 t[0],
                 maxAbsWeight))


def layerLinesSVGs(maxChainCircles: int,
                   layerOrder: int,
                   numOfLayers: int,
                   maxAbsWeight: float,
                   layer_val: Layer) -> Sequence:
    return layer_val \
        .zip_with_index() \
        .flat_map(lambda t:
                  neuronLinesSVGs(
                      maxChainCircles,
                      layer_val.size(),
                      layerOrder,
                      numOfLayers,
                      t[1],
                      maxAbsWeight,
                      t[0].weights))


def networkSVG(network_val: Network) -> Drawing:
    maxChainCircles = \
        network_val \
            .zip_with_index() \
            .map(lambda t:
                 t[0].size() + 1 if t[1] == network_val.size() - 1 \
                     else t[0].size()) \
            .max()
    numOfLayers = network_val.size()
    maxAbsWeight = network_val \
        .flat_map(
        lambda layer_val:
        layer_val.flat_map(
            lambda neuron_val:
            neuron_val.weights.map(
                lambda weight:
                abs(weight)
            )
        )
    ).max()
    circlesSVGs = network_val \
        .zip_with_index() \
        .flat_map(lambda t:
                  layerCirclesSVGs(
                      maxChainCircles,
                      t[1],
                      numOfLayers,
                      t[0]))
    linesSVGs = network_val \
        .zip_with_index() \
        .flat_map(lambda t:
                  layerLinesSVGs(
                      maxChainCircles,
                      t[1],
                      numOfLayers,
                      maxAbsWeight,
                      t[0]))
    w = circleCX(numOfLayers + 1)
    h = circleCY(
        maxChainCircles,
        maxChainCircles,
        maxChainCircles
    )
    netSVGs = linesSVGs + circlesSVGs
    drawing = Drawing(w, h)
    for shape in netSVGs:
        drawing.append(shape)
    return drawing
