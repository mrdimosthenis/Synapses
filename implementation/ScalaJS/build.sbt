enablePlugins(ScalaJSPlugin)

name := "synapses"

version := "7.3.1"

scalaVersion := "2.13.1"

scalaJSUseMainModuleInitializer := false

libraryDependencies ++= Seq(
  "io.circe" %%% "circe-core" % "0.12.3",
  "io.circe" %%% "circe-generic" % "0.12.3",
  "io.circe" %%% "circe-parser" % "0.12.3"
)
