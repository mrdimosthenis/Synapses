var assert = require('assert');
require('synapses');

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
