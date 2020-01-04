var assert = require('assert');
require('../../../Scala/target/scala-2.13/synapses-opt.js');

describe('statistics tests', function () {

    let expectedWithOutputValuesArr =
            [ [ [ 0.0, 0.0, 1.0], [ 0.0, 0.0, 1.0] ],
              [ [ 0.0, 0.0, 1.0], [ 0.0, 1.0, 1.0] ] ];

    let expectedWithOutputValuesIter = expectedWithOutputValuesArr[Symbol.iterator]();

    it('root mean square error', function () {
        assert.deepEqual(
            Statistics.rootMeanSquareError(expectedWithOutputValuesIter),
            0.7071067811865476
        );
    });

});
