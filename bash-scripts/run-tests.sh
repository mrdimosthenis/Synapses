cd ../
cd test-projects

cd FSharpTest
rm -r bin
rm -r obj
dotnet restore
dotnet test
cd ../

cd CSharpTest
rm -r bin
rm -r obj
dotnet restore
dotnet test
cd ../

cd ScalaTest
rm -r project/target
rm -r target
sbt test
cd ../

cd JavaTest
rm -r target
mvn clean test
cd ../

cd JavaScriptTest
rm -r node_modules
npm install
npm test
cd ../

cd PythonTest
rm -r dist
poetry build
source venv/bin/activate
python3 -m unittest discover test
deactivate
cd ../
