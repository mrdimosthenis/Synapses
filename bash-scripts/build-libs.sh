cd ../
cd implementation

# F#
cd DotNet/Synapses
rm -r bin
rm -r obj
rm README.md
cp ../../../README.md README.md
dotnet restore
dotnet build --configuration Release
dotnet pack --configuration Release
cd ../../

# C#
cd DotNet/SynapsesCSharp
rm -r bin
rm -r obj
rm README.md
cp ../../../README.md README.md
dotnet restore
dotnet build --configuration Release
dotnet pack --configuration Release
cd ../../

cd Scala
rm -r project/target
rm -r target
rm README.md
cp ../../README.md README.md
sbt assembly
cd ../

cd ScalaJS
rm -r project/target
rm -r target
rm README.md
cp ../../README.md README.md
sbt fullOptJS
cd ../

cd JavaScript
rm -r target
rm synapses-opt.js
rm README.md
cp ../../README.md README.md
cp ../ScalaJS/target/scala-2.13/synapses-opt.js synapses-opt.js
npm build
cd ../

cd Python
rm -r dist
rm README.md
cp ../../README.md README.md
poetry build
cd ../

cd Elixir
rm -r _build
rm -r deps
cp ../../README.md README.md
mix deps.get
mix compile
cd ../
