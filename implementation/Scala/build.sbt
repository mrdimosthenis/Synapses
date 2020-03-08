name := "synapses"

version := "7.3.0"

scalaVersion := "2.13.1"

libraryDependencies ++= Seq(
  "io.circe" %% "circe-core" % "0.12.3",
  "io.circe" %% "circe-generic" % "0.12.3",
  "io.circe" %% "circe-parser" % "0.12.3"
)

organization := "com.github.mrdimosthenis"
homepage := Some(url("https://github.com/mrdimosthenis/Synapses"))
scmInfo := Some(
  ScmInfo(url("https://github.com/mrdimosthenis/Synapses"),
    "git@github.com:mrdimosthenis/Synapses.git")
)
developers := List(Developer(
  "mrdimosthenis",
  "Dimosthenis Michailidis",
  "mrdimosthenis@hotmail.com",
  url("https://github.com/mrdimosthenis")
))
licenses += ("MIT", url("https://rem.mit-license.org/"))
publishMavenStyle := true
publishTo := Some(
  if (isSnapshot.value)
    Opts.resolver.sonatypeSnapshots
  else
    Opts.resolver.sonatypeStaging
)
