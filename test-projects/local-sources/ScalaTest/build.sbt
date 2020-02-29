name := "ScalaTest"

version := "0.1"

scalaVersion := "2.13.1"

libraryDependencies ++= Seq(
  "io.circe" %% "circe-core" % "0.12.3",
  "io.circe" %% "circe-generic" % "0.12.3",
  "io.circe" %% "circe-parser" % "0.12.3",
  "com.lihaoyi" %% "scalatags" % "0.8.2",
  "io.monix" %% "minitest" % "2.7.0" % "test",
  "com.github.tototoshi" %% "scala-csv" % "1.3.6"
)

testFrameworks += new TestFramework("minitest.runner.Framework")
