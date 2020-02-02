name := "ScalaTest"

version := "0.1"

scalaVersion := "2.13.1"

libraryDependencies ++= Seq(
  //"com.github.mrdimosthenis" % "synapses" % "7.2.0"
  //  from "file:////home/dimos/IdeaProjects/multilang/Synapses/implementation/Scala/target/scala-2.13/synapses-assembly-7.2.0.jar",
  "com.github.mrdimosthenis" %% "synapses" % "7.2.0",
  "io.monix" %% "minitest" % "2.7.0" % "test",
  "com.github.tototoshi" %% "scala-csv" % "1.3.6"
)

testFrameworks += new TestFramework("minitest.runner.Framework")
