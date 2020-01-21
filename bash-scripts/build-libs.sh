cd ../
cd implementation

cd FSharp
rm -r bin
rm -r obj
rm README.md
dotnet restore
dotnet build --configuration Release
dotnet pack --configuration Release
cp ../../README.md README.md
cd ../

cd CSharp
rm -r bin
rm -r obj
rm README.md
dotnet restore
dotnet build --configuration Release
dotnet pack --configuration Release
cp ../../README.md README.md
cd ../

cd Scala
rm -r target
rm README.md
sbt assembly
cp ../../README.md README.md
cd ../

cd ScalaJS
rm -r target
rm README.md
sbt fullOptJS
cp ../../README.md README.md
cd ../

cd JavaScript
rm -r target
rm synapses-opt.js
rm README.md
cp ../ScalaJS/target/scala-2.13/synapses-opt.js synapses-opt.js
cp ../../README.md README.md
npm build
cd ../
