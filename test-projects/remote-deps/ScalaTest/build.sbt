name := "ScalaTest"

version := "0.1"

scalaVersion := "2.13.1"

libraryDependencies ++= Seq(
  "com.github.mrdimosthenis" %% "synapses" % "7.3.1",
  "io.monix" %% "minitest" % "2.7.0" % "test",
  "com.github.tototoshi" %% "scala-csv" % "1.3.6"
)

testFrameworks += new TestFramework("minitest.runner.Framework")
