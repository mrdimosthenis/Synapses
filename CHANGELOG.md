## [7.4.1]
### Added
- Support for the Elixir language
- Newtonsoft.Json dependency for DotNet.
- Support for Python versions 3.7, 3.8, 3.9.

### Changed
- _Nothing has been changed._

### Removed
- System.Text.Json dependency for DotNet.

## [7.3.1]
### Added
- JetBrains reference in README.
- Throw exception when the number of input/expected values does not match the size of input/output layer.
- Restrict input and output/expected values to prevent number overflows.

### Changed
- _Nothing has been changed._

### Removed
- _Nothing has been removed._

## [7.3.0]
### Added
- NeuralNetwork can be visualized with svg drawing.

### Changed
- _Nothing has been changed._

### Removed
- _Nothing has been removed._

## [7.2.1]
### Added
- NeuralNetwork and DataPreprocessor instances are forced to be realized.

### Changed
- README is more readable.

### Removed
- _Nothing has been removed._

## [7.1.1]
### Added
- Java interface, tests and slate docs.
- Symlinks

### Changed
- Namespaces of implementation.
- JVM instances are deployed to maven.
- Fix preprocessor example in slate.

### Removed
- Patches for facades.

## [7.1.0]
### Added
- Create and use patches for facades.
- Automate the build and test projects for all languages.
- Introduce `Statistics.rootMeanSquareError` in documentation and wright tests for it.

### Changed
- Create and use slate docs instead of readme.
- `Statistics.rootMeanSquareError` works with stream/seq of tuples.

### Removed
- _Nothing has been removed._
