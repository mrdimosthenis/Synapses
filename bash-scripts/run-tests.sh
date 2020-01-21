cd ../

cd test-projects/FSharpTest
dotnet restore
dotnet test
cd ../../

cd test-projects/CSharpTest
dotnet restore
dotnet test
cd ../../

cd test-projects/ScalaTest
sbt test
cd ../../

cd test-projects/JavaScriptTest
npm update
npm test
cd ../../

cd test-projects/JavaTest
mvn clean test
cd ../../
