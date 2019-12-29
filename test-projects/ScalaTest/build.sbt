name := "ScalaTest"

version := "0.1"

scalaVersion := "2.13.1"

unmanagedBase := file("../../Scala/target/scala-2.13/")

libraryDependencies ++= Seq(
  "io.monix" %% "minitest" % "2.7.0" % "test",
  "com.github.tototoshi" %% "scala-csv" % "1.3.6"
)

testFrameworks += new TestFramework("minitest.runner.Framework")
