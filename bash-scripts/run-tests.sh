cd ../

cd test-projects/F#Test
dotnet restore
dotnet test
cd ../../

cd test-projects/ScalaTest
sbt test
cd ../../

cd test-projects/JavaScriptTest
npm test
cd ../../

cd test-projects/JavaTest
mvn clean test
cd ../../
