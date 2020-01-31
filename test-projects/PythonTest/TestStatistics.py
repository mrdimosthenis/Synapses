import unittest

from synapses_py import Statistics

expectedWithOutputValuesList = [
    ([0.0, 0.0, 1.0], [0.0, 0.0, 1.0]),
    ([0.0, 0.0, 1.0], [0.0, 1.0, 1.0])
]

expectedWithOutputValuesIterator = iter(expectedWithOutputValuesList)


class TestStatistics(unittest.TestCase):
    def test_rootMeanSquareError(self):
        self.assertEqual(
            0.7071067811865476,
            Statistics.rootMeanSquareError(expectedWithOutputValuesIterator)
        )


if __name__ == '__main__':
    unittest.main()
